namespace ChessEngine {
    public class Square: Piece {
        public bool empty = true;

        public object Copy()
        {
            return MemberwiseClone();
        }

        public Square(Piece Piece, bool squareEmpty = false)
        {
            pos = Piece.pos;
            piece = Piece.piece;
            isWhite = Piece.isWhite;
            empty = squareEmpty;
            pin = Piece.pin;
        }

        public Square() {
            
        }
    }
}