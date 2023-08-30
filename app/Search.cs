
namespace ChessEngine
{
    class Search
    {
        public static void Main(string[] position, bool white)
        {
            List<string> piecesWhite = new();
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i][0] == 'w')
                    piecesWhite.Add(position[i]);
            }
            List<string> piecesBlack = new();
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i][0] == 'b')
                    piecesBlack.Add(position[i]);
            }
        }
    }
}