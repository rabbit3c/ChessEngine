namespace ChessEngine
{
    class Bishop
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.DiagonalMoves(piece, pos));

            /*string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Bishop at {piece.pos} to {combinedString}");*/
            return legalMoves;
        }
    }
}