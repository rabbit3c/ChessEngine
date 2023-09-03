
namespace ChessEngine
{
    class Search
    {
        static List<Piece> ownPieces = new();

        public static void Main(List<Position> positions, int depth)
        {
            List<Position> newPositions = new();
            int newPos = 0;

            foreach (Position position in positions)
            {
                newPositions.AddRange(GeneratePositions(position, out int n));
                newPos += n;
            }

            Console.WriteLine($"Amount of new positions: {newPos}");

            depth--;
            if (depth == 0)
            {
                return;
            }

            Main(newPositions, depth);
        }
        public static List<Position> GeneratePositions(Position pos, out int newPos)
        {
            ownPieces = pos.OwnPieces();
            newPos = 0;
            List<Position> newPositions = new();

            //Console.WriteLine("Legal Moves in this position:");

            //getting every possible move
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