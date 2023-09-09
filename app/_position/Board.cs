namespace ChessEngine
{
    public class ChessBoard
    {
        public List<Square> Board { get; set; } = new List<Square>() {
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

        public List<Square> GetFile(int iStart, int iEnd, bool including = false)
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
            return column;
        }

        public List<Square> GetRank(int iStart, int iEnd, bool including = false)
        {
            if (including)
            {
                if (Math.Abs(iStart - iEnd) >= 0 && iStart >= 0 && iEnd >= 0 && iStart < 64 && iEnd < 64)
                {
                    List<Square> lineIncluding = Board.GetRange(Math.Min(iStart, iEnd), Math.Abs(iEnd - iStart) + 1);
                    return lineIncluding;
                }
            }
            else
            {
                List<Square> line = Board.GetRange(Math.Min(iStart, iEnd) + 1, Math.Abs(iEnd - iStart) - 1);
                return line;
            }
            return new();
        }

        public List<Square> GetDiagonal(int pos1, int pos2, bool including = false)
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
                    return diagonal;
                }
                for (int i = Math.Min(iStart, iEnd); i <= Math.Max(iEnd, iStart); i += 7)
                {
                    diagonal.Add(Board[i]);
                }
                return diagonal;
            }
            return new();
        }
    }
}