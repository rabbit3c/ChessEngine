
namespace ChessEngine {

    public class Piece {
        public (int x, int y) pos;
        public bool isWhite;

        public int piece;
        public const int Pawn = 0;
        public const int Knight = 1;
        public const int Bishop = 2;
        public const int Rook = 3;
        public const int Queen = 4;
        public const int King = 5;

        public Piece((int, int) posPiece = new(), bool whitePiece = true, int pieceType = 0)
        {
            pos = posPiece;
            piece = pieceType;
            isWhite = whitePiece;
        }
    }
}