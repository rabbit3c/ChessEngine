
namespace ChessEngine
{
    struct Move
    {
        public static bool Unobstructed(int pos, bool isWhite, Position position)
        {
            Square targetSquare = position.Board[pos];
            if (!targetSquare.empty)
                if (targetSquare.isWhite == isWhite)
                    return false;
            return true;
        }

        public static bool NothingInTheWay(int oldPos, int newPos, Position pos)
        {
            if (oldPos.Y() == newPos.Y())
            {
                if (Math.Abs(oldPos - newPos) > 1)
                {
                    Square[] line = pos.GetRank(oldPos, newPos);
                    foreach (Square square in line)
                    {
                        if (!square.empty)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (oldPos.X() == newPos.X())
            {
                if (Math.Abs(oldPos - newPos) > 8)
                {
                    Square[] column = pos.GetFile(oldPos, newPos);
                    foreach (Square square in column)
                    {
                        if (!square.empty)
                            return false;
                    }
                }
                return true;
            }
            else if (Math.Abs(oldPos.X() - newPos.X()) == Math.Abs(oldPos.Y() - newPos.Y()))
            {
                if (Math.Abs(oldPos - newPos) >= 14)
                {
                    Square[] diagonal = pos.GetDiagonal(oldPos, newPos);
                    foreach (Square square in diagonal)
                    {
                        if (!square.empty)
                            return false;
                    }
                }
                return true;
            }
            return false; //if none of the Conditions were right, they can't be on the same line
        }

        public static List<int> SlidingMoves(Piece piece, Position pos)
        {
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };
            bool[] allowedDirections = piece.IsPinned(pos, out bool _);
            List<int> legalMoves = new();
            for (int i = piece.piece == Piece.Bishop ? 4 : 0; i < (piece.piece == Piece.Rook ? 4 : directions.Length); i++)
            {
                if (allowedDirections[Math.DivRem(i, 2, out int _)])
                {
                    for (int n = 0; n < PrecomputedData.numSquareToEdge[piece.pos][i]; n++)
                    {
                        int move = piece.pos + directions[i] * (n + 1);
                        if (Unobstructed(move, pos.WhitesTurn, pos))
                        {
                            legalMoves.Add(move);
                        }
                        else if (move != piece.pos)
                            break;
                        if (!Unobstructed(move, !pos.WhitesTurn, pos))
                        {
                            break;
                        }
                    }
                }
            }
            return legalMoves;
        }
    }
}