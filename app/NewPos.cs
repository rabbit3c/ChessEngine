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
            Piece newPiece = (Piece)MovedPiece.Copy();
            newPiece.pos = move;
            newPositions[0].RemoveAt(MovedPiece.pos.PosXYToInt());
            Square targetSquare = (Square)newPositions[0].Board[move.PosXYToInt()].Copy();
            if (!targetSquare.empty && targetSquare.piece == Piece.Rook)
            {
                newPositions[0].NoCastle(targetSquare);
            }
            else if (MovedPiece.piece == Piece.Pawn && move == oldPos.EnPassantTarget)
            {
                newPositions[0].RemoveAt(oldPos.EnPassantTarget.PosXYToInt() + (MovedPiece.isWhite ? -8 : 8));
            }

            //check if any pawn is promoting
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

            //Check if there is a new enPassant target
            if (MovedPiece.piece == Piece.Pawn && Math.Abs(MovedPiece.pos.y - move.y) == 2)
                newPositions[0].EnPassantTarget = (MovedPiece.pos.x, MovedPiece.isWhite ? MovedPiece.pos.y + 1 : MovedPiece.pos.y - 1);

            //Castle and remove Castling Rights
            if (MovedPiece.piece == Piece.King)
            {
                if (Math.Abs(MovedPiece.pos.x - move.x) == 2)
                {
                    Piece rook = new();

                    if (move.x == 3)
                    {
                        rook = (Piece)newPositions[0].Board[(1, move.y).PosXYToInt()].Copy();
                        rook.pos = (4, move.y);
                        newPositions[0].RemoveAt((1, move.y).PosXYToInt());
                    }
                    else if (move.x == 7)
                    {
                        rook = (Piece)newPositions[0].Board[(8, move.y).PosXYToInt()].Copy();
                        rook.pos = (6, move.y);
                        newPositions[0].RemoveAt((8, move.y).PosXYToInt());
                    }
                    newPositions[0].Add(rook);
                }
                newPositions[0].NoCastle(MovedPiece);
            }

            //Remove Castling Rights
            if (MovedPiece.piece == Piece.Rook)
            {
                newPositions[0].NoCastle(MovedPiece);
            }

            foreach (Position newPos in newPositions)
            {
                newPos.SplitColors();
                newPos.ToggleTurn();
            }

            // Check if the Enemies could take the King
            if (InCheck(newPositions[0]))
            {
                newPositions.Clear();
                return newPositions;
            }

            //Console.WriteLine(MovedPiece.piece);
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

        public static bool InCheck(Position pos)
        {
            int posKing = 0;
            foreach (int i in pos.EnemyPieces())
            {
                if (pos.Board[i].piece == Piece.King)
                {
                    posKing = pos.Board[i].pos.PosXYToInt();
                    break;
                }
            }

            foreach (int i in pos.OwnPieces())
            {
                if (i.x() == posKing.x() && (pos.Board[i].piece == Piece.Rook || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                        return true;
                }
                else if (i.y() == posKing.y() && (pos.Board[i].piece == Piece.Rook || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                    {
                        return true;
                    }
                }
                else if (Math.Abs(i.x() - posKing.x()) == Math.Abs(i.y() - posKing.y()) && (pos.Board[i].piece == Piece.Bishop || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                        return true;
                }
                else if (pos.Board[i].piece == Piece.Knight)
                {
                    foreach ((int, int) moveN in Knight.Moves(pos.Board[i], pos))
                        if (moveN.PosXYToInt() == posKing)
                            return true;
                }
                else if (pos.Board[i].piece == Piece.Pawn)
                {
                    foreach ((int, int) moveP in Pawn.Moves(pos.Board[i], pos))
                        if (moveP.PosXYToInt() == posKing)
                            return true;
                }
            }
            return false;
        }
    }
}