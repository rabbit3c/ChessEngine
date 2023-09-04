namespace ChessEngine
{
    class NewPos
    {
        public static List<Position> Format(Position oldPos, Piece MovedPiece, (int x, int y) move)
        {
            List<Position> newPositions = new()
            {
                (Position)oldPos.Copy()
            };
            Piece newPiece = new();

            for (int i = 0; i < newPositions[0].Pieces.Count;)
            {
                if (newPositions[0].Pieces[i].pos == MovedPiece.pos)
                {
                    newPiece = (Piece)newPositions[0].Pieces[i].Copy();
                    newPositions[0].RemoveAt(i);
                }
                else if (newPositions[0].Pieces[i].pos == move)
                {
                    if (newPositions[0].Pieces[i].piece == Piece.Rook)
                        newPositions[0].NoCastle(newPositions[0].Pieces[i]);
                    newPositions[0].RemoveAt(i);
                }
                else if (MovedPiece.piece == Piece.Pawn && newPositions[0].Pieces[i].piece == Piece.Pawn && move == oldPos.EnPassantTarget)
                {
                    if (MovedPiece.isWhite ? newPositions[0].Pieces[i].pos == (move.x, move.y - 1) : newPositions[0].Pieces[i].pos == (move.x, move.y + 1))
                        newPositions[0].RemoveAt(i);
                    else
                        i++;
                }
                else
                    i++;

            }

            newPiece.pos = move;

            if (newPiece.Promoting())
            {
                for (int i = 1; i <= 3; i++)
                {
                    newPiece.piece = i;
                    newPositions.Add((Position)newPositions[0].Copy());
                    newPositions[i].Add((Piece)newPiece.Copy());
                }
                newPiece.piece = Piece.Queen;
            }

            newPositions[0].Add(newPiece);

            newPositions[0].EnPassantTarget = (0, 0);

            if (MovedPiece.piece == Piece.Pawn && Math.Abs(MovedPiece.pos.y - move.y) == 2)
                newPositions[0].EnPassantTarget = (MovedPiece.pos.x, MovedPiece.isWhite ? MovedPiece.pos.y + 1 : MovedPiece.pos.y - 1);
            if (MovedPiece.piece == Piece.King)
            {

                if (Math.Abs(MovedPiece.pos.x - move.x) == 2)
                    foreach (Position newPosition in newPositions)
                    {
                        foreach (Piece piece in newPosition.Pieces)
                        {
                            if (move.x == 3)
                            {
                                if (piece.pos == (1, move.y))
                                    piece.pos = (4, move.y);
                            }
                            else if (move.x == 7)
                                if (piece.pos == (8, move.y))
                                    piece.pos = (6, move.y);
                        }
                    }
                newPositions[0].NoCastle(MovedPiece);
            }

            if (MovedPiece.piece == Piece.Rook)
            {
                newPositions[0].NoCastle(MovedPiece);
            }

            foreach (Position newPos in newPositions)
            {
                newPos.SplitColors();
                newPos.ToggleTurn();
            }

            if (!NotInCheck(newPositions[0])) {
                newPositions.Clear();
                return newPositions;
            }

            return newPositions;
        }

        public static List<Position> New(Position oldPos, Piece Piece, List<(int x, int y)> moves, out int newPos)
        {
            List<Position> newPositions = new();
            foreach ((int, int) move in moves)
            {
                newPositions.AddRange(Format(oldPos, Piece, move));
            }
            newPos = newPositions.Count;
            return newPositions;
        }

        public static bool NotInCheck(Position pos)
        {

            (int x, int y) posKing = (0, 0);
            foreach (Piece p in pos.EnemyPieces())
            {
                if (p.piece == Piece.King)
                {
                    posKing = p.pos;
                    break;
                }
            }

            foreach (Piece p in pos.OwnPieces())
            {
                if (p.pos.x == posKing.x && (p.piece == Piece.Rook || p.piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, p.pos, pos))
                        return false;
                }
                else if (p.pos.y == posKing.y && (p.piece == Piece.Rook || p.piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, p.pos, pos))
                        return false;
                }
                else if (Math.Abs(p.pos.x - posKing.x) == Math.Abs(p.pos.y - posKing.y) && (p.piece == Piece.Bishop || p.piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, p.pos, pos))
                        return false;
                }
                else if (p.piece == Piece.Knight)
                {
                    foreach ((int, int) moveN in Knight.Moves(p, pos))
                        if (moveN == posKing)
                            return false;
                }
                else if (p.piece == Piece.Pawn)
                {
                    foreach ((int, int) moveP in Pawn.Moves(p, pos))
                        if (moveP == posKing)
                            return false;
                }
            }
            return true;
        }
    }
}