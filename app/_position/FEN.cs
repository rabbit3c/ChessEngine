namespace ChessEngine
{
    public class FEN : Position
    {
        public void FormatPosition(string positionFEN)
        {
            int i = positionFEN.IndexOf(' ');
            string positionPiecesFEN = positionFEN[..i];

            (int x, int y) currPos = (0, 7);

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
                        if (c == 'K')
                            WhiteKing = currPos.PosXYToInt();
                        else
                            BlackKing = currPos.PosXYToInt();
                        break;
                    case '/':
                        currPos.y--;
                        currPos.x = 0;
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
                        piece.pos = currPos.PosXYToInt();
                        Board[currPos.PosXYToInt()] = new(piece);
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
                EnPassantTarget = (char.ToUpper(info[0]) - 65, ToInt(info[1]) - 1).PosXYToInt();
            else
            {
                EnPassantTarget = -1;
            }
            if (info.Contains(' '))
            {
                info = info[(info.IndexOf(' ') + 1)..];
                if (int.TryParse(info[..(info.Contains(' ') ? info.IndexOf(' ') : info.Length - 1)], out int num))
                {
                    halfmoves = num;
                }
            }
            else
            {
                halfmoves = 0;
            }

            SplitColors();
            InitializePins();
            InitializeBitBoards();
            check = Check(OwnKing(), out checkBB);
        }

        public static string FormatFEN(Position pos)
        {
            string FEN = "";
            for (int y = 7; y >= 0; y--)
            {
                int counter = 0;
                for (int x = 0; x <= 7; x++)
                {
                    char p = 'x';
                    foreach (Square square in pos.Board)
                    {
                        if (!square.empty)
                        {
                            if (square.pos.X() == x && square.pos.Y() == y)
                            {
                                switch (square.piece)
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
                                if (square.isWhite)
                                {
                                    p = char.ToUpper(p);
                                }
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
                if (y != 0)
                {
                    FEN += "/";
                }
            }
            FEN += ' ';
            if (pos.WhitesTurn)
                FEN += "w";
            else
                FEN += "b";
            FEN += ' ';

            if (pos.WShortCastle)
                FEN += "K";
            if (pos.WLongCastle)
                FEN += "Q";
            if (pos.BShortCastle)
                FEN += "k";
            if (pos.BLongCastle)
                FEN += "q";
            if (!pos.WShortCastle && !pos.WLongCastle && !pos.BShortCastle && !pos.BLongCastle)
                FEN += '-';
            FEN += ' ';

            if (pos.EnPassantTarget != -1)
            {
                FEN += char.ToLower(Convert.ToChar(pos.EnPassantTarget.X() + 65));
                FEN += (pos.EnPassantTarget.Y() + 1).ToString();
            }
            else
            {
                FEN += '-';
            }
            FEN += ' ';
            FEN += pos.halfmoves;


            return FEN;
        }

        static int ToInt(char c)
        {
            return c - '0';
        }
    }
}