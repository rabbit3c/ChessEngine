
namespace ChessEngine
{
    class Search
    {
        static List<Piece> ownPieces = new();
        public static void Main(Position pos)
        {
            ownPieces = pos.OwnPieces();
            
            //getting every possible move
            Console.WriteLine("Legal Moves in this position:");
            for (int i = 0; i < ownPieces.Count; i++) {
                switch (ownPieces[i].piece) {
                    case Piece.King:
                        King.LegalMoves(posKing: posPiece(i), pos);
                        break;
                    case Piece.Queen:
                        Queen.LegalMoves(posQueen: posPiece(i), pos);
                        break;
                    case Piece.Bishop:
                        Bishop.LegalMoves(posBishop: posPiece(i), pos);
                        break;
                    case Piece.Rook:
                        Rook.LegalMoves(posRook: posPiece(i), pos);
                        break;
                    case Piece.Knight:
                        Knight.LegalMoves(posKnight: posPiece(i), pos);
                        break;
                    default:
                        Pawn.LegalMoves(posPawn: posPiece(i), pos);
                        break;
                }
            }
        }

        static (int, int) posPiece(int i) {
            Piece piece = ownPieces[i];
            return (piece.pos.x, piece.pos.y);
        }
    }
}