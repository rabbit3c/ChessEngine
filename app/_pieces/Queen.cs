namespace ChessEngine
{

    class Queen
    {

        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.StraightMoves(piece, pos));
            legalMoves.AddRange(Move.StraightMoves(piece, pos, positiv: false));
            legalMoves.AddRange(Move.DiagonalMoves(piece, pos));

            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Queen at {piece.pos} to {combinedString}");
            return legalMoves;
        }
    }
}