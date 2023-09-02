namespace ChessEngine
{
    class NewPos
    {
        public static Position Format(Position oldPos, Piece MovedPiece, (int x, int y) move)
        {
            Position newPos = (Position)oldPos.Copy();
            Piece newPiece = new();
            List<Piece> pieces = newPos.Pieces;

            for (int i = 0; i < pieces.Count;)
            {
                if (pieces[i].pos == MovedPiece.pos)
                {
                    newPiece = (Piece)pieces[i].Copy();
                    newPos.RemoveAt(i);
                }
                else if (pieces[i].pos == move)
                {
                    if (pieces[i].piece == Piece.Rook)
                    {
                        newPos.NoCastle(pieces[i]);
                    }
                    newPos.RemoveAt(i);
                }
                else if (MovedPiece.piece == Piece.Pawn && move == oldPos.EnPassantTarget && pieces[i].piece == Piece.Pawn && MovedPiece.isWhite ? pieces[i].pos == (move.x, move.y - 1) : pieces[i].pos == (move.x, move.y + 1))
                    newPos.RemoveAt(i);
                else
                    i++;

            }

            newPiece.pos = move;
            newPos.Add(newPiece);

            newPos.EnPassantTarget = (0, 0);

            if (MovedPiece.piece == Piece.Pawn && Math.Abs(MovedPiece.pos.y - move.y) == 2)
                newPos.EnPassantTarget = (MovedPiece.pos.x, MovedPiece.pos.y + 1);
            if (MovedPiece.piece == Piece.King)
            {

                if (Math.Abs(MovedPiece.pos.x - move.x) == 2)
                    foreach (Piece piece in oldPos.Pieces)
                    {
                        if (move.x == 3)
                            if (piece.pos == (1, move.y))
                                piece.pos = (4, move.y);
                            else
                            if (piece.pos == (8, move.y))
                                piece.pos = (6, move.y);
                    }

                newPos.NoCastle(MovedPiece);
            }

            if (MovedPiece.piece == Piece.Rook)
            {
                newPos.NoCastle(MovedPiece);
            }

            newPos.SplitColors();
            newPos.ToggleTurn();

            return newPos;
        }

        public static List<Position> New(Position oldPos, Piece Piece, List<(int x, int y)> moves, out int newPos)
        {
            List<Position> newPositions = new();
            newPos = 0;
            foreach ((int, int) move in moves)
            {
                newPositions.Add(Format(oldPos, Piece, move));
                newPos++;
            }
            return newPositions;
        }
    }
}