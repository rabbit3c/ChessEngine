
namespace ChessEngine
{
    class Pawn
    {
        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            //List<int> legalMoves = Moves(piece, pos, true);
            //string combinedString = string.Join(", ", legalMoves);
            //Console.WriteLine($"Pawn at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.pos}, {legalMoves.Count}");
            return Moves(piece, pos);
        }

        public static List<int> Moves(Piece piece, Position pos)
        {
            Pin pin = piece.pin;
            List<int> moves = new();

            if (pin.allowedDirections[0])
            {
                if (piece.isWhite)
                {
                    moves.Add(piece.pos + 8);
                    if (piece.pos.Y() == 1)
                        moves.Add(piece.pos + 16);
                }

                else
                {
                    moves.Add(piece.pos - 8);
                    if (piece.pos.Y() == 6)
                        moves.Add(piece.pos - 16);
                }
                for (int i = 0; i < moves.Count; i++)
                {
                    if (Illegal(piece.pos, moves[i], pos))
                    {
                        moves.RemoveAt(i);
                        i--;
                    }
                }
            }

            moves.AddRange(DiagonalMoves(piece, pos, pin.allowedDirections, true));

            return moves;
        }

        public static List<int> DiagonalMoves(Piece piece, Position pos, bool[] allowedDirections, bool legal = false)
        {
            List<int> diagonalMoves = new();
            int[] directions = { 9, 7, -7, -9 };
            for (int i = piece.isWhite ? 0 : 2; i < (piece.isWhite ? 2 : directions.Length); i++)
            {
                if (!legal)
                {
                    if (PrecomputedData.numSquareToEdge[piece.pos][i % 2 == 0 ? 3 : 2] != 0)
                    {
                        diagonalMoves.Add(piece.pos + directions[i]);
                    }
                }
                else if (allowedDirections[(i > 0 && i < 3) ? 3 : 2])
                {
                    if (PrecomputedData.numSquareToEdge[piece.pos][i % 2 == 0 ? 3 : 2] != 0)
                    {
                        var move = piece.pos + directions[i];
                        if (Takeable(move, pos, piece.isWhite))
                            diagonalMoves.Add(move);
                    }
                }
            }
            return diagonalMoves;
        }

        public static bool Takeable(int square, Position pos, bool isWhite)
        {
            List<int> enemyPieces = isWhite ? pos.PiecesBlack : pos.PiecesWhite;

            if (enemyPieces.Contains(square)) return true;

            if (pos.EnPassantTarget == square) return true;

            return false;
        }

        public static bool Illegal(int oldPos, int newPos, Position position)
        {
            if (!Move.Unobstructed(newPos, false, position)) return true;

            if (!Move.Unobstructed(newPos, true, position)) return true;

            if (!Move.NothingInTheWay(oldPos, newPos, position)) return true;

            return false;
        }
    }
}