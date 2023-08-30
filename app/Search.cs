
namespace ChessEngine
{
    class Search
    {
        public static void Main(string[] position, bool whitesTurn)
        {   
            Position pos = new(whitesTurn);

            //creating seperate arrays for the white and black pieces
            List<string> piecesWhite = new();
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i][0] == 'w')
                    piecesWhite.Add(position[i]);
            }
            pos.PiecesWhite = piecesWhite;

            List<string> piecesBlack = new();
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i][0] == 'b')
                    piecesBlack.Add(position[i]);
            }
            pos.PiecesBlack = piecesBlack;

            //getting every possible move
            List<string> ownPieces = pos.OwnPieces();
            for (int i = 0; i < ownPieces.Count; i++) {
                switch (ownPieces[i][1]) {
                    case 'K':
                        King.LegalMoves(posKing: StrToTuple(ownPieces[i], 2, 3), pos);
                        break;
                    case 'Q':
                        break;
                    case 'B':
                        break;
                    case 'R':
                        break;
                    case 'N':
                        break;
                    default:
                        break;
                }
            }
        }

        static (int, int) StrToTuple(string str, int i1, int i2) {
            return (Convert.ToInt32(str.Substring(i1, 1)), Convert.ToInt32(str.Substring(i2, 1)));
        }
    }
}