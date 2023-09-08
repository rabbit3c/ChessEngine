
namespace ChessEngine
{

    public class Piece
    {
        public int pos;
        public bool isWhite;
        public int piece;
        public const int Pawn = 0;
        public const int Knight = 1;
        public const int Bishop = 2;
        public const int Rook = 3;
        public const int Queen = 4;
        public const int King = 5;

        public Piece(int posPiece = new(), bool whitePiece = true, int pieceType = 0)
        {
            pos = posPiece;
            piece = pieceType;
            isWhite = whitePiece;
        }

        public virtual object Copy()
        {
            Piece copy = new(pos, isWhite, piece);
            return copy;
        }

        public bool IsPinned(int move, Position position)
        {
            int posKing = 0;
            List<Square> Pieces = position.Board;

            foreach (int i in position.OwnPieces())
            {
                if (position.Board[i].piece == King)
                {
                    posKing = i;
                    break;
                }
            }

            if (pos.X() == posKing.X() && move.X() != posKing.X())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.X() == posKing.X())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos && pos < posKing) || (i > pos && pos > posKing))
                                if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                    return true;
                return false;
            }

            else if (pos.Y() == posKing.Y() && move.Y() != posKing.Y())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.Y() == posKing.Y())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos && pos < posKing) || (i > pos && pos > posKing))
                                if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                    return true;
                return false;
            }

            else if (Math.Abs(pos.Y() - posKing.Y()) == Math.Abs(pos.X() - posKing.X()))
            {
                if (Math.Abs(move.Y() - posKing.Y()) != Math.Abs(move.X() - posKing.X()))
                {
                    foreach (int i in position.EnemyPieces())
                    {
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Bishop)
                        {
                            if (Math.Abs(Pieces[i].pos.Y() - posKing.Y()) == Math.Abs(Pieces[i].pos.X() - posKing.X())) {
                                if ((i.X() < pos.X() && pos.X() < posKing.X()) || (i.X() > pos.X() && pos.X() > posKing.X()))
                                    if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                        return true;
                            }
                        }
                    }
                }
                return false;
            }
            return false;
        }

        public bool Promoting()
        {
            if (piece == Pawn)
                return isWhite ? pos.Y() == 8 : pos.Y() == 1;
            return false;
        }
    }
}