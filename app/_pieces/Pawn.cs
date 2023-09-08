
using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    class Pawn
    {
        public static List<(int, int)> LegalMoves(Piece piece, Position pos)
        {
            List<int> moves = Moves(piece, pos);
            List<(int, int)> legalMoves = new();

            /*for (int i = 0; i < legalMoves.Count; i++)
            {
                if (piece.IsPinned(legalMoves[i], pos))
                {
                    legalMoves.RemoveAt(i);
                    i--;
                }
            }*/
            for (int i = 0; i < moves.Count; i++)
            {
                if (!piece.IsPinned(moves[i].IntToPosXY(), pos))
                {
                    legalMoves.Add(moves[i].IntToPosXY());
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
            int position = piece.pos.PosXYToInt();
            if (pos.WhitesTurn)
            {
                moves.Add(position + 8);
                if (piece.pos.y == 2)
                    moves.Add(position + 16);
            }

            else
            {
                moves.Add(position - 8);
                if (piece.pos.y == 7)
                    moves.Add(position - 16);
            }

            for (int i = 0; i < moves.Count; i++)
            {
                if (!Move.Unobstructed(moves[i], piece.isWhite, pos) || !Move.Unobstructed(moves[i], !piece.isWhite, pos) || !Move.NothingInTheWay(piece.pos.PosXYToInt(), moves[i], pos))
                {
                    moves.RemoveAt(i);
                    i--;
                }
            }

            int[] directions = {9, 7, -7, -9};
            for (int i = pos.WhitesTurn ? 0 : 2; i < (pos.WhitesTurn ? 2 : directions.Length); i++) {
                if (PrecomputedData.numSquareToEdge[position][i % 2 == 0 ? 3 : 2] != 0) {
                    var move = position + directions[i];
                    if (Takeable(move, pos))
                        moves.Add(move);
                }
            }

            return moves;
        }

        static bool Takeable(int square, Position pos)
        {
            List<int> enemyPieces = pos.EnemyPieces();
            if (pos.EnPassantTarget.PosXYToInt() == square)
                return true;
            if (enemyPieces.Contains(square))
                return true;
            return false;
        }
    }
}