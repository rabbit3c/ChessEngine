
using System.Runtime.CompilerServices;

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

        public static bool Unobstructed((int x, int y) pos, List<Piece> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].pos == (pos.x, pos.y))
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
                        if (pos.Pieces[i].pos == (x, newPos.y) && square != oldPos && square != newPos)
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
                        if (pos.Pieces[i].pos == (newPos.x, y) && square != oldPos && square != newPos)
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
                    for (int i = 0; i < pos.Pieces.Count; i++)
                    {
                        if (pos.Pieces[i].pos == (x, y) && square != oldPos && square != newPos)
                            return false;
                    }
                }
            }
            return true;
        }

        public static List<(int, int)> DiagonalMoves(Piece piece, Position pos, bool xPos = true, bool yPos = true)
        {
            List<(int, int)> legalMoves = new();
            for (int n = 0; xPos ? n <= 8 - piece.pos.x : n <= piece.pos.x - 1; n++)
            {
                int x = xPos ? piece.pos.x + n : piece.pos.x - n;
                int y = yPos ? piece.pos.y + n : piece.pos.y - n;
                if (yPos ? y <= 8 : y >= 1)
                {
                    (int, int) move = (x, y);
                    if (Unobstructed(move, pos.OwnPieces()))
                    {
                        if (NotInCheck(piece, move, pos))
                            legalMoves.Add(move);
                    }
                    else if (move != piece.pos)
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

        public static List<(int, int)> StraightMoves(Piece piece, Position pos, bool positiv = true)
        {
            List<(int, int)> legalMoves = new();
            for (int x = piece.pos.x; positiv ? x <= 8 : x >= 1;)
            {
                (int, int) move = (x, piece.pos.y);
                if (Unobstructed(move, pos.OwnPieces()))
                {
                    if (NotInCheck(piece, move, pos))
                        legalMoves.Add(move);
                }
                else if (move != piece.pos)
                    break;
                if (!Unobstructed(move, pos.EnemyPieces()))
                    break;
                if (positiv)
                    x++;
                else
                    x--;
            }
            for (int y = piece.pos.y; positiv ? y <= 8 : y >= 1;)
            {
                (int, int) move = (piece.pos.x, y);
                if (Unobstructed(move, pos.OwnPieces()))
                {
                    if (NotInCheck(piece, move, pos))
                        legalMoves.Add(move);
                }
                else if (move != piece.pos)
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

        public static bool NotInCheck(Piece piece, (int x, int y) move, Position pos)
        {
            Position newPos = NewPos.Format(pos, piece, move);
            newPos.ToggleTurn();

            (int x, int y) posKing = (0, 0);
            foreach (Piece p in newPos.OwnPieces())
            {
                if (p.piece == Piece.King)
                {
                    posKing = p.pos;
                    break;
                }
            }

            foreach (Piece p in newPos.EnemyPieces())
            {
                if (p.pos.x == posKing.x)
                {
                    if (NothingInTheWay(posKing, p.pos, newPos))
                    {
                        Console.WriteLine("illegal");
                        Console.WriteLine(move);
                        return false;
                    }
                }
                else if (p.pos.y == posKing.y)
                {
                    if (NothingInTheWay(posKing, p.pos, newPos))
                    {
                        Console.WriteLine("illegal");
                        Console.WriteLine(move);
                        return false;
                    }
                }
                else
                {
                    if (p.Diagonal(posKing))
                    {
                        if (NothingInTheWay(posKing, p.pos, newPos))
                        {
                            Console.WriteLine("illegal");
                            Console.WriteLine(move);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}