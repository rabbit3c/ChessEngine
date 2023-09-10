
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

            bool enPassant = MovedPiece.piece == Piece.Pawn && move == oldPos.EnPassantTarget;

            //Move Piece
            MovePiece(oldPos, newPositions, MovedPiece, move);

            //Castle and remove Castling Rights
            Castle(newPositions[0], MovedPiece, move);

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

            if (enPassant) {
                newPositions[0].check = true; //Changing check to be true in case of en Passant Discovered Attack, I'm too lazy to check explicitly for the moment
            }

            // Check if the Enemies could take the King, but only if there was a check
            if (newPositions[0].check)
            {
                if (newPositions[0].Illegal())
                {
                    newPositions.Clear();
                    return newPositions;
                }
            }

            foreach (Position newPos in newPositions)
            {
                newPos.check = MovedPiece.DiscoveredCheck(newPos, move) || newPos.Check(newPos.Board[move]);
                if (enPassant) {
                    newPos.check = true; //Changing check to be true in case of en Passant Discovered Attack, I'm too lazy to check explicitly for the moment
                }
            }

            //Console.WriteLine(MovedPiece.piece);
            return newPositions;
        }

        public static void Castle(Position newPosition, Piece MovedPiece, int move)
        {
            if (MovedPiece.piece == Piece.King)
            {
                if (Math.Abs(MovedPiece.pos.X() - move.X()) == 2)
                {
                    Piece rook = new();

                    if (move.X() == 2)
                    {
                        rook = (Piece)newPosition.Board[(0, move.Y()).PosXYToInt()].Copy();
                        rook.pos = (3, move.Y()).PosXYToInt();
                        newPosition.RemoveAt((0, move.Y()).PosXYToInt());
                    }
                    else if (move.X() == 6)
                    {
                        rook = (Piece)newPosition.Board[(7, move.Y()).PosXYToInt()].Copy();
                        rook.pos = (5, move.Y()).PosXYToInt();
                        newPosition.RemoveAt((7, move.Y()).PosXYToInt());
                    }
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
        }
    }
}