namespace ChessEngine
{
    public class Position
    {

        public List<string> Pieces { get; set; } = new();
        public List<string> PiecesWhite { get; set; } = new();
        public List<string> PiecesBlack { get; set; } = new();
        public bool WhitesTurn;

        public Position(bool whitesTurn)
        {
            WhitesTurn = whitesTurn;
        }

        public List<string> OwnPieces()
        {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }

        public List<string> EnemyPieces()
        {
            return WhitesTurn ? PiecesBlack : PiecesWhite;
        }

        public void FormatPosition(string[] position)
        {
            //creating seperate arrays for the white and black pieces
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

            Pieces = position.ToList();
            PiecesWhite = piecesWhite;
            PiecesBlack = piecesBlack;
        }
    }
}