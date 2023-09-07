
namespace ChessEngine
{
    public class Position : ChessBoard
    {
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
                EnPassantTarget = EnPassantTarget.GetClone(),
                WhitesTurn = WhitesTurn,
                WLongCastle = WLongCastle,
                WShortCastle = WShortCastle,
                BLongCastle = BLongCastle,
                BShortCastle = BShortCastle
            };
            return copy;
        }

        public object Convert()
        {
            FEN copy = new()
            {
                Board = Board.GetClone(),
                PiecesBlack = PiecesBlack.GetClone(),
                PiecesWhite = PiecesWhite.GetClone(),
                EnPassantTarget = EnPassantTarget.GetClone(),
                WhitesTurn = WhitesTurn,
                WLongCastle = WLongCastle,
                WShortCastle = WShortCastle,
                BLongCastle = BLongCastle,
                BShortCastle = BShortCastle
            };
            return copy;
        }

        public void RemoveAt(int i)
        {
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
        }

        public void NoLongCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.y == 1)
                WLongCastle = false;
            else if (!piece.isWhite && piece.pos.y == 8)
                BLongCastle = false;

        }

        public void NoShortCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.y == 1)
                WShortCastle = false;
            else if (!piece.isWhite && piece.pos.y == 8)
                BShortCastle = false;
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
                if (piece.pos.x == 8)
                    NoShortCastle(piece);
                else if (piece.pos.x == 1)
                    NoLongCastle(piece);
            }
        }

        public ulong Hash()
        {
            ulong hash = 0;
            foreach (Square square in Board)
            {
                if (!square.empty)
                {
                    int i = 0;
                    i += square.pos.x - 1 + (square.pos.y - 1) * 8;
                    i += square.piece * 64;
                    i += System.Convert.ToInt32(square.isWhite) * 384;
                    hash ^= ZobristHashes.hashes[i];
                }
            }
            if (!WhitesTurn)
                hash ^= ZobristHashes.hashes[768];
            if (WShortCastle)
                hash ^= ZobristHashes.hashes[769];
            if (WLongCastle)
                hash ^= ZobristHashes.hashes[770];
            if (BShortCastle)
                hash ^= ZobristHashes.hashes[771];
            if (BLongCastle)
                hash ^= ZobristHashes.hashes[772];
            if (EnPassantTarget != (0, 0))
            {
                ;
                hash ^= ZobristHashes.hashes[EnPassantTarget.PosXYToInt() + 773];
            }
            return hash;
        }
    }
}