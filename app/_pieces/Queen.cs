namespace ChessEngine
{

    class Queen
    {

        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            return Move.SlidingMoves(piece, pos);

            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Queen at {piece.pos} to {combinedString}");
        }
    }
}