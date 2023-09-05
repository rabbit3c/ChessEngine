namespace ChessEngine {
    public class Transpositions {
        public static readonly Dictionary<ulong, TranspositionInfo> lookupTable =  new();

        public static void Add(Position pos, int resultingPositions, int depth, int eval) {
            ulong hash = pos.Hash();
            TranspositionInfo transposInfo = new() {
                resultingPositions = resultingPositions,
                depth = depth,
                eval = eval
            };
            lookupTable.Add(hash, transposInfo);
        }
    }

    public struct TranspositionInfo {
        public int depth;
        public int resultingPositions;
        public int eval;
    }
}