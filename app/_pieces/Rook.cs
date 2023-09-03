namespace ChessEngine
{
    class Rook
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.StraightMoves(piece, pos));
            legalMoves.AddRange(Move.StraightMoves(piece, pos, positiv: false));

            /*string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Rook at {piece.pos} to {combinedString}");*/
            return legalMoves;
        }
    }
}