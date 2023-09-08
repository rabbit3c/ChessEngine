
namespace ChessEngine
{
    class King
    {
        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            List<int> legalMoves = new();
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };

            for (int i = 0; i < directions.Length; i++)
            {
                if (PrecomputedData.numSquareToEdge[piece.pos][i] != 0)
                {
                    int move = piece.pos + directions[i];
                    if (Legal(move, piece.isWhite, pos) && piece.pos != move)
                        legalMoves.Add(move);
                }
            }

            if (NotInCheck(piece, piece.pos, pos))
            {
                if (pos.WhitesTurn)
                {
                    if (pos.WShortCastle)
                    {
                        if (Move.NothingInTheWay(4, 7, pos))
                            if (NotInCheck(piece, 5, pos) && NotInCheck(piece, 6, pos))
                                legalMoves.Add(6); //g1
                    }
                    if (pos.WLongCastle)
                    {
                        if (Move.NothingInTheWay(4, 0, pos))
                            if (NotInCheck(piece, 3, pos) && NotInCheck(piece, 2, pos))
                                legalMoves.Add(2); //c1
                    }
                }
                else
                {
                    if (pos.BShortCastle)
                    {
                        if (Move.NothingInTheWay(60, 63, pos))
                            if (NotInCheck(piece, 61, pos) && NotInCheck(piece, 62, pos))
                                legalMoves.Add(62); //g8
                    }
                    if (pos.BLongCastle)
                    {
                        if (Move.NothingInTheWay(60, 56, pos))
                            if (NotInCheck(piece, 59, pos) && NotInCheck(piece, 58, pos))
                                legalMoves.Add(58); //c8
                    }
                }
            }
            //string combinedString = string.Join( ", ", legalMoves);
            //Console.WriteLine($"King at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.piece}, {legalMoves.Count}");
            return legalMoves;
        }

        static bool Legal(int move, bool isWhite, Position pos)
        {
            if (Move.Unobstructed(move, isWhite, pos))
                return true;
            return false;
        }

        public static bool NotInCheck(Piece piece, int move, Position pos)
        {
            List<Position> newPositions = NewPos.Format(pos, piece, move);
            if (newPositions.Count == 0) // if there are zero new position, the move results in the king being taken
            {
                return false;
            }
            return true;
        }
    }
}