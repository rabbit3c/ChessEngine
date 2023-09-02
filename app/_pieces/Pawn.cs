
namespace ChessEngine
{
    class Pawn
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
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
            for (int i = 0; i < moves.Count; i++) {
                if (!Legal(piece, moves[i], pos)) {
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
            /*string combinedString = string.Join(", ", moves);
            Console.WriteLine($"Pawn at {piece.pos} to {combinedString}");*/
            return moves;
        }

        static bool Legal(Piece piece, (int x, int y) move, Position pos)
        {   
            if (Move.NothingInTheWay(piece.pos, move, pos) && Move.Unobstructed(move, pos.Pieces) && Move.NotInCheck(piece, move, pos))
                return true;
            return false;
        }
        static bool Takeable((int x, int y) square, Position pos)
        {
            List<Piece> enemyPieces = pos.EnemyPieces();
            if (pos.EnPassantTarget == square) {
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