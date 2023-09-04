
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
                foreach (Piece piece in pos.Pieces)
                {
                    if (piece.pos != oldPos && piece.pos != newPos)
                    {
                        if (Math.Abs(oldPos.x - piece.pos.x) == Math.Abs(oldPos.y - piece.pos.y))
                        {
                            if (Math.Max(oldPos.x, newPos.x) > piece.pos.x && piece.pos.x > Math.Min(oldPos.x, newPos.x))
                                if (Math.Max(oldPos.y, newPos.y) > piece.pos.y && piece.pos.y > Math.Min(oldPos.y, newPos.y))
                                    return false;
                        }
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
                        if (!piece.IsPinned(move, pos))
                        {
                            legalMoves.Add(move);
                        }
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
                    if (!piece.IsPinned(move, pos))
                    {
                        legalMoves.Add(move);
                    }
                }
                else if (move != piece.pos)
                    break;
                if (!Unobstructed(move, pos.EnemyPieces()))
                {
                    break;
                }
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
                    if (!piece.IsPinned(move, pos))
                    {
                        legalMoves.Add(move);
                    }
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
            List<Position> newPositions = NewPos.Format(pos, piece, move);
            if (newPositions.Count != 0)
            {
                Position newPos = NewPos.Format(pos, piece, move)[0];

                (int x, int y) posKing = (0, 0);
                if (piece.piece != Piece.King)
                {
                    foreach (Piece p in newPos.EnemyPieces()) //The own king is now an enemy, because the turn switched
                    {
                        if (p.piece == Piece.King)
                        {
                            posKing = p.pos;
                            break;
                        }
                    }
                }
                else
                {
                    posKing = move;
                }

                foreach (Piece p in newPos.OwnPieces())
                {
                    if (p.pos.x == posKing.x && (p.piece == Piece.Rook || p.piece == Piece.Queen))
                    {
                        if (NothingInTheWay(posKing, p.pos, newPos))
                            return false;
                    }
                    else if (p.pos.y == posKing.y && (p.piece == Piece.Rook || p.piece == Piece.Queen))
                    {
                        if (NothingInTheWay(posKing, p.pos, newPos))
                            return false;
                    }
                    else if (Math.Abs(p.pos.x - posKing.x) == Math.Abs(p.pos.y - posKing.y) && (p.piece == Piece.Bishop || p.piece == Piece.Queen))
                    {
                        if (NothingInTheWay(posKing, p.pos, newPos))
                            return false;
                    }
                    else if (p.piece == Piece.Knight)
                    {
                        foreach ((int, int) moveN in Knight.Moves(p, newPos))
                            if (moveN == posKing)
                                return false;
                    }
                    else if (p.piece == Piece.Pawn)
                    {
                        foreach ((int, int) moveP in Pawn.Moves(p, newPos))
                            if (moveP == posKing)
                                return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}