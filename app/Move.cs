
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

        public static bool NothingInTheWay(int oldPos, int newPos, Position pos) {
            ulong mask = Bitboards.MaskLine(oldPos, newPos, out bool NotInLine);
            if (NotInLine) {
                return false;
            }
            return (pos.occupiedBB & mask) == 0;
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