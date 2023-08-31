namespace ChessEngine
{

    class Queen
    {

        public static List<(int, int)> LegalMoves((int, int) posQueen, Position pos)
        {
            List<(int, int)> legalMoves = new();
            legalMoves.AddRange(Move.StraightMoves(posQueen, pos));
            legalMoves.AddRange(Move.StraightMoves(posQueen, pos, positiv: false));
            legalMoves.AddRange(Move.DiagonalMoves(posQueen, pos));
            legalMoves.AddRange(Move.DiagonalMoves(posQueen, pos, yPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(posQueen, pos, xPos: false));
            legalMoves.AddRange(Move.DiagonalMoves(posQueen, pos, xPos: false, yPos: false));

            string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Queen at {posQueen} to {combinedString}");
            return legalMoves;
        }
    }
}