
namespace ChessEngine
{
    class Pawn
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> moves = Moves(piece, pos);

            for (int i = 0; i < moves.Count; i++)
            {
                if (piece.IsPinned(moves[i], pos))
                {
                    moves.RemoveAt(i);
                    i--;
                }
            }

            //string combinedString = string.Join(", ", moves);
            //Console.WriteLine($"Pawn at {piece.pos} to {combinedString}");
            return moves;
        }

        public static List<(int, int)> Moves(Piece piece, Position pos)
        {
            List<(int, int)> moves = new();
            if (pos.WhitesTurn)
            {
                moves.Add((piece.pos.x, piece.pos.y + 1));
                if (piece.pos.y == 2)
                    moves.Add((piece.pos.x, piece.pos.y + 2));
            }
            else
            {
                moves.Add((piece.pos.x, piece.pos.y - 1));
                if (piece.pos.y == 7)
                    moves.Add((piece.pos.x, piece.pos.y - 2));
            }
            for (int i = 0; i < moves.Count; i++)
            {
                if (!Move.Unobstructed(moves[i], pos.Pieces) || !Move.NothingInTheWay(piece.pos, moves[i], pos))
                {
                    moves.RemoveAt(i);
                    i--;
                }
            }

            if (pos.WhitesTurn)
            {
                var move = (piece.pos.x + 1, piece.pos.y + 1);
                if (Takeable(move, pos))
                    moves.Add(move);
                move = (piece.pos.x - 1, piece.pos.y + 1);
                if (Takeable(move, pos))
                    moves.Add(move);
            }
            else
            {
                var move = (piece.pos.x + 1, piece.pos.y - 1);
                if (Takeable(move, pos))
                    moves.Add(move);
                move = (piece.pos.x - 1, piece.pos.y - 1);
                if (Takeable(move, pos))
                    moves.Add(move);
            }
            return moves;
        }

        static bool Takeable((int x, int y) square, Position pos)
        {
            List<Piece> enemyPieces = pos.EnemyPieces();
            if (pos.EnPassantTarget == square)
            {
                return true;
            }
            for (int i = 0; i < enemyPieces.Count; i++)
            {
                if (enemyPieces[i].pos == (square.x, square.y))
                    return true;
            }
            return false;
        }
    }
}