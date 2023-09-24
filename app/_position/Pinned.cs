
namespace ChessEngine
{
    partial class Position
    {
        public void InitializePins()
        {
            foreach (int piece in PiecesWhite)
            {
                Pin pin = Board[piece].IsPinned(this);
                Board[piece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = piece;
            }
            foreach (int piece in PiecesBlack)
            {
                Pin pin = Board[piece].IsPinned(this);
                Board[piece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = piece;
            }
        }

        public void CalculatePins(int i, int posKing)
        { //i: y+, y-, x+, x-, x+y+, x-y-, x-y+, x+y-
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };
            Func<int, int, bool, Square[]>[] functions = {GetFile, GetFile, GetRank, GetRank, GetDiagonal, GetDiagonal, GetDiagonal, GetDiagonal };
            Square[] squares = functions[i](posKing + directions[i], posKing + directions[i] * PrecomputedData.numSquareToEdge[posKing][i], true);
            foreach (Square square in squares)
            {
                if (square.empty) continue;

                square.pin = square.IsPinned(this);
                Board[square.pin.pinningPiece].pinnedPiece = square.pos;
            }
        }

        public void NewPin(int piece, int move)
        {
            int posKing = OwnKing();
            LoopDirections(piece, move, posKing, CheckForPinnedPiece);
        }

        public bool VerifyPin(Piece piece, int move) {
            int posKing = piece.isWhite ? WhiteKing : BlackKing;

            if (!piece.pin.pinned) return false;

            if (move == piece.pin.pinningPiece) return false;

            if (piece.pos.HorizontalTo(move)) {
                if (piece.pos.HorizontalTo(posKing)) return true;
            }
            if (piece.pos.VerticalTo(move)) {
                if (piece.pos.VerticalTo(posKing)) return true;
            }
            else if (piece.pos.Diagonal(move)) {
                if (piece.pos.Diagonal(posKing)) return true;
            }
            return false;
        }

        public void RemovePin(int pinnedPiece)
        {
            if (pinnedPiece == -1) return;

            Board[pinnedPiece].pin = Board[pinnedPiece].IsPinned(this);
        }

        public void RecalculatePins(int pos)
        {
            int[] posisitionsKings = { WhiteKing, BlackKing };

            foreach (int posKing in posisitionsKings)
            {
                if (posKing.HorizontalTo(pos))
                {
                    CalculatePins(pos > posKing ? 0 : 1, posKing);
                }
                else if (posKing.VerticalTo(pos))
                {
                    CalculatePins(pos > posKing ? 3 : 2, posKing);
                }
                else if (pos.Diagonal(posKing))
                {
                    Math.DivRem(pos - posKing, 9, out int remainder);
                    if (remainder == 0)
                    {
                        CalculatePins(pos > posKing ? 4 : 5, posKing);
                    }
                    else
                    {
                        CalculatePins(pos > posKing ? 6 : 7, posKing);
                    }
                }
            }
        }

        void LoopDirections(int piece, int pos1, int pos2, Action<Square[], int, int> func)
        {
            if (piece == Piece.Queen || piece == Piece.Rook)
            {
                if (pos1.HorizontalTo(pos2))
                {
                    Square[] rank = GetFile(pos1, pos2);
                    func(rank, pos1, 0);
                    return;
                }

                if (pos1.VerticalTo(pos2))
                {
                    Square[] file = GetRank(pos1, pos2);
                    func(file, pos1, 1);
                    return;
                }
            }

            if (piece == Piece.Queen || piece == Piece.Bishop)
            {
                if (pos1.Diagonal(pos2))
                {
                    Square[] diagonal = GetDiagonal(pos1, pos2);
                    Math.DivRem(pos1 - pos2, 9, out int remainder);
                    func(diagonal, pos1, remainder == 0 ? 2 : 3);
                    return;
                }
            }
        }

        void CheckForPinnedPiece(Square[] squares, int move, int direction)
        {
            int pinnedPiece = -1;
            foreach (Square square in squares)
            {
                if (square.empty) continue;

                if (square.isWhite != WhitesTurn) return;

                if (pinnedPiece != -1) return; //Don't add new Piece because there are two pieces in the way

                pinnedPiece = square.pos;
            }
            if (pinnedPiece != -1)
            {
                Pin pin = new()
                {
                    pinningPiece = move,
                    pinned = true
                };
                pin.allowedDirections[direction] = true;
                Board[pinnedPiece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = pinnedPiece;
            }
        }
    }
}