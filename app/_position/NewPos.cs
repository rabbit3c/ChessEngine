
namespace ChessEngine
{
    class NewPos
    {

        public static List<Position> New(Position oldPos, Piece Piece, List<int> moves, bool lastDepth)
        {
            List<Position> newPositions = new();
            foreach (int move in moves)
            {
                newPositions.AddRange(Format(oldPos, Piece, move, lastDepth));
            }
            return newPositions;
        }

        public static List<Position> Format(Position oldPos, Piece MovedPiece, int move, bool lastDepth)
        {
            List<Position> newPositions = new()
            {
                (Position)oldPos.Copy()
            };

            newPositions[0].UpdateBitBoard(MovedPiece.pos, move);

            bool enPassant = MovedPiece.piece == Piece.Pawn && move == oldPos.EnPassantTarget;

            //Move Pieces
            MovePiece(oldPos, newPositions, MovedPiece, move);

            //Castle and remove Castling Rights
            Castle(newPositions[0], MovedPiece, move);

            newPositions[0].RemoveEnPassantTarget();

            //Check if there is a new enPassant target
            if (MovedPiece.piece == Piece.Pawn)
                if (Math.Abs(MovedPiece.pos.Y() - move.Y()) == 2)
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

            if (enPassant)
            {
                newPositions[0].check = true; //Changing check to be true in case of en Passant Discovered Attack, I'm too lazy to check explicitly for the moment
            }

            // Check if the Enemies could take the King, but only if there was a check or the piece is a King
            if (newPositions[0].check || MovedPiece.piece == Piece.King)
            {
                if (newPositions[0].Illegal())
                {
                    newPositions.Clear();
                    return newPositions;
                }
            }

            if (lastDepth) return newPositions; //if this is the last depth, checks and pins don't have to be calculated

            foreach (Position newPos in newPositions)
            {
                if (MovedPiece.piece == Piece.King)
                {
                    newPos.InitializePins(); //if the king moves, all pins must be recalculated
                }
                else if (MovedPiece.piece == Piece.Knight) { }
                else if (MovedPiece.piece == Piece.Pawn) { }
                else
                {
                    newPos.RemovePin(MovedPiece.pinnedPiece); //Check if piece is unpinning a piece
                }

                if (MovedPiece.piece != Piece.King)
                {
                    newPos.RecalculatePins(MovedPiece.pos); //Check if piece creates a new pin because it moves out of the way
                    newPos.RecalculatePins(move); //Check if piece moves in the way of a pin
                }

                if (enPassant)
                {
                    newPos.InitializePins(); //Recalculating all Pins, I'm too lazy to make this more efficient for the moment
                }

                newPos.doubleCheck = false;

                if (newPos.Check(newPos.Board[move]))
                {
                    if (MovedPiece.DiscoveredCheck(newPos, move))
                    {
                        newPos.doubleCheck = true;
                    }
                    newPos.check = true;
                }

                else
                {
                    newPos.check = MovedPiece.DiscoveredCheck(newPos, move);

                    if (newPos.Board[move].piece > Piece.Knight && newPos.Board[move].piece < Piece.King)
                    {
                        newPos.NewPin(newPos.Board[move].piece, move); //Check if there is a new pin;
                    }
                }

                if (enPassant)
                    newPos.check = true; //Changing check to be true in case of en Passant Discovered Attack, I'm too lazy to check explicitly for the moment

                newPos.hashesThreeFold.Add(newPos.hash);
            }

            return newPositions;
        }

        public static void Castle(Position newPosition, Piece MovedPiece, int move)
        {
            if (MovedPiece.piece == Piece.King)
            {
                if (Math.Abs(MovedPiece.pos.X() - move.X()) == 2)
                {
                    Piece rook = new();
                    int moveRook = 0;

                    if (move.X() == 2)
                    {
                        rook = (Piece)newPosition.Board[(0, move.Y()).PosXYToInt()].Copy();
                        moveRook = (3, move.Y()).PosXYToInt();
                        newPosition.RemoveAt((0, move.Y()).PosXYToInt());
                    }
                    else if (move.X() == 6)
                    {
                        rook = (Piece)newPosition.Board[(7, move.Y()).PosXYToInt()].Copy();
                        moveRook = (5, move.Y()).PosXYToInt();
                        newPosition.RemoveAt((7, move.Y()).PosXYToInt());
                    }
                    newPosition.UpdateBitBoard(rook.pos, moveRook);
                    rook.pos = moveRook;
                    newPosition.Add(rook);
                }
                newPosition.NoCastle(MovedPiece);
                if (MovedPiece.isWhite)
                {
                    newPosition.WhiteKing = move;
                }
                else
                {
                    newPosition.BlackKing = move;
                }
            }
        }

        public static void MovePiece(Position oldPos, List<Position> newPositions, Piece MovedPiece, int move)
        {
            Piece newPiece = (Piece)MovedPiece.CopyPiece();
            if (oldPos.VerifyPin(MovedPiece, move))
            {
                newPiece.pin = MovedPiece.pin;
            }

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
                newPositions[0].EnPassantBB(MovedPiece.isWhite);
            }

            //check if any pawn is promoting
            if (newPiece.Promoting())
            {
                for (int i = 1; i <= 3; i++)
                {
                    newPiece.piece = i;
                    newPositions.Add((Position)newPositions[0].Copy());
                    newPositions[i].Add((Piece)newPiece.CopyPiece());
                }
                newPiece.piece = Piece.Queen;
            }
            newPositions[0].Add(newPiece);
        }
    }
}