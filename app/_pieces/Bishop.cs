namespace ChessEngine
{
    class Bishop
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            return Move.SlidingMoves(piece, pos);
            
            /*string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Bishop at {piece.pos} to {combinedString}");*/
        }
    }
}