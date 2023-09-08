
using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    class Pawn
    {
        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            List<int> legalMoves = Moves(piece, pos);

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

        public static List<int> Moves(Piece piece, Position pos)
        {
            List<int> moves = new();
            if (pos.WhitesTurn)
            {
                moves.Add(piece.pos + 8);
                if (piece.pos.Y() == 2)
                    moves.Add(piece.pos + 16);
            }

            else
            {
                moves.Add(piece.pos - 8);
                if (piece.pos.Y() == 7)
                    moves.Add(piece.pos - 16);
            }

            for (int i = 0; i < moves.Count; i++)
            {
                if (!Move.Unobstructed(moves[i], piece.isWhite, pos) || !Move.Unobstructed(moves[i], !piece.isWhite, pos) || !Move.NothingInTheWay(piece.pos, moves[i], pos))
                {
                    moves.RemoveAt(i);
                    i--;
                }
            }

            int[] directions = {9, 7, -7, -9};
            for (int i = pos.WhitesTurn ? 0 : 2; i < (pos.WhitesTurn ? 2 : directions.Length); i++) {
                if (PrecomputedData.numSquareToEdge[piece.pos][i % 2 == 0 ? 3 : 2] != 0) {
                    var move = piece.pos + directions[i];
                    if (Takeable(move, pos))
                        moves.Add(move);
                }
            }

            return moves;
        }

        static bool Takeable(int square, Position pos)
        {
            List<int> enemyPieces = pos.EnemyPieces();
            if (pos.EnPassantTarget == square)
                return true;
            if (enemyPieces.Contains(square))
                return true;
            return false;
        }
    }
}