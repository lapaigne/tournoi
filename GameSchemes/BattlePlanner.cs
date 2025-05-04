namespace Tournoi.Battles
{
    public class BattlePlanner
    {
        private Dictionary<string, int[][]> predefined;
        private int[] players;
        private int size;

        public BattlePlanner()
        {
            /*
                0b0000000000110011
                0b0000000000011110
                0b0000000000101101
                0b0000000000111001
                0b0000000000110110
                0b0000000000001111 
             */
            int[][] six = [
                [0, 1, 4, 5],
                [1, 2, 3, 4],
                [0, 2, 3, 5],
                [0, 3, 4, 5],
                [1, 2, 4, 5],
                [0, 1, 2, 3]
            ];

            int[][] seven = [
                [1, 2, 4, 5],
                [0, 3, 5, 6],
                [1, 3, 4, 5],
                [0, 1, 2, 4],
                [2, 3, 5, 6],
                [0, 4, 5, 6],
                [0, 1, 2, 3]
            ];

            predefined = new Dictionary<string, int[][]>
            {
                { "six", six },
                { "seven", seven }
            };

        }

        public T[][] ApplyScheme<T>(string schemeName, T[] players)
        {
            if (!predefined.ContainsKey(schemeName))
            {
                throw new ArgumentException($"Scheme <{schemeName}> doesn't exist");
            }

            int[][] scheme = predefined[schemeName];

            if (players.Count() != scheme.Count())
            {
                throw new ArgumentException($"Incompatible scheme: counts don't match");
            }

            T[][] result = scheme.Select(b => b.Select(i => players[i]).ToArray()).ToArray();
            return result;
        }
    }
}