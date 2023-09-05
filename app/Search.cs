
namespace ChessEngine
{
    public class Search
    {
        static List<Piece> ownPieces = new();

        public static void Main(Position position, int depth, out int AmountPos)
        {
            int newPos = 0;
            AmountPos = 0; 

            depth--;

            if (depth == 0) {
                GeneratePositions(position, out int n);
                newPos += n;
                AmountPos = newPos;
                return;
            }

            List<Position> newPositions = GeneratePositions(position, out int _);

            foreach (Position newPosition in newPositions)
            {
                int AmountNewPos = 0;
                ulong hash = newPosition.Hash();
                if (Transpositions.lookupTable.ContainsKey(hash)) { // checking if position is in look up table
                    TranspositionInfo transposInfo = Transpositions.lookupTable[hash];
                    if (transposInfo.depth == depth) { //checking if transposition is at right depth
                        AmountNewPos = transposInfo.resultingPositions;
                    }
                    else {
                        Main(newPosition, depth, out AmountNewPos);
                    }
                }
                else { //if not generate resulting positions and add to Transposition table
                    Main(newPosition, depth, out AmountNewPos);
                    Transpositions.Add(newPosition, AmountNewPos, depth, 0);
                }
                AmountPos += AmountNewPos;
            }
        }
        public static List<Position> GeneratePositions(Position pos, out int newPos)
        {
            ownPieces = pos.OwnPieces();
            newPos = 0;
            List<Position> newPositions = new();

            for (int i = 0; i < ownPieces.Count; i++)
            {
                List<(int, int)> moves = ownPieces[i].piece switch
                {
                    Piece.King => King.LegalMoves(ownPieces[i], pos),
                    Piece.Queen => Queen.LegalMoves(ownPieces[i], pos),
                    Piece.Bishop => Bishop.LegalMoves(ownPieces[i], pos),
                    Piece.Rook => Rook.LegalMoves(ownPieces[i], pos),
                    Piece.Knight => Knight.LegalMoves(ownPieces[i], pos),
                    _ => Pawn.LegalMoves(ownPieces[i], pos),
                };
                newPositions.AddRange(NewPos.New(pos, ownPieces[i], moves, out int n));
                newPos += n;
            }
            return newPositions;
        }
    }
}