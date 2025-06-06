using Tournoi.Models;

namespace Tournoi.Balancer
{
    public class EQBalancer
    {
        private delegate bool Condition(EQTeam team, EQPlayer player);
        private EQPlayer[] _players;
        private int _shortCount;
        private bool _found;
        private ulong _forcedCities;
        private ulong[] _solution;
        private Dictionary<string, int> _cityCount;
        private Dictionary<string, ulong> _cityMasks;
        private List<Condition> _skipConditions;

        public List<List<int>> GetSolution()
        {
            var result = new List<List<int>>();
            if (_solution == null) return result;
            for (int i = 0; i < _solution.Length; i++)
            {
                var teamMask = _solution[i];
                var team = new List<int>();
                for (int j = 0; j < 64; j++)
                {
                    if (((teamMask >> j) & 1UL) != 0)
                        team.Add(j);
                }
                if (team.Any())
                    result.Add(team);
            }
            return result;
        }

        public void PrepareData(PersonModel[] players)
        {
            _cityCount = new Dictionary<string, int>();
            _cityMasks = new Dictionary<string, ulong>();
            _skipConditions = new List<Condition>()
            {
                // same cities condition
                delegate (EQTeam team, EQPlayer player)
                {
                    return (team.cities & player.cityMask) != 0;
                },
                // forced cities condition
                delegate (EQTeam team, EQPlayer player)
                {
                    return team.count >= 2
                        && (_forcedCities & team.cities) == 0
                        && (_forcedCities & player.cityMask) == 0;
                },
                delegate (EQTeam team, EQPlayer player)
                {
                    return team.hasFemale && player.sex == Sex.Female;
                }
            };

            _shortCount = players.Length / 4;
            _solution = new ulong[_shortCount];

            int count = 0;

            foreach (PersonModel player in players)
            {
                if (!_cityCount.TryAdd(player.City, 1))
                {
                    _cityCount[player.City]++;
                }
                else
                {
                    _cityMasks[player.City] = 1UL << count;
                    count++;
                }
            }

            _players = players.Select(p => new EQPlayer
            {
                cityMask = _cityMasks[p.City],
                sex = p.Sex,
                rank = p.Rank
            }).OrderByDescending(p => p.rank)
                .ThenByDescending(p => p.sex)
                .ThenBy(p => p.cityMask)
                .ToArray();

            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].indexMask = 1UL << i;
            }

            var cities = _cityCount.Where(city => city.Value == _shortCount).Select(city => _cityMasks[city.Key]);
            _forcedCities = cities.Any() ? cities.Aggregate((acc, n) => acc |= n) : ~0UL;

            var p = players.Select(p => p.City + " " + _cityMasks[p.City]);
            var debug = _players.Select(p => p.cityMask);
            var ps = string.Join("\n", p);
            var ds = string.Join("\n", debug);
            /*throw new Exception(ps);*/
        }

        public void StartSearch()
        {
            EQTeam initial = new EQTeam();
            initial.AddPlayer(_players[0]);
            initial.index = 0;
            initial.available = ~0UL;
            Search(initial);
        }

        private void Search(EQTeam team)
        {
            if (_found) { return; }


            /*if (_solution.Last() != 0UL)*/
            if (_solution[1] != 0UL)
            {
                _found = true;
                return;
            }

            if (team.count == 4)
            {
                _solution[team.index] = team.members;

                team.members = default;
                team.cities = default;
                team.count = default;
                team.third = default;
                team.index++;

                team.AddPlayer(_players[team.index]);
                Search(team);
            }

            (int, int) range = team.count switch
            {
                1 => (_shortCount, _shortCount * 2),
                2 or 3 => (_shortCount * 2, _shortCount * 4),
            };

            if (team.third > range.Item1)
            {
                range.Item1 = team.third;
            }

            EQPlayer previous = new EQPlayer();

            for (int i = range.Item1; i < range.Item2; i++)
            {
                if ((team.available & _players[i].indexMask) == 0) { continue; }
                bool equality = (previous.cityMask == _players[i].cityMask)
                    && (previous.rank == _players[i].rank);

                if (equality) { continue; }
                previous = _players[i];

                bool skip = false;
                foreach (var condition in _skipConditions)
                {
                    if (condition(team, _players[i]))
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip) { continue; }

                team.AddPlayer(_players[i]);

                if (team.count == 3)
                {
                    team.third = i;
                }

                Search(team);
                team.RemovePlayer(_players[i]);
            }
        }
    }
}