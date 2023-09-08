
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
            if (oldPos.Y() == newPos.Y() && Math.Abs(oldPos - newPos) > 1)
            {
                List<Square> line = pos.GetRank(oldPos, newPos);
                foreach (Square square in line)
                {
                    if (!square.empty)
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (oldPos.X() == newPos.X() && Math.Abs(oldPos - newPos) > 8)
            {
                List<Square> column = pos.GetFile(oldPos, newPos);
                foreach (Square square in column)
                {
                    if (!square.empty)
                        return false;
                }
                return true;
            }
            else
            {
                if (Math.Abs(oldPos - newPos) >= 14)
                {
                    List<Square> diagonal = pos.GetDiagonal(oldPos, newPos);
                    foreach (Square square in diagonal)
                    {
                        if (!square.empty)
                            return false;
                    }
                    return true;
                }
            }
            return true;
        }

        public static List<int> SlidingMoves(Piece piece, Position pos) {
            int[] directions = {8, -8, -1, 1, 9, -9, 7, -7};
            int posInt = piece.pos;
            List<int> legalMoves = new();
            for (int i = piece.piece == Piece.Bishop ? 4 : 0; i < (piece.piece == Piece.Rook ? 4 : directions.Length); i++)
            {
                for (int n = 0; n < PrecomputedData.numSquareToEdge[posInt][i]; n++)
                {
                    int move = posInt + directions[i] * (n + 1);
                    if (Unobstructed(move, pos.WhitesTurn, pos))
                    {

                        if (!piece.IsPinned(move, pos))
                        {
                            legalMoves.Add(move);
                        }
                    }
                    else if (move != posInt)
                        break;
                    if (!Unobstructed(move, !pos.WhitesTurn, pos))
                    {
                        break;
                    }
                }
            }
            return legalMoves;
        }
    }
}