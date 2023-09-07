
namespace ChessEngine
{
    public class PrecomputedData
    {
        public static readonly int[][] numSquareToEdge = new int[64][];
        public static readonly Dictionary<int, ulong> hashes = new();

        public static void PrecomputedMoveData()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {

                    int numNorth = 7 - rank;
                    int numSouth = rank;
                    int numWest = file;
                    int numEast = 7 - file;

                    int squareIndex = rank * 8 + file;
                    numSquareToEdge[squareIndex] = new int[8] {
                        numNorth,
                        numSouth,
                        numWest,
                        numEast,
                        Math.Min(numNorth, numEast),
                        Math.Min(numSouth, numWest),
                        Math.Min(numNorth, numWest),
                        Math.Min(numSouth, numEast)
                    };
                }
            }
        }

        public static void GenerateZobristHashes()
        {
            Random random = new();

            for (int i = 0; i < 837; i++)
            {
                hashes.Add(i, (ulong)random.NextInt64() * (ulong)(random.NextDouble() + 1));
            }
        }

        public static void Precompute() {
            GenerateZobristHashes();
            PrecomputedMoveData();
        }
    }
}