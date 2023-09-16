
using System.ComponentModel;

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
                    if (Move.Unobstructed(move, piece.isWhite, pos))
                        legalMoves.Add(move);
                }
            }

            if (NotIllegal(piece.pos, pos))
            {
                if (pos.WhitesTurn)
                {
                    if (pos.WShortCastle)
                    {
                        if (Move.NothingInTheWay(4, 7, pos))
                            if (NotIllegal(5, pos) && NotIllegal(6, pos))
                                legalMoves.Add(6); //g1
                    }
                    if (pos.WLongCastle)
                    {
                        if (Move.NothingInTheWay(4, 0, pos))
                            if (NotIllegal(3, pos) && NotIllegal(2, pos))
                                legalMoves.Add(2); //c1
                    }
                }
                else
                {
                    if (pos.BShortCastle)
                    {
                        if (Move.NothingInTheWay(60, 63, pos))
                            if (NotIllegal(61, pos) && NotIllegal(62, pos))
                                legalMoves.Add(62); //g8
                    }
                    if (pos.BLongCastle)
                    {
                        if (Move.NothingInTheWay(60, 56, pos))
                            if (NotIllegal(59, pos) && NotIllegal(58, pos))
                                legalMoves.Add(58); //c8
                    }
                }
            }

            //string combinedString = string.Join( ", ", legalMoves);
            //Console.WriteLine($"King at {piece.pos} to {combinedString}");
            //Console.WriteLine($"{piece.piece}, {legalMoves.Count}");
            return legalMoves;
        }

        public static bool NotIllegal(int move, Position pos)
        {
            return !pos.Check(move);
        }
    }
}