using System.Numerics;

namespace Tournoi.Balancer
{
    public struct EQTeam
    {
        public ulong cities;
        public ulong available;
        public ulong members;
        public int index;
        public int count;
        public int third;
        public bool hasFemale;
        // TODO: add field for sex for balancing

        public int[] GetPlayers()
        {
            int[] players = new int[count];
            int index = 0;
            for (int i = 0; i < 64; i++)
            {
                if (index == count)
                {
                    break;
                }

                if (((members >> i) % 2) != 0)
                {
                    players[index] = i;
                    i++;
                }
            }
            return players;
        }

        public void AddPlayer(EQPlayer player)
        {
            cities |= player.cityMask;
            members |= player.indexMask;
            available &= ~player.indexMask;
            if (player.sex == Sex.Female)
            {
                hasFemale = true;
            }
            count++;
        }

        public void RemovePlayer(EQPlayer player)
        {
            cities &= ~player.cityMask;
            members &= ~player.indexMask;
            available |= player.indexMask;
            if (player.sex == Sex.Female)
            {
                hasFemale = false;
            }
            count--;
        }
    }
}