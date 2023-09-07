
using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    class Pawn
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<(int, int)> legalMoves = Moves(piece, pos);

            for (int i = 0; i < legalMoves.Count; i++)
            {
                if (piece.IsPinned(legalMoves[i], pos))
                {
                    legalMoves.RemoveAt(i);
                    i--;
                }
            }

            //string combinedString = string.Join(", ", moves);
            //Console.WriteLine($"Pawn at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.pos}, {legalMoves.Count}");
            return legalMoves;
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
                if (!Move.Unobstructed(moves[i], piece.isWhite, pos) || !Move.Unobstructed(moves[i], !piece.isWhite, pos) || !Move.NothingInTheWay(piece.pos.PosXYToInt(), moves[i].PosXYToInt(), pos))
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
            List<int> enemyPieces = pos.EnemyPieces();
            if (pos.EnPassantTarget == square)
            {
                return true;
            }
            if (square.x >= 1 && square.x <= 8) {
                if (enemyPieces.Contains(square.PosXYToInt()))
                    return true;
            }
            return false;
        }
    }
}