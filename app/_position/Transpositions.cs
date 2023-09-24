namespace ChessEngine
{
    public class Transpositions
    {
        public static readonly Dictionary<ulong, TranspositionInfo> lookupTable = new();

        public static void Add(Position pos, int resultingPositions, int depth, int eval)
        {
            if (lookupTable.ContainsKey(pos.hash)) return;
                
            TranspositionInfo transposInfo = new()
            {
                resultingPositions = resultingPositions,
                depth = depth,
                eval = eval
            };
            lookupTable.Add(pos.hash, transposInfo);
        }
    }

    public struct TranspositionInfo
    {
        public int depth;
        public int resultingPositions;
        public int eval;
    }
}