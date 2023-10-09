namespace ChessEngine
{
    public struct Pin
    {
        public int pinningPiece;
        public bool pinned = false;
        public int allowedDirections = -1; //North-South, West-East, Northeast - Southwest, Northwest - Southeast

        public Pin()
        {
        }
    }
}