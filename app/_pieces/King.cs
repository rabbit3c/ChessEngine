
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
            string combinedString = string.Join( ", ", legalMoves);
            Console.WriteLine($"King at {posKing} to {combinedString}");
            return legalMoves;
        }

        static bool Legal((int x, int y) move, Position pos)
        {
            if (Move.Inbound(move) && Move.Unprotected(move) && Move.Unobstructed(move, pos.OwnPieces()))
                return true;
            return false;
        }
    }
}