namespace ChessEngine
{
    class NewPos
    {

        public static List<Position> New(Position oldPos, Piece Piece, List<int> moves, out int newPos)
        {
            List<Position> newPositions = new();
            foreach (int move in moves)
            {
                newPositions.AddRange(Format(oldPos, Piece, move));
            }
            newPos = newPositions.Count;
            return newPositions;
        }
        
        public static List<Position> Format(Position oldPos, Piece MovedPiece, int move)
        {
            List<Position> newPositions = new()
            {
                (Position)oldPos.Copy()
            };
            Piece newPiece = (Piece)MovedPiece.Copy();
            newPiece.pos = move;
            newPositions[0].RemoveAt(MovedPiece.pos);
            Square targetSquare = (Square)newPositions[0].Board[move].Copy();
            if (!targetSquare.empty && targetSquare.piece == Piece.Rook)
            {
                newPositions[0].NoCastle(targetSquare);
            }
            else if (MovedPiece.piece == Piece.Pawn && move == oldPos.EnPassantTarget)
            {
                newPositions[0].RemoveAt(oldPos.EnPassantTarget + (MovedPiece.isWhite ? -8 : 8));
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

            //Castle and remove Castling Rights
            if (MovedPiece.piece == Piece.King)
            {
                if (Math.Abs(MovedPiece.pos.X() - move.X()) == 2)
                {
                    Piece rook = new();

                    if (move.X() == 3)
                    {
                        rook = (Piece)newPositions[0].Board[(1, move.Y()).PosXYToInt()].Copy();
                        rook.pos = (4, move.Y()).PosXYToInt();
                        newPositions[0].RemoveAt((1, move.Y()).PosXYToInt());
                    }
                    else if (move.X() == 7)
                    {
                        rook = (Piece)newPositions[0].Board[(8, move.Y()).PosXYToInt()].Copy();
                        rook.pos = (6, move.Y()).PosXYToInt();
                        newPositions[0].RemoveAt((8, move.Y()).PosXYToInt());
                    }
                    newPositions[0].Add(rook);
                }
                newPositions[0].NoCastle(MovedPiece);
            }

            newPositions[0].RemoveEnPassantTarget();

            //Check if there is a new enPassant target
            if (MovedPiece.piece == Piece.Pawn && Math.Abs(MovedPiece.pos.Y() - move.Y()) == 2)
                newPositions[0].AddEnPassantTarget((MovedPiece.pos.X(), MovedPiece.isWhite ? MovedPiece.pos.Y() + 1 : MovedPiece.pos.Y() - 1).PosXYToInt());


            //Remove Castling Rights
            if (MovedPiece.piece == Piece.Rook)
            {
                newPositions[0].NoCastle(MovedPiece);
            }

            foreach (Position newPos in newPositions)
            {
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

        public static bool InCheck(Position pos)
        {
            int posKing = 0;
            foreach (int i in pos.EnemyPieces())
            {
                if (pos.Board[i].piece == Piece.King)
                {
                    posKing = pos.Board[i].pos;
                    break;
                }
            }

            foreach (int i in pos.OwnPieces())
            {
                if (i.X() == posKing.X() && (pos.Board[i].piece == Piece.Rook || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                        return true;
                }
                else if (i.Y() == posKing.Y() && (pos.Board[i].piece == Piece.Rook || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                    {
                        return true;
                    }
                }
                else if (Math.Abs(i.X() - posKing.X()) == Math.Abs(i.Y() - posKing.Y()) && (pos.Board[i].piece == Piece.Bishop || pos.Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, pos))
                        return true;
                }
                else if (pos.Board[i].piece == Piece.Knight)
                {
                    foreach (int moveN in Knight.Moves(pos.Board[i], pos))
                        if (moveN == posKing)
                            return true;
                }
                else if (pos.Board[i].piece == Piece.Pawn)
                {
                    foreach (int moveP in Pawn.Moves(pos.Board[i], pos))
                        if (moveP == posKing)
                            return true;
                }
            }
            return false;
        }
    }
}