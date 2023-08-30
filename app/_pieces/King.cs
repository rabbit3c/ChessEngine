using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    class King
    {
        public static List<(int, int)> LegalMoves((int x, int y) posKing, List<string> piecesWhite, List<string> piecesBlack, bool white)
        {
            List<(int, int)> legalMoves = new();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    (int, int) move = (posKing.x + x, posKing.y + y);
                    if (Legal(move))
                        legalMoves.Add((posKing.x + x, posKing.y + y));
                }
            }
            return legalMoves;
        }

        static bool Legal((int x, int y) move)
        {
            if (Move.Inbound(move) && Move.Unprotected(move))
                return true;
            return false;
        }
    }
}