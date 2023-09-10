namespace ChessEngine {
    public class Square: Piece {
        public bool empty = true;

        public override object Copy()
        {
            return MemberwiseClone();
        }

        public Square(int posPiece, bool whitePiece, int pieceType, bool squareEmpty = false)
        {
            pos = posPiece;
            piece = pieceType;
            isWhite = whitePiece;
            empty = squareEmpty;
        }

        public Square(Piece Piece, bool squareEmpty = false)
        {
            pos = Piece.pos;
            piece = Piece.piece;
            isWhite = Piece.isWhite;
            empty = squareEmpty;
        }

        public Square() {
            
        }
    }
}