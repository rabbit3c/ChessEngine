
namespace ChessEngine
{
    class Search
    {
        static List<string> ownPieces = new();
        public static void Main(Position pos)
        {
            ownPieces = pos.OwnPieces();
            
            //getting every possible move
            Console.WriteLine("Legal Moves in this position:");
            for (int i = 0; i < ownPieces.Count; i++) {
                switch (ownPieces[i][1]) {
                    case 'K':
                        King.LegalMoves(posKing: StrToTuple(i), pos);
                        break;
                    case 'Q':
                        Queen.LegalMoves(posQueen: StrToTuple(i), pos);
                        break;
                    case 'B':
                        Bishop.LegalMoves(posBishop: StrToTuple(i), pos);
                        break;
                    case 'R':
                        Rook.LegalMoves(posRook: StrToTuple(i), pos);
                        break;
                    case 'N':
                        break;
                    default:
                        Pawn.LegalMoves(posPawn: StrToTuple(i, 1, 2), pos);
                        break;
                }
            }
        }

        static (int, int) StrToTuple(int i, int i1 = 2, int i2 = 3) {
            string str = ownPieces[i];
            return (Convert.ToInt32(str.Substring(i1, 1)), Convert.ToInt32(str.Substring(i2, 1)));
        }
    }
}