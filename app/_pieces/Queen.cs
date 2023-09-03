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
            legalMoves.AddRange(Move.DiagonalMoves(piece, pos, yPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(piece, pos, xPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(piece, pos, xPos: false, yPos: false));

            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Queen at {piece.pos} to {combinedString}");
            return legalMoves;
        }
    }
}