
using System.ComponentModel.DataAnnotations;

namespace ChessEngine
{
    class Move
    {
        public static bool Inbound((int x, int y) pos)
        {
            if (pos.x >= 1 && pos.x <= 8 && pos.y >= 1 && pos.y <= 8)
                return true;
            return false;
        }

        public static bool Unprotected((int x, int y) pos)
        {
            // needs to be implemented
            return true;
        }

        public static bool Unobstructed((int x, int y) pos, List<string> ownPieces)
        {
            for (int i = 0; i < ownPieces.Count; i++)
            {
                if (ownPieces[i].Contains($"{pos.x}{pos.y}"))
                    return false;
            }
            return true;
        }

        public static bool NothingInTheWay((int x, int y) oldPos, (int x, int y) newPos, Position pos)
        {
            if (oldPos.y == newPos.y)
            {
                for (int x = Math.Min(oldPos.x, newPos.x); x <= Math.Max(oldPos.x, newPos.x); x++)
                {
                    (int, int) square = (x, newPos.y);
                    for (int i = 0; i < pos.Pieces.Count; i++)
                    {
                        if (pos.Pieces[i].Contains($"{x}{newPos.y}") && square != oldPos && square != newPos)
                            return false;
                    }
                }
            }
            else if (oldPos.x == newPos.x)
            {
                for (int y = Math.Min(oldPos.y, newPos.y); y <= Math.Max(oldPos.y, newPos.y); y++)
                {
                    (int, int) square = (newPos.x, y);
                    for (int i = 0; i < pos.Pieces.Count; i++)
                    {
                        if (pos.Pieces[i].Contains($"{newPos.x}{y}") && square != oldPos && square != newPos)
                            return false;
                    }
                }
            }
            else
            {
                for (int n = 0; n < Math.Abs(newPos.x - oldPos.x); n++)
                {
                    int x = newPos.x > oldPos.x ? oldPos.x + n : oldPos.x - n;
                    int y = newPos.y > oldPos.y ? oldPos.y + n : oldPos.y - n;
                    (int, int) square = (x, newPos.y);
                    Console.WriteLine(square);
                    for (int i = 0; i < pos.Pieces.Count; i++)
                    {
                        if (pos.Pieces[i].Contains($"{x}{y}") && square != oldPos && square != newPos)
                            return false;
                    }
                }
            }
            return true;
        }

        public static List<(int, int)> DiagonalMoves((int x, int y) posPiece, Position pos, bool xPos = true, bool yPos = true)
        {
            List<(int, int)> legalMoves = new();
            for (int n = 0; xPos ? n <= 8 - posPiece.x : n <= posPiece.x - 1; n++)
            {
                int x = xPos ? posPiece.x + n : posPiece.x - n;
                int y = yPos ? posPiece.y + n : posPiece.y - n;
                if (yPos ? y <= 8 : y >= 1)
                {
                    (int, int) move = (x, y);
                    if (Unobstructed(move, pos.OwnPieces()))
                    {
                        if (Legal(move, posPiece, pos))
                            legalMoves.Add(move);
                    }
                    else if (move != posPiece)
                        break;
                    if (!Unobstructed(move, pos.EnemyPieces()))
                        break;
                }
                else
                {
                    break;
                }
            }
            return legalMoves;
        }

        public static List<(int, int)> StraightMoves((int x, int y) posPiece, Position pos, bool positiv = true)
        {
            List<(int, int)> legalMoves = new();
            for (int x = posPiece.x; positiv ? x <= 8 : x >= 1;)
            {
                (int, int) move = (x, posPiece.y);
                if (Unobstructed(move, pos.OwnPieces()))
                {
                    if (Legal(move, posPiece, pos))
                        legalMoves.Add(move);
                }
                else if (move != posPiece)
                    break;
                if (!Unobstructed(move, pos.EnemyPieces()))
                    break;
                if (positiv) 
                    x++;
                else
                    x--;
            }
            for (int y = posPiece.y; positiv ? y <= 8 : y >= 1;)
            {
                (int, int) move = (posPiece.x, y);
                if (Unobstructed(move, pos.OwnPieces()))
                {
                    if (Legal(move, posPiece, pos))
                        legalMoves.Add(move);
                }
                else if (move != posPiece)
                    break;
                if (!Unobstructed(move, pos.EnemyPieces()))
                    break;
                if (positiv) 
                    y++;
                else
                    y--;
            }

            return legalMoves;
        }

        static bool Legal((int x, int y) move, (int x, int y) posPiece, Position pos)
        {
            if (move != posPiece)
                return true;
            return false;
        }
    }
}