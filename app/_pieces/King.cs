
namespace ChessEngine
{
    class King
    {
        public static List<(int, int)> LegalMoves((int x, int y) posKing, Position pos)
        {
            List<(int, int)> legalMoves = new();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    (int, int) move = (posKing.x + x, posKing.y + y);
                    if (Legal(move, pos) && posKing != move)
                        legalMoves.Add((posKing.x + x, posKing.y + y));
                }
            }
            if (pos.WhitesTurn) {
                if (pos.WShortCastle) {
                    if (Move.NothingInTheWay((5, 1), (8, 1), pos)) 
                        legalMoves.Add((7, 1));
                }
                if (pos.WLongCastle) {
                    if (Move.NothingInTheWay((5, 1), (1, 1), pos))
                        legalMoves.Add((3, 1));
                }
            }
            else {
                if (pos.BShortCastle) {
                    if (Move.NothingInTheWay((5, 8), (8, 8), pos)) 
                        legalMoves.Add((7, 8));
                }
                if (pos.BLongCastle) {
                    if (Move.NothingInTheWay((5, 8), (1, 8), pos))
                        legalMoves.Add((3, 8));
                }
            }
            string combinedString = string.Join( ", ", legalMoves);
            Console.WriteLine($"King at {posKing} to {combinedString}");
            return legalMoves;
        }

        static bool Legal((int x, int y) move, Position pos)
        {
            if (Move.Inbound(move) && Move.NotInCheck(move, pos) && Move.Unobstructed(move, pos.OwnPieces()))
                return true;
            return false;
        }
    }
}