namespace ChessEngine
{
    public partial class Position
    {
        public Square[] Board { get; set; } = {
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new(), new(), new(), new(),
        };
        public List<int> PiecesWhite { get; set; } = new();
        public List<int> PiecesBlack { get; set; } = new();

        public int EnPassantTarget { get; set; } = new();
        public bool WhitesTurn = true;
        public bool WShortCastle = true;
        public bool WLongCastle = true;
        public bool BShortCastle = true;
        public bool BLongCastle = true;
        public int WhiteKing;
        public int BlackKing;
        public ulong hash;


        public Square[] GetFile(int iStart, int iEnd, bool including = false)
        {
            List<Square> column = new();
            if (including)
            {
                if (Math.Abs(iStart - iEnd) >= 0 && iStart >= 0 && iEnd >= 0 && iStart < 64 && iEnd < 64)
                {
                    for (int i = Math.Min(iStart, iEnd); i <= Math.Max(iEnd, iStart); i += 8)
                    {
                        column.Add(Board[i]);
                    }
                }
            }
            else
            {
                for (int i = Math.Min(iStart, iEnd) + 8; i <= Math.Max(iEnd, iStart) - 8; i += 8)
                {
                    column.Add(Board[i]);
                }
            }
            return column.ToArray();
        }

        public Square[] GetRank(int iStart, int iEnd, bool including = false)
        {
            if (including)
            {
                if (Math.Abs(iStart - iEnd) >= 0 && iStart >= 0 && iEnd >= 0 && iStart < 64 && iEnd < 64)
                {
                    Square[] lineIncluding = Board[Math.Min(iStart, iEnd)..(Math.Max(iStart, iEnd) + 1)];
                    return lineIncluding;
                }
            }
            else
            {
                Square[] line = Board[(Math.Min(iStart, iEnd) + 1)..Math.Max(iStart, iEnd)];
                return line.ToArray();
            }
            return Array.Empty<Square>();
        }

        public Square[] GetDiagonal(int pos1, int pos2, bool including = false)
        {
            List<Square> diagonal = new();
            int iStart = pos1;
            int iEnd = pos2;
            Math.DivRem(pos1 - pos2, 9, out int rem9);
            if (!including)
            {
                iStart = rem9 == 0 ? Math.Min(pos1, pos2) + 9 : Math.Min(pos1, pos2) + 7;
                iEnd = rem9 == 0 ? Math.Max(pos1, pos2) - 9 : Math.Max(pos1, pos2) - 7;
            }

            if (Math.Abs(iStart - iEnd) >= 0 && iStart >= 0 && iEnd >= 0 && iStart < 64 && iEnd < 64)
            {
                if (rem9 == 0)
                {
                    for (int i = Math.Min(iStart, iEnd); i <= Math.Max(iEnd, iStart); i += 9)
                    {
                        diagonal.Add(Board[i]);
                    }
                    return diagonal.ToArray();
                }
                for (int i = Math.Min(iStart, iEnd); i <= Math.Max(iEnd, iStart); i += 7)
                {
                    diagonal.Add(Board[i]);
                }
                return diagonal.ToArray();
            }
            return Array.Empty<Square>();
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