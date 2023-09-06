namespace ChessEngine
{
    public class ZobristHashes
    {
        public static readonly Dictionary<int, ulong> hashes =  new();

        public static void GenerateHashes() {
            Random random = new();

            for (int i = 0; i < 837; i++) {
                hashes.Add(i, (ulong)random.NextInt64() * (ulong)(random.NextDouble() + 1));
            }
        }
    }
}