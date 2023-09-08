
using System.Net;

namespace ChessEngine
{
    public class Position : ChessBoard
    {
        public ulong hash;
        public List<int> OwnPieces()
        {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }

        public List<int> EnemyPieces()
        {
            return WhitesTurn ? PiecesBlack : PiecesWhite;
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
                hash = hash
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
                HashPiece(targetSquare);
            }
            Board[piece.pos] = new(piece.pos, piece.isWhite, piece.piece);
        }

        public void RemoveAt(int i)
        {
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
            if (piece.isWhite && piece.pos.Y() == 1 && WLongCastle)
            {
                hash ^= PrecomputedData.hashes[770];
                WLongCastle = false;
            }
            else if (!piece.isWhite && piece.pos.Y() == 8 && BLongCastle)
            {
                hash ^= PrecomputedData.hashes[772];
                BLongCastle = false;
            }

        }

        public void NoShortCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.Y() == 1 && WShortCastle)
            {
                hash ^= PrecomputedData.hashes[769];
                WShortCastle = false;
            }
            else if (!piece.isWhite && piece.pos.Y() == 8 && BShortCastle)
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
                if (piece.pos.X() == 8)
                    NoShortCastle(piece);
                else if (piece.pos.X() == 1)
                    NoLongCastle(piece);
            }
        }

        public void Hash()
        {
            hash = 0;
            foreach (Square square in Board)
            {
                if (!square.empty)
                {
                    HashPiece(square);
                }
            }
            if (!WhitesTurn)
                hash ^= PrecomputedData.hashes[768];
            if (WShortCastle)
                hash ^= PrecomputedData.hashes[769];
            if (WLongCastle)
                hash ^= PrecomputedData.hashes[770];
            if (BShortCastle)
                hash ^= PrecomputedData.hashes[771];
            if (BLongCastle)
                hash ^= PrecomputedData.hashes[772];
            if (EnPassantTarget != -1)
            {
                hash ^= PrecomputedData.hashes[EnPassantTarget + 773];
            }
        }
        public void HashPiece(Piece piece)
        {
            int i = 0;
            i += piece.pos;
            i += piece.piece * 64;
            i += Convert.ToInt32(piece.isWhite) * 384;
            hash ^= PrecomputedData.hashes[i];
        }
    }
}