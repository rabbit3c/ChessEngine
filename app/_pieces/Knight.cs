namespace ChessEngine
{

    class Knight
    {

        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            Pin pin = piece.pin;

            if (pin.pinned) return new();

            return Moves(piece, pos);
        }

        public static List<int> Moves(Piece piece, Position pos)
        {
            List<int> moves = new();
            int[] directions = { 17, 15, 10, 6, -6, -10, -15, -17 };

            for (int i = 0; i < directions.Length; i++)
            {
                if (!Inbound(piece.pos, i)) continue;
                
                int move = piece.pos + directions[i];
                if (Move.Unobstructed(move, piece.isWhite, pos))
                    moves.Add(move);
            }

            return moves;
        }

        public static bool Inbound(int pos, int i)
        {
            if (PrecomputedData.numSquareToEdge[pos][i < 4 ? 0 : 1] >= ((i > 1 && i < 6) ? 1 : 2)) //some logic to check if the move will be inbound or not
                if (PrecomputedData.numSquareToEdge[pos][(i & 1) == 1 ? 2 : 3] >= ((i > 1 && i < 6) ? 2 : 1))
                    return true;
            return false;
        }
        /*
        bool[] inbound = {
            squareToEdge[0] >= 2 && squareToEdge[3] >= 1, 
            squareToEdge[0] >= 2 && squareToEdge[2] >= 1, 
            squareToEdge[0] >= 1 && squareToEdge[3] >= 2, 
            squareToEdge[0] >= 1 && squareToEdge[2] >= 2,
            squareToEdge[1] >= 1 && squareToEdge[3] >= 2, 
            squareToEdge[1] >= 1 && squareToEdge[2] >= 2, 
            squareToEdge[1] >= 2 && squareToEdge[3] >= 1, 
            squareToEdge[1] >= 2 && squareToEdge[2] >= 1,
        };
        */
    }
}