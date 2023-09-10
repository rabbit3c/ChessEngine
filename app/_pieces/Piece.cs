
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
            bool[] allowedDirections = { false, false, false, false }; //North-South, West-East, Northeast - Southwest, Northwest - Southeast
            bool[] defaultValues = { true, true, true, true };
            pinned = false;
            int posKing = position.OwnKing();

            if (pos.X() == posKing.X())
            {
                if (Move.NothingInTheWay(posKing, pos, position))
                {
                    List<Square> file = pos < posKing ? position.GetFile(pos.X(), pos - 8, true) : position.GetFile(pos + 8, pos.X() + 56, true);
                    pinned = CheckSquares(file, posKing, Rook);
                }
                if (pinned)
                {
                    allowedDirections[0] = true;
                    return allowedDirections;
                }
                return defaultValues;
            }

            else if (pos.Y() == posKing.Y())
            {
                if (Move.NothingInTheWay(posKing, pos, position))
                {
                    List<Square> rank = pos < posKing ? position.GetRank(pos.Y() * 8, pos - 1, true) : position.GetRank(pos + 1, pos.Y() * 8 + 7, true);
                    pinned = CheckSquares(rank, posKing, Rook);
                }
                if (pinned)
                {
                    allowedDirections[1] = true;
                    return allowedDirections;
                }
                return defaultValues;
            }

            else if (Math.Abs(pos.Y() - posKing.Y()) == Math.Abs(pos.X() - posKing.X()))
            {
                Math.DivRem(pos - posKing, 9, out int remainder);
                if (remainder == 0)
                {
                    if (Move.NothingInTheWay(posKing, pos, position))
                    {
                        List<Square> diagonal = pos < posKing ? position.GetDiagonal(pos - 9 * PrecomputedData.numSquareToEdge[pos][5], pos - 9, true) : position.GetDiagonal(pos + 9, pos + 9 * PrecomputedData.numSquareToEdge[pos][4], true);
                        pinned = CheckSquares(diagonal, posKing, Bishop);
                    }
                    if (pinned)
                    {
                        allowedDirections[2] = true;
                        return allowedDirections;
                    }
                    return defaultValues;
                }
                else
                {
                    if (Move.NothingInTheWay(posKing, pos, position))
                    {
                        List<Square> diagonal = pos < posKing ? position.GetDiagonal(pos - 7 * PrecomputedData.numSquareToEdge[pos][7], pos - 7, true) : position.GetDiagonal(pos + 7, pos + 7 * PrecomputedData.numSquareToEdge[pos][6], true);
                        pinned = CheckSquares(diagonal, posKing, Bishop);
                    }
                    if (pinned)
                    {
                        allowedDirections[3] = true;
                        return allowedDirections;
                    }
                    return defaultValues;
                }
            }
            return defaultValues;
        }

        public bool CheckSquares(List<Square> squares, int posKing, int AttackingPiece)
        {
            bool pinned = false;
            foreach (Square square in squares)
            {
                if (!square.empty)
                {
                    if ((square.piece == Queen || square.piece == AttackingPiece) && square.isWhite != isWhite)
                    {
                        pinned = true;
                        if (pos > posKing)
                            return pinned;
                    }
                    else
                    {
                        if (pos > posKing)
                        {
                            return false;
                        }
                        else
                        {
                            pinned = false;
                        }
                    }
                }
            }
            return pinned;
        }

        public bool Promoting()
        {
            if (piece == Pawn)
                return isWhite ? pos.Y() == 7 : pos.Y() == 0;
            return false;
        }
    }
}