namespace ChessEngine {
    class FEM: Position {
        public void FormatPosition(string positionFEN)
        {   
            int i = positionFEN.IndexOf(' ');
            string positionPiecesFEN = positionFEN[..i];

            (int x, int y) currPos = (1, 8);

            foreach (char c in positionPiecesFEN) {

                Piece piece = new();

                switch (c) {
                    case 'p': case 'P':
                        piece.piece = Piece.Pawn;
                        break;
                    case 'n': case 'N':
                        piece.piece = Piece.Knight;
                        break;
                    case 'b': case 'B':
                        piece.piece = Piece.Bishop;
                        break;
                    case 'r': case 'R':
                        piece.piece = Piece.Rook;
                        break;
                    case 'q': case 'Q':
                        piece.piece = Piece.Queen;
                        break;
                    case 'k': case 'K':
                        piece.piece = Piece.King;
                        break;
                    case '/':
                        currPos.y--;
                        currPos.x = 1;
                        break;
                    default:
                        int n = ToInt(c);
                        currPos.x += n;
                        break;
                }
                
                switch (c) {
                    case 'p': case 'b': case 'n': case 'r': case'q': case'k':
                    piece.isWhite = false;
                    break;
                }

                switch (c) {
                    case 'p': case 'P': case 'b': case 'B': case 'n': case 'N': case 'r': case 'R': case'q': case'Q': case'k': case 'K':
                        piece.pos = currPos;
                        Pieces.Add(piece);
                        currPos.x++;
                        break;
                    default:
                        break;
                }
            }
            string info = positionFEN[(i + 1)..];
            if (info[0] == 'b') 
                WhitesTurn = false;

            info = info[2..];
            string infoCastling = info[..info.IndexOf(' ')];

            if (!infoCastling.Contains('K'))
                WShortCastle = false;
            if (!infoCastling.Contains('k'))
                BShortCastle = false;
            if (!infoCastling.Contains('Q'))
                WLongCastle = false;
            if (!infoCastling.Contains('q'))
                BLongCastle = false;

            info = info[(info.IndexOf(' ') + 1)..];
            if (info[0] != '-')
                EnPassantTarget = (char.ToUpper(info[0]) - 64, ToInt(info[1]));

            foreach (Piece piece in Pieces) {
                if (piece.isWhite) 
                    PiecesWhite.Add(piece);
                else
                    PiecesBlack.Add(piece);
            }
        }

        static int ToInt(char c) {
            return c - '0';
        }
    }
}