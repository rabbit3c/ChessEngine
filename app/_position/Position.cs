
namespace ChessEngine
{
    public partial class Position
    {
        public int halfmoves;
        public List<ulong> hashesThreeFold = new();
        public List<int> OwnPieces()
        {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }

        public List<int> EnemyPieces()
        {
            return WhitesTurn ? PiecesBlack : PiecesWhite;
        }

        public int OwnKing()
        {
            return WhitesTurn ? WhiteKing : BlackKing;
        }

        public int EnemyKing() 
        {
            return WhitesTurn ? BlackKing : WhiteKing;
        }

        public object Copy()
        {
            Position copy = new()
            {
                Board = Board.GetClone(),
                PiecesBlack = PiecesBlack.GetClone(),
                PiecesWhite = PiecesWhite.GetClone(),
                EnPassantTarget = EnPassantTarget,
                WhitesTurn = WhitesTurn,
                WLongCastle = WLongCastle,
                WShortCastle = WShortCastle,
                BLongCastle = BLongCastle,
                BShortCastle = BShortCastle,
                hash = hash,
                check = check,
                WhiteKing = WhiteKing,
                BlackKing = BlackKing,
                halfmoves = halfmoves
            };
            return copy;
        }

        public void RemoveEnPassantTarget() {
            if (EnPassantTarget != -1)
            {
                hash ^= PrecomputedData.hashes[EnPassantTarget + 773];
                EnPassantTarget = -1;
            }
        }

        public void AddEnPassantTarget(int pos) {
            EnPassantTarget = pos;
            hash ^= PrecomputedData.hashes[EnPassantTarget + 773];
        }

        public virtual void Add(Piece piece)
        {
            HashPiece(piece);
            Square targetSquare = Board[piece.pos];
            if (!targetSquare.empty) {
                hashesThreeFold.Clear(); //If piece is taken, past positions can't be reached again
                halfmoves = 0; //reset 50 move rule
                HashPiece(targetSquare);
                if (targetSquare.isWhite)
                    PiecesWhite.Remove(targetSquare.pos);
                else
                    PiecesBlack.Remove(targetSquare.pos);
            }
            else if (piece.piece == Piece.Pawn) {
                halfmoves = 0; //reset 50 move rule
            }
            else {
                halfmoves += 1;
            }
            if (piece.isWhite)
                PiecesWhite.Add(piece.pos);
            else 
                PiecesBlack.Add(piece.pos);
            Board[piece.pos] = new(piece.pos, piece.isWhite, piece.piece);
        }

        public void RemoveAt(int i)
        {
            PiecesBlack.Remove(i);
            PiecesWhite.Remove(i);
            HashPiece(Board[i]);
            Board[i] = new();
        }

        public void SplitColors()
        {

            PiecesWhite.Clear();
            PiecesBlack.Clear();

            for (int i = 0; i < 64; i++)
            {
                if (!Board[i].empty)
                {
                    if (Board[i].isWhite)
                        PiecesWhite.Add(i);
                    else
                        PiecesBlack.Add(i);
                }
            }
        }

        public void ToggleTurn()
        {
            WhitesTurn = !WhitesTurn;
            hash ^= PrecomputedData.hashes[768];
        }

        public void NoLongCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.Y() == 0 && WLongCastle)
            {
                hash ^= PrecomputedData.hashes[770];
                WLongCastle = false;
            }
            else if (!piece.isWhite && piece.pos.Y() == 7 && BLongCastle)
            {
                hash ^= PrecomputedData.hashes[772];
                BLongCastle = false;
            }

        }

        public void NoShortCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.Y() == 0 && WShortCastle)
            {
                hash ^= PrecomputedData.hashes[769];
                WShortCastle = false;
            }
            else if (!piece.isWhite && piece.pos.Y() == 7 && BShortCastle)
            {
                hash ^= PrecomputedData.hashes[771];
                BShortCastle = false;

            }
        }

        public void NoCastle(Piece piece)
        {
            if (piece.piece == Piece.King)
            {
                NoShortCastle(piece);
                NoLongCastle(piece);
            }
            else if (piece.piece == Piece.Rook)
            {
                if (piece.pos.X() == 7)
                    NoShortCastle(piece);
                else if (piece.pos.X() == 0)
                    NoLongCastle(piece);
            }
        }

        public bool ThreeFold() {
            ulong position = hashesThreeFold.Last();
            int counter = 0;
            foreach (ulong hash in hashesThreeFold) {
                if (hash == position)
                    counter ++;
                if (counter == 3)
                    return true;
            }
            return false;
        }
    }
}