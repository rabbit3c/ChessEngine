
namespace ChessEngine
{
    public class Piece
    {
        public int pos;
        public bool isWhite;
        public int piece;
        public Pin pin = Pin.Default();
        public int pinnedPiece = -1;
        public const int Pawn = 0;
        public const int Knight = 1;
        public const int Bishop = 2;
        public const int Rook = 3;
        public const int Queen = 4;
        public const int King = 5;

        public Piece(bool whitePiece = true, int pieceType = 0)
        {
            piece = pieceType;
            isWhite = whitePiece;
        }

        public object CopyPiece()
        {
            Piece copy = new()
            {
                pos = pos,
                piece = piece,
                isWhite = isWhite
            };
            return copy;
        }

        public bool DiscoveredCheck(Position position, int move)
        {
            int posKing = position.OwnKing();
            if (pos.VerticalTo(posKing))
            {
                if (move.VerticalTo(posKing)) return false;

                if (!Move.NothingInTheWay(posKing, pos, position)) return false;

                Square[] file = pos < posKing ? position.GetFile(pos.X(), pos - 8, true) : position.GetFile(pos + 8, pos.X() + 56, true);
                return CheckSquares(file, posKing, Rook, isWhite, out int _);
            }
            else if (pos.HorizontalTo(posKing))
            {
                if (move.HorizontalTo(posKing)) return false;

                if (!Move.NothingInTheWay(posKing, pos, position)) return false;

                Square[] rank = pos < posKing ? position.GetRank(pos.Y() * 8, pos - 1, true) : position.GetRank(pos + 1, pos.Y() * 8 + 7, true);
                return CheckSquares(rank, posKing, Rook, isWhite, out int _);
            }
            else if (pos.Diagonal(posKing))
            {
                if (move.Diagonal(posKing)) return false;
                    
                if (!Move.NothingInTheWay(posKing, pos, position)) return false;

                Math.DivRem(pos - posKing, 9, out int remainder);
                Square[] diagonal;
                if (remainder == 0)
                {
                    diagonal = pos < posKing ? position.GetDiagonal(pos - 9 * PrecomputedData.numSquareToEdge[pos][5], pos - 9, true) : position.GetDiagonal(pos + 9, pos + 9 * PrecomputedData.numSquareToEdge[pos][4], true);
                }
                else
                {
                    diagonal = pos < posKing ? position.GetDiagonal(pos - 7 * PrecomputedData.numSquareToEdge[pos][7], pos - 7, true) : position.GetDiagonal(pos + 7, pos + 7 * PrecomputedData.numSquareToEdge[pos][6], true);
                }
                return CheckSquares(diagonal, posKing, Bishop, isWhite, out int _);
            }
            return false;
        }

        public Pin IsPinned(Position position)
        {
            Pin pin = new();

            int posKing = isWhite ? position.WhiteKing : position.BlackKing;

            if (pos.VerticalTo(posKing))
            {
                if (Move.NothingInTheWay(posKing, pos, position))
                {
                    Square[] file = pos < posKing ? position.GetFile(pos.X(), pos - 8, true) : position.GetFile(pos + 8, pos.X() + 56, true);
                    pin.pinned = CheckSquares(file, posKing, Rook, !isWhite, out pin.pinningPiece);
                }
                if (pin.pinned)
                    pin.allowedDirections[0] = true;
            }

            else if (pos.HorizontalTo(posKing))
            {
                if (Move.NothingInTheWay(posKing, pos, position))
                {
                    Square[] rank = pos < posKing ? position.GetRank(pos.Y() * 8, pos - 1, true) : position.GetRank(pos + 1, pos.Y() * 8 + 7, true);
                    pin.pinned = CheckSquares(rank, posKing, Rook, !isWhite, out pin.pinningPiece);
                }
                if (pin.pinned)
                    pin.allowedDirections[1] = true;
            }

            else if (pos.Diagonal(posKing))
            {
                Math.DivRem(pos - posKing, 9, out int remainder);
                if (remainder == 0)
                {
                    if (Move.NothingInTheWay(posKing, pos, position))
                    {
                        Square[] diagonal = pos < posKing ? position.GetDiagonal(pos - 9 * PrecomputedData.numSquareToEdge[pos][5], pos - 9, true) : position.GetDiagonal(pos + 9, pos + 9 * PrecomputedData.numSquareToEdge[pos][4], true);
                        pin.pinned = CheckSquares(diagonal, posKing, Bishop, !isWhite, out pin.pinningPiece);
                    }
                    if (pin.pinned)
                        pin.allowedDirections[2] = true;
                }
                else
                {
                    if (Move.NothingInTheWay(posKing, pos, position))
                    {
                        Square[] diagonal = pos < posKing ? position.GetDiagonal(pos - 7 * PrecomputedData.numSquareToEdge[pos][7], pos - 7, true) : position.GetDiagonal(pos + 7, pos + 7 * PrecomputedData.numSquareToEdge[pos][6], true);
                        pin.pinned = CheckSquares(diagonal, posKing, Bishop, !isWhite, out pin.pinningPiece);
                    }
                    if (pin.pinned)
                        pin.allowedDirections[3] = true;

                }
            }
            if (pin.pinned) return pin;
            return Pin.Default();
        }

        public bool CheckSquares(Square[] squares, int posKing, int AttackingPiece, bool white, out int piece)
        {
            bool asc = pos > posKing; //bool to skim through array from posKing to pos
            piece = 0;
            for (int i = asc ? 0 : squares.Length - 1; asc ? i < squares.Length : i >= 0; i += asc ? 1 : -1)
            {
                if (squares[i].empty) continue;

                if (squares[i].piece != Queen && squares[i].piece != AttackingPiece) return false;

                if (squares[i].isWhite != white) return false;

                piece = squares[i].pos;
                return true;
            }
            return false;
        }

        public List<int> LegalMoves(Position pos)
        {
            return Move.SlidingMoves(this, pos);
        }

        public bool Promoting()
        {
            if (piece == Pawn)
                return isWhite ? pos.Y() == 7 : pos.Y() == 0;
            return false;
        }
    }
}