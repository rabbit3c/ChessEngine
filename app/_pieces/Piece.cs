
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

        public bool[] IsPinned(Position position, out bool pinned)
        {
            bool[] isPinned = { false, false, false, false }; //North-South, West-East, Northeast - Southwest, Northwest - Southeast
            pinned = true;
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

            if (pos.X() == posKing.X())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.X() == posKing.X())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos && pos < posKing) || (i > pos && pos > posKing))
                                if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                {
                                    isPinned[0] = true;
                                    return isPinned;
                                }
            }

            else if (pos.Y() == posKing.Y())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.Y() == posKing.Y())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos && pos < posKing) || (i > pos && pos > posKing))
                                if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                {
                                    isPinned[1] = true;
                                    return isPinned;
                                }
            }

            else if (Math.Abs(pos.Y() - posKing.Y()) == Math.Abs(pos.X() - posKing.X()))
            {
                Math.DivRem(pos - posKing, 9, out int remainder);
                if (remainder == 0)
                {
                    foreach (int i in position.EnemyPieces())
                    {
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Bishop)
                        {
                            if (Math.Abs(Pieces[i].pos.Y() - posKing.Y()) == Math.Abs(Pieces[i].pos.X() - posKing.X()))
                            {
                                if ((i.X() < pos.X() && pos.X() < posKing.X()) || (i.X() > pos.X() && pos.X() > posKing.X()))
                                    if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                    {
                                        isPinned[2] = true;
                                        return isPinned;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    foreach (int i in position.EnemyPieces())
                    {
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Bishop)
                        {
                            if (Math.Abs(Pieces[i].pos.Y() - posKing.Y()) == Math.Abs(Pieces[i].pos.X() - posKing.X()))
                            {
                                if ((i.X() < pos.X() && pos.X() < posKing.X()) || (i.X() > pos.X() && pos.X() > posKing.X()))
                                    if (Move.NothingInTheWay(posKing, pos, position) && Move.NothingInTheWay(pos, i, position))
                                    {
                                        isPinned[3] = true;
                                        return isPinned;
                                    }
                            }
                        }
                    }
                }
            }
            pinned = false;
            bool[] defaultValues = { true, true, true, true };
            return defaultValues;
        }

        public bool Promoting()
        {
            if (piece == Pawn)
                return isWhite ? pos.Y() == 8 : pos.Y() == 1;
            return false;
        }
    }
}