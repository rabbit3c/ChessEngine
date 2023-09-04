namespace ChessEngine
{

    class Knight
    {

        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = new();

            if (!piece.IsPinned(new(), pos))
            {
                for (int x = -2; x <= 2; x += 4)
                {
                    for (int y = -1; y <= 1; y += 2)
                    {
                        (int, int) move = (piece.pos.x + x, piece.pos.y + y);
                        if (Legal(piece, move, pos))
                            legalMoves.Add(move);
                    }
                }
                for (int y = -2; y <= 2; y += 4)
                {
                    for (int x = -1; x <= 1; x += 2)
                    {
                        (int, int) move = (piece.pos.x + x, piece.pos.y + y);
                        if (Legal(piece, move, pos))
                            legalMoves.Add(move);
                    }
                }
            }

            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Knight at {piece.pos} to {combinedString}");
            return legalMoves;
        }

        public static List<(int, int)> Moves(Piece piece, Position pos)
        {
            List<(int, int)> moves = new();

            for (int x = -2; x <= 2; x += 4)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    (int, int) move = (piece.pos.x + x, piece.pos.y + y);
                    moves.Add(move);
                }
            }
            for (int y = -2; y <= 2; y += 4)
            {
                for (int x = -1; x <= 1; x += 2)
                {
                    (int, int) move = (piece.pos.x + x, piece.pos.y + y);
                    moves.Add(move);
                }
            }

            return moves;
        }

        public static bool Legal(Piece piece, (int, int) move, Position pos)
        {
            if (Move.Unobstructed(move, pos.OwnPieces()) && Move.Inbound(move))
                return true;
            return false;
        }
    }
}