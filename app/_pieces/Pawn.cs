
namespace ChessEngine
{
    class Pawn
    {
        public static List<(int, int)> LegalMoves((int x, int y) posPawn, Position pos)
        {
            List<(int, int)> moves = new();
            if (pos.WhitesTurn)
            {
                moves.Add((posPawn.x, posPawn.y + 1));
                if (posPawn.y == 2)
                    moves.Add((posPawn.x, posPawn.y + 2));
            }
            else
            {
                moves.Add((posPawn.x, posPawn.y - 1));
                if (posPawn.y == 7)
                    moves.Add((posPawn.x, posPawn.y - 2));
            }
            for (int i = 0; i < moves.Count; i++) {
                if (!Legal(posPawn, moves[i], pos)) {
                    moves.RemoveAt(i);
                    i--;
                }
            }

            if (pos.WhitesTurn)
            {   
                var move = (posPawn.x + 1, posPawn.y + 1);
                if (Takeable(move, pos))
                    moves.Add(move);
                move = (posPawn.x - 1, posPawn.y + 1);
                if (Takeable(move, pos))
                    moves.Add(move);
            }
            else
            {
                var move = (posPawn.x + 1, posPawn.y - 1);
                if (Takeable(move, pos))
                    moves.Add(move);
                move = (posPawn.x - 1, posPawn.y - 1);
                if (Takeable(move, pos))
                    moves.Add(move);
            }
            string combinedString = string.Join(", ", moves);
            Console.WriteLine($"Pawn at {posPawn} to {combinedString}");
            return moves;
        }

        static bool Legal((int x, int y)posPawn, (int x, int y) move, Position pos)
        {   
            if (Move.NothingInTheWay(posPawn, move, pos) && Move.Unobstructed(move, pos.Pieces))
                return true;
            return false;
        }
        static bool Takeable((int x, int y) square, Position pos)
        {
            List<string> enemyPieces = pos.EnemyPieces();
            for (int i = 0; i < enemyPieces.Count; i++)
            {
                if (enemyPieces[i].Contains($"{square.x}{square.y}"))
                    return true;
            }
            return false;
        }
    }
}