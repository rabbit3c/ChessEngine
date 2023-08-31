namespace ChessEngine
{
    class Bishop
    {
        public static List<(int, int)> LegalMoves((int x, int y) posBishop, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.DiagonalMoves(posBishop, pos));
            legalMoves.AddRange(Move.DiagonalMoves(posBishop, pos, yPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(posBishop, pos, xPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(posBishop, pos, xPos: false, yPos: false));

            string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Bishop at {posBishop} to {combinedString}");
            return legalMoves;
        }
    }
}