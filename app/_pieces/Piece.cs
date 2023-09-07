using System.Runtime.InteropServices;

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

        public virtual object Copy()
        {
            Piece copy = new(pos, isWhite, piece);
            return copy;
        }

        public bool IsPinned((int x, int y) move, Position position)
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

            if (pos.x == posKing.x() && move.x != posKing.x())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.x() == posKing.x())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos.PosXYToInt() && pos.PosXYToInt() < posKing) || (i > pos.PosXYToInt() && pos.PosXYToInt() > posKing))
                                if (Move.NothingInTheWay(posKing, pos.PosXYToInt(), position) && Move.NothingInTheWay(pos.PosXYToInt(), i, position))
                                    return true;
                return false;
            }

            else if (pos.y == posKing.y() && move.y != posKing.y())
            {
                foreach (int i in position.EnemyPieces())
                    if (i.y() == posKing.y())
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Rook)
                            if ((i < pos.PosXYToInt() && pos.PosXYToInt() < posKing) || (i > pos.PosXYToInt() && pos.PosXYToInt() > posKing))
                                if (Move.NothingInTheWay(posKing, pos.PosXYToInt(), position) && Move.NothingInTheWay(pos.PosXYToInt(), i, position))
                                    return true;
                return false;
            }

            else if (Math.Abs(pos.y - posKing.y()) == Math.Abs(pos.x - posKing.x()))
            {
                if (Math.Abs(move.y - posKing.y()) != Math.Abs(move.x - posKing.x()))
                {
                    foreach (int i in position.EnemyPieces())
                    {
                        if (Pieces[i].piece == Queen || Pieces[i].piece == Bishop)
                        {
                            if (Math.Abs(Pieces[i].pos.y - posKing.y()) == Math.Abs(Pieces[i].pos.x - posKing.x())) {
                                if ((i.x() < pos.x && pos.x < posKing.x()) || (i.x() > pos.x && pos.x > posKing.x()))
                                    if (Move.NothingInTheWay(posKing, pos.PosXYToInt(), position) && Move.NothingInTheWay(pos.PosXYToInt(), i, position))
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
                return isWhite ? pos.y == 8 : pos.y == 1;
            return false;
        }
    }
}