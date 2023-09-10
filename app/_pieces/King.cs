
namespace ChessEngine
{
    class King
    {
        public static List<int> LegalMoves(Piece piece, Position pos)
        {
            List<int> legalMoves = new();
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };

            for (int i = 0; i < directions.Length; i++)
            {
                if (PrecomputedData.numSquareToEdge[piece.pos][i] != 0)
                {
                    int move = piece.pos + directions[i];
                    if (Move.Unobstructed(move, piece.isWhite, pos) && NotIllegal(piece, move, pos))
                        legalMoves.Add(move);
                }
            }

            if (NotIllegal(piece, piece.pos, pos))
            {
                if (pos.WhitesTurn)
                {
                    if (pos.WShortCastle)
                    {
                        if (Move.NothingInTheWay(4, 7, pos))
                            if (NotIllegal(piece, 5, pos) && NotIllegal(piece, 6, pos))
                                legalMoves.Add(6); //g1
                    }
                    if (pos.WLongCastle)
                    {
                        if (Move.NothingInTheWay(4, 0, pos))
                            if (NotIllegal(piece, 3, pos) && NotIllegal(piece, 2, pos))
                                legalMoves.Add(2); //c1
                    }
                }
                else
                {
                    if (pos.BShortCastle)
                    {
                        if (Move.NothingInTheWay(60, 63, pos))
                            if (NotIllegal(piece, 61, pos) && NotIllegal(piece, 62, pos))
                                legalMoves.Add(62); //g8
                    }
                    if (pos.BLongCastle)
                    {
                        if (Move.NothingInTheWay(60, 56, pos))
                            if (NotIllegal(piece, 59, pos) && NotIllegal(piece, 58, pos))
                                legalMoves.Add(58); //c8
                    }
                }
            }
            
            //string combinedString = string.Join( ", ", legalMoves);
            //Console.WriteLine($"King at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.piece}, {legalMoves.Count}");
            return legalMoves;
        }

        public static bool NotIllegal(Piece piece, int move, Position pos)
        {
            List<Position> newPositions = new()
            {
                (Position)pos.Copy()
            };
            NewPos.MovePiece(pos, newPositions, piece, move);
            NewPos.Castle(newPositions[0], piece, move);
            newPositions[0].RemoveEnPassantTarget();
            if (newPositions[0].Check()) 
            {
                return false;
            }
            return true;
        }
    }
}