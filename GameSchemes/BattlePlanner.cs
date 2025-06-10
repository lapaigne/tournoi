using System.Text.Json;

namespace Tournoi.Battles
{
    public class BattlePlanner
    {
        private readonly Dictionary<string, int[][]> predefined;
        private int[] players;
        private int size;

        public BattlePlanner()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "GameSchemes", "battleschemes.json");
            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"Scheme file not found: {jsonPath}");
            }

            var json = File.ReadAllText(jsonPath);

            predefined = JsonSerializer.Deserialize<Dictionary<string, int[][]>>(json) ?? new Dictionary<string, int[][]>();
        }

        public T[][] ApplyScheme<T>(string schemeName, T[] players)
        {
            if (!predefined.ContainsKey(schemeName))
            {
                throw new ArgumentException($"Scheme <{schemeName}> doesn't exist");
            }

            int[][] scheme = predefined[schemeName];

            if (players.Length != scheme.Length)
            {
                throw new ArgumentException($"Incompatible scheme: counts don't match");
            }

            T[][] result = scheme.Select(b => b.Select(i => players[i]).ToArray()).ToArray();
            return result;
        }
    }
}
