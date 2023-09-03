namespace ChessEngine
{
    class FEN : Position
    {
        public void FormatPosition(string positionFEN)
        {
            int i = positionFEN.IndexOf(' ');
            string positionPiecesFEN = positionFEN[..i];

            (int x, int y) currPos = (1, 8);

            foreach (char c in positionPiecesFEN)
            {

                Piece piece = new();

                switch (c)
                {
                    case 'p':
                    case 'P':
                        piece.piece = Piece.Pawn;
                        break;
                    case 'n':
                    case 'N':
                        piece.piece = Piece.Knight;
                        break;
                    case 'b':
                    case 'B':
                        piece.piece = Piece.Bishop;
                        break;
                    case 'r':
                    case 'R':
                        piece.piece = Piece.Rook;
                        break;
                    case 'q':
                    case 'Q':
                        piece.piece = Piece.Queen;
                        break;
                    case 'k':
                    case 'K':
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

                switch (c)
                {
                    case 'p':
                    case 'b':
                    case 'n':
                    case 'r':
                    case 'q':
                    case 'k':
                        piece.isWhite = false;
                        break;
                }

                switch (c)
                {
                    case 'p':
                    case 'P':
                    case 'b':
                    case 'B':
                    case 'n':
                    case 'N':
                    case 'r':
                    case 'R':
                    case 'q':
                    case 'Q':
                    case 'k':
                    case 'K':
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

            foreach (Piece piece in Pieces)
            {
                if (piece.isWhite)
                    PiecesWhite.Add(piece);
                else
                    PiecesBlack.Add(piece);
            }
        }

        public string FormatFEN()
        {
            string FEN = "";
            for (int y = 8; y >= 1; y--)
            {
                int counter = 0;
                for (int x = 1; x <= 8; x++)
                {
                    char p = 'x';
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.pos.x == x && piece.pos.y == y)
                        {
                            switch (piece.piece)
                            {
                                case Piece.Pawn:
                                    p = 'p';
                                    break;
                                case Piece.Knight:
                                    p = 'n';
                                    break;
                                case Piece.Bishop:
                                    p = 'b';
                                    break;
                                case Piece.Rook:
                                    p = 'r';
                                    break;
                                case Piece.Queen:
                                    p = 'q';
                                    break;
                                case Piece.King:
                                    p = 'k';
                                    break;
                                default:
                                    break;
                            }
                            if (piece.isWhite)
                            {
                                p = char.ToUpper(p);
                            }
                        }
                    }
                    if (p != 'x')
                    {
                        if (counter != 0)
                        {
                            FEN += counter.ToString();
                            counter = 0;
                        }
                        FEN += p;
                    }
                    else
                    {
                        counter++;
                    }
                }
                if (counter != 0)
                {
                    FEN += counter.ToString();
                }
                if (y != 1) {
                    FEN += "/";
                }
            }
            FEN += ' ';
            if (WhitesTurn) 
                FEN += "w";
            else 
                FEN += "b";
            FEN += ' ';

            if (WShortCastle) 
                FEN += "K";
            if (WLongCastle) 
                FEN += "Q";
            if (BShortCastle) 
                FEN += "k";
            if (BLongCastle) 
                FEN += "q";
            if (!WShortCastle && !WLongCastle && !BShortCastle && !BLongCastle)
                FEN += '-';
            FEN += ' ';

            if (EnPassantTarget != (0, 0)) {
                switch (EnPassantTarget.x) {
                    case 1:
                        FEN += 'a';
                        break;
                    case 2:
                        FEN += 'b';
                        break;
                    case 3:
                        FEN += 'c';
                        break;
                    case 4:
                        FEN += 'd';
                        break;
                    case 5:
                        FEN += 'e';
                        break;
                    case 6:
                        FEN += 'f';
                        break;  
                    case 7: 
                        FEN += 'g';
                        break;
                    case 8:
                        FEN += 'h';
                        break;  
                }
                FEN += EnPassantTarget.y.ToString();
            }
            else {
                FEN += '-';
            }


            return FEN;
        }

        static int ToInt(char c)
        {
            return c - '0';
        }
    }
}