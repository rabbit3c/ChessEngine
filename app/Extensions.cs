namespace ChessEngine
{
    public static class Extensions
    {
        public static List<Piece> GetClone(this List<Piece> source)
        {
            return source.Select(item => (Piece)item.Copy())
                    .ToList();
        }

        public static List<Square> GetClone(this List<Square> source)
        {
            return source.Select(item => (Square)item.Copy())
                    .ToList();
        }

        public static List<int> GetClone(this List<int> source)
        {
            return source.ToList();
        }

        public static (int, int) GetClone(this (int, int) source)
        {
            return (source.Item1, source.Item2);
        }

        public static int PosXYToInt (this (int x, int y) source) {
            return source.x - 1 + (source.y - 1) * 8;
        }

        public static int X (this int source) {
            return (source & 0b_111) + 1;
        }

        public static int Y (this int source) {
            return (source >> 3 & 0b_111) + 1;
        }

        public static (int, int) IntToPosXY (this int source) {
            int y = Math.DivRem(source, 8, out int x);
            return (x + 1, y + 1);
        }
    }
}