namespace ChessEngine
{
    class Rook
    {
        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            return Move.SlidingMoves(piece, pos);

            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Rook at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.piece}, {legalMoves.Count}");
        }
    }
}