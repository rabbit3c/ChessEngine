
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
                    if (Legal(piece, move, pos) && piece.pos != move)
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
            return legalMoves;
        }

        static bool Legal(Piece piece, (int x, int y) move, Position pos)
        {
            if (Move.Inbound(move) && Move.Unobstructed(move, pos.OwnPieces()))
                if (Move.NotInCheck(piece, move, pos)) {
                    return true;
                }
            return false;
        }
    }
}