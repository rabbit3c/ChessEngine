
namespace ChessEngine
{
    class King
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };

            for (int i = 0; i < directions.Length; i++)
            {
                if (PrecomputedData.numSquareToEdge[piece.pos.PosXYToInt()][i] != 0)
                {
                    int move = piece.pos.PosXYToInt() + directions[i];
                    if (Legal(move, piece.isWhite, pos) && piece.pos.PosXYToInt() != move)
                        legalMoves.Add(move.IntToPosXY());
                }
            }

            if (Move.NotInCheck(piece, piece.pos, pos))
            {
                if (pos.WhitesTurn)
                {
                    if (pos.WShortCastle)
                    {
                        if (Move.NothingInTheWay(4, 7, pos))
                            if (Move.NotInCheck(piece, (6, 1), pos) && Move.NotInCheck(piece, (7, 1), pos))
                                legalMoves.Add(6.IntToPosXY());
                    }
                    if (pos.WLongCastle)
                    {
                        if (Move.NothingInTheWay(4, 0, pos))
                            if (Move.NotInCheck(piece, (4, 1), pos) && Move.NotInCheck(piece, (3, 1), pos))
                                legalMoves.Add(2.IntToPosXY());
                    }
                }
                else
                {
                    if (pos.BShortCastle)
                    {
                        if (Move.NothingInTheWay(60, 63, pos))
                            if (Move.NotInCheck(piece, (6, 8), pos) && Move.NotInCheck(piece, (7, 8), pos))
                                legalMoves.Add(62.IntToPosXY());
                    }
                    if (pos.BLongCastle && Move.NotInCheck(piece, (4, 8), pos))
                    {
                        if (Move.NothingInTheWay(60, 56, pos))
                            if (Move.NotInCheck(piece, (4, 8), pos) && Move.NotInCheck(piece, (3, 8), pos))
                                legalMoves.Add(58.IntToPosXY());
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
    }
}