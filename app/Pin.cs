namespace ChessEngine
{
    public struct Pin
    {
        public int pinningPiece;
        public bool pinned = false;
        public bool[] allowedDirections = { false, false, false, false }; //North-South, West-East, Northeast - Southwest, Northwest - Southeast

        public Pin()
        {
        }

        public static Pin Default()
        {
            bool[] defaultValues = { true, true, true, true };
            return new()
            {
                allowedDirections = defaultValues
            };
        }
    }
}