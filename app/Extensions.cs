namespace ChessEngine
{
    public static class Extensions
    {
        public static Square[] GetClone(this Square[] source)
        {
            return source.Select(item => (Square)item.Copy()).ToArray();
        }

        public static List<int> GetClone(this List<int> source)
        {
            return source.ToList();
        }

        public static int PosXYToInt (this (int x, int y) source) {
            return source.x + (source.y << 3);
        }

        public static int X (this int source) {
            return source & 0b_111;
        }

        public static int Y (this int source) {
            return source >> 3 & 0b_111;
        }

        public static bool Diagonal (this int pos1, int pos2) {
            return Math.Abs(pos1.X() - pos2.X()) == Math.Abs(pos1.Y() - pos2.Y());
        }

        public static bool HorizontalTo (this int pos1, int pos2) {
            return (pos1.X() ^ pos2.X()) == 0;
        }

        public static bool VerticalTo (this int pos1, int pos2) {
            return (pos1.Y() ^ pos2.Y()) == 0;
        }
    }
}