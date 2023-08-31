namespace ChessEngine
{
    class Rook
    {
        public static List<(int, int)> LegalMoves((int x, int y) posRook, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.StraightMoves(posRook, pos));
            legalMoves.AddRange(Move.StraightMoves(posRook, pos, positiv: false));

            string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Rook at {posRook} to {combinedString}");
            return legalMoves;
        }
    }
}