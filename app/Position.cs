namespace ChessEngine
{
    public class Position
    {

        public List<Piece> Pieces { get; set; } = new();
        public List<Piece> PiecesWhite { get; set; } = new();
        public List<Piece> PiecesBlack { get; set; } = new();
        public bool WhitesTurn;

        public bool WShortCastle = true;
        public bool WLongCastle = true;
        public bool BShortCastle = true;
        public bool BLongCastle = true;

        public Position(bool whitesTurn, bool wShortCastle = true, bool wLongCastle = true, bool bShortCastle = true, bool bLongCastle = true)
        {
            WhitesTurn = whitesTurn;
            WShortCastle = wShortCastle;
            WLongCastle = wLongCastle;
            BShortCastle = bShortCastle;
            BLongCastle = bLongCastle;
        }

        public List<Piece> OwnPieces()
        {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }

        public List<Piece> EnemyPieces()
        {
            return WhitesTurn ? PiecesBlack : PiecesWhite;
        }

        public void FormatPosition(string[] position)
        {

            foreach (string piece in position) {
                bool whitePiece = false;
                if (piece[0] == 'w') {
                    whitePiece = true;
                }
                (int, int) pos = (ToInt(piece[2]), ToInt(piece[3]));
                Piece piece1 = new(pos, whitePiece, ToInt(piece[1]));
                Pieces.Add(piece1);
            }

            foreach (Piece piece in Pieces) {
                if (piece.isWhite) 
                    PiecesWhite.Add(piece);
                else
                    PiecesBlack.Add(piece);
            }
        }

        public void ChangePosition((int x, int y) posPiece, (int x, int y) move) {
            Piece newPiece = new();
            foreach (Piece piece in Pieces) {
                if (piece.pos == posPiece) {
                    Pieces.Remove(piece);
                    newPiece = piece;
                }
                if (piece.pos == posPiece) {
                    Pieces.Remove(piece);
                }
            }
            newPiece.pos = move;
            Pieces.Add(newPiece);
        }

        static int ToInt(char c) {
            return (int)(c - '0');
        }
    }
}