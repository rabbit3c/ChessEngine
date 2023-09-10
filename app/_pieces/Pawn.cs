
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
            return Moves(piece, pos, true);
        }

        public static List<int> Moves(Piece piece, Position pos, bool legal = false)
        {
            bool[] allowedDirections = piece.IsPinned(pos, out bool _);
            List<int> moves = new();

            if (legal)
            {
                if (allowedDirections[0])
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
                        if (!Move.Unobstructed(moves[i], piece.isWhite, pos) || !Move.Unobstructed(moves[i], !piece.isWhite, pos) || !Move.NothingInTheWay(piece.pos, moves[i], pos))
                        {
                            moves.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            int[] directions = { 9, 7, -7, -9 };
            for (int i = piece.isWhite ? 0 : 2; i < (piece.isWhite ? 2 : directions.Length); i++)
            {
                if (allowedDirections[(i > 0 && i < 3) ? 3 : 2] || !legal)
                {
                    if (PrecomputedData.numSquareToEdge[piece.pos][i % 2 == 0 ? 3 : 2] != 0)
                    {
                        var move = piece.pos + directions[i];
                        if (Takeable(move, pos, piece.isWhite))
                            moves.Add(move);
                    }
                }
            }

            return moves;
        }

        static bool Takeable(int square, Position pos, bool isWhite)
        {
            List<int> enemyPieces = isWhite ? pos.PiecesBlack : pos.PiecesWhite;
            if (enemyPieces.Contains(square))
                return true;
            if (pos.EnPassantTarget == square)
                return true;
            return false;
        }
    }
}