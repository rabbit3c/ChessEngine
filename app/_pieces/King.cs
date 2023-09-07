
namespace ChessEngine
{
    class King
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    (int, int) move = (piece.pos.x + x, piece.pos.y + y);
                    if (Legal(move, piece.isWhite, pos) && piece.pos != move)
                        legalMoves.Add((piece.pos.x + x, piece.pos.y + y));
                }
            }
            if (Move.NotInCheck(piece, piece.pos, pos))
            {
                if (pos.WhitesTurn)
                {
                    if (pos.WShortCastle)
                    {
                        if (Move.NothingInTheWay((5, 1), (8, 1), pos))
                            if (Move.NotInCheck(piece, (6, 1), pos) && Move.NotInCheck(piece, (7, 1), pos))
                                legalMoves.Add((7, 1));
                    }
                    if (pos.WLongCastle)
                    {
                        if (Move.NothingInTheWay((5, 1), (1, 1), pos))
                            if (Move.NotInCheck(piece, (4, 1), pos) && Move.NotInCheck(piece, (3, 1), pos))
                                legalMoves.Add((3, 1));
                    }
                }
                else
                {
                    if (pos.BShortCastle)
                    {
                        if (Move.NothingInTheWay((5, 8), (8, 8), pos))
                            if (Move.NotInCheck(piece, (6, 8), pos) && Move.NotInCheck(piece, (7, 8), pos))
                                legalMoves.Add((7, 8));
                    }
                    if (pos.BLongCastle && Move.NotInCheck(piece, (4, 8), pos))
                    {
                        if (Move.NothingInTheWay((5, 8), (1, 8), pos))
                            if (Move.NotInCheck(piece, (4, 8), pos) && Move.NotInCheck(piece, (3, 8), pos))
                                legalMoves.Add((3, 8));
                    }
                }
            }
            //string combinedString = string.Join( ", ", legalMoves);
            //Console.WriteLine($"King at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.piece}, {legalMoves.Count}");
            return legalMoves;
        }

        static bool Legal((int x, int y) move, bool isWhite, Position pos)
        {
            if (Move.Inbound(move))
                if (Move.Unobstructed(move, isWhite, pos))
                    return true;
            return false;
        }
    }
}