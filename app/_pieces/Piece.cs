namespace ChessEngine
{

    public class Piece
    {
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

        public object Copy()
        {
            Piece copy = new(pos, isWhite, piece);
            return copy;
        }

        public bool IsPinned((int x, int y) move, Position position)
        {
            (int x, int y) posKing = (0, 0);
            foreach (Piece p in position.OwnPieces())
            {
                if (p.piece == King)
                {
                    posKing = p.pos;
                    break;
                }
            }

            if (pos.x == posKing.x && move.x != posKing.x)
            {
                foreach (Piece p in position.EnemyPieces())
                    if (p.piece == Queen || p.piece == Rook)
                        if (p.pos.x == posKing.x)
                            if (Move.NothingInTheWay(posKing, p.pos, position))
                                return true;
            }

            else if (pos.y == posKing.y && move.y != posKing.y)
            {
                foreach (Piece p in position.EnemyPieces())
                    if (p.piece == Queen || p.piece == Rook)
                        if (p.pos.y == posKing.y)
                            if (Move.NothingInTheWay(posKing, p.pos, position))
                                return true;
            }

            else
            {
                if (Math.Abs(pos.y - posKing.y) == Math.Abs(pos.x - posKing.x))
                {
                    if (Math.Abs(move.y - posKing.y) != Math.Abs(move.x - posKing.x))
                        foreach (Piece p in position.EnemyPieces())
                            if (p.piece == Queen || p.piece == Bishop)
                                if (Math.Abs(p.pos.y - posKing.y) == Math.Abs(p.pos.x - posKing.x))
                                    if (Move.NothingInTheWay(posKing, p.pos, position))
                                        return true;
                }
            }
            return false;
        }

        public bool Promoting() {
            if (piece == Pawn) 
                return isWhite ? pos.y == 8 : pos.y == 1;
            return false;
        }
    }
}