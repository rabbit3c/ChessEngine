
namespace ChessEngine
{
    partial class Position
    {
        public void InitializePins()
        {
            foreach (int piece in PiecesWhite)
            {
                Pin pin = Board[piece].IsPinned(this);
                Board[piece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = piece;
            }

            foreach (int piece in PiecesBlack)
            {
                Pin pin = Board[piece].IsPinned(this);
                Board[piece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = piece;
            }

        }

        void CalculatePins(int pos, int i, int posKing)
        { //i: y+, y-, x+, x-, x+y+, x-y-, x-y+, x+y-
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };
            Func<int, int, bool, Square[]>[] functions = { GetFile, GetFile, GetRank, GetRank, GetDiagonal, GetDiagonal, GetDiagonal, GetDiagonal };

            bool asc = directions[i] > 0;
            if (!Board[pos].empty)
            {
                if (posKing + directions[i] != pos && !Move.NothingInTheWay(pos, posKing, this))
                {
                    Square[] squares = functions[i](posKing, pos, false);

                    for (int n = asc ? 0 : squares.Length - 1; asc ? n < squares.Length : n >= 0; n += asc ? 1 : -1)
                    {
                        if (squares[n].empty) continue;

                        if (squares[n].isWhite != Board[posKing].isWhite) return;

                        if (squares[n].pin.pinningPiece == pos) return;

                        if (squares[n].pin.pinningPiece < pos && asc) return;

                        if (squares[n].pin.pinningPiece > pos && !asc) return;

                        squares[n].pin = Pin.Default();

                        return; //Stop after first piece
                    }
                }
                else
                {
                    Square[] squares = functions[i](pos + directions[i], pos + directions[i] * PrecomputedData.numSquareToEdge[pos][i], true);

                    for (int n = asc ? 0 : squares.Length - 1; asc ? n < squares.Length : n >= 0; n += asc ? 1 : -1)
                    {
                        if (squares[n].empty) continue;

                        if (squares[n].isWhite != Board[posKing].isWhite)
                        {
                            if (Board[pos].isWhite != Board[posKing].isWhite) return;

                            if (squares[n].piece != (i < 4 ? Piece.Rook : Piece.Bishop)) return;

                            Board[pos].pin = new()
                            {
                                pinned = true,
                                pinningPiece = squares[n].pos
                            };
                            Board[pos].pin.allowedDirections[Math.DivRem(i, 2, out _)] = true;
                            Board[Board[pos].pin.pinningPiece].pinnedPiece = pos;

                            return;
                        }

                        squares[n].pin = Pin.Default();
                        return;
                    }
                }
            }
            else
            {
                Square[] squares = functions[i](posKing + directions[i], posKing + directions[i] * PrecomputedData.numSquareToEdge[posKing][i], true);

                for (int n = asc ? 0 : squares.Length - 1; asc ? n < squares.Length - 1 : n > 0; n += asc ? 1 : -1)
                {
                    if (squares[n].empty) continue;

                    if (squares[n].isWhite != Board[posKing].isWhite) return;

                    squares[n].pin = IsPinned(squares[n], asc ? squares[(n + 1)..] : squares[..n], i);
                    Board[squares[n].pin.pinningPiece].pinnedPiece = squares[n].pos;

                    return; //Stop after first piece
                }
            }
        }

        void DeletePinsKing(int posKing, bool isWhite)
        {
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };
            for (int j = 0; j < directions.Length; j++)
            {
                if (PrecomputedData.numSquareToEdge[posKing][j] < 1) continue;

                for (int i = posKing + directions[j]; directions[j] > 0 ? i <= posKing + directions[j] * PrecomputedData.numSquareToEdge[posKing][j] : i >= 0; i += directions[j])
                {
                    if (Board[i].empty) continue;

                    if (isWhite != Board[i].isWhite) break;

                    if (Board[i].piece == Piece.King) continue;

                    Board[i].pin = Pin.Default();
                    break;
                }
            }
        }

        void NewPinsKing(int move)
        {
            int[] directions = { 8, -8, -1, 1, 9, -9, 7, -7 };
            for (int j = 0; j < directions.Length; j++)
            {
                int pinnedPiece = -1;
                int pinningPiece = -1;

                if (PrecomputedData.numSquareToEdge[move][j] < 1) continue;

                for (int i = move + directions[j];
                     directions[j] > 0 ? i <= move + directions[j] * PrecomputedData.numSquareToEdge[move][j] :
                     i >= move + directions[j] * PrecomputedData.numSquareToEdge[move][j];
                     i += directions[j])
                {
                    if (Board[i].empty) continue;

                    if (Board[move].isWhite != Board[i].isWhite)
                    {
                        int piece = Board[i].piece;
                        if (pinnedPiece == -1) break;

                        if (j < 4)
                        {
                            if (piece != Piece.Rook && piece != Piece.Queen)
                            {
                                pinnedPiece = -1;
                                break;
                            }
                        }
                        else if (piece != Piece.Bishop && piece != Piece.Queen)
                        {
                            pinnedPiece = -1;
                            break;
                        }

                        pinningPiece = i;
                        break;
                    }

                    if (pinnedPiece != -1)
                    {
                        pinnedPiece = -1;
                        break;
                    }

                    pinnedPiece = i;
                }
                if (pinnedPiece != -1 && pinningPiece != -1)
                {
                    Pin pin = new()
                    {
                        pinningPiece = pinningPiece,
                        pinned = true
                    };
                    pin.allowedDirections[Math.DivRem(j, 2, out _)] = true;
                    Board[pinnedPiece].pin = pin;
                    Board[pin.pinningPiece].pinnedPiece = pinnedPiece;
                }
            }
        }

        public void RecalculatePinsKing(int posKing, bool isWhite, int move)
        {
            DeletePinsKing(posKing, isWhite);
            NewPinsKing(move);
        }

        Pin IsPinned(Piece piece, Square[] squares, int i)
        {
            Pin pin = new();

            Func<int, int, bool, Square[]>[] functions = { GetFile, GetFile, GetRank, GetRank, GetDiagonal, GetDiagonal, GetDiagonal, GetDiagonal };

            int posKing = piece.isWhite ? WhiteKing : BlackKing;
            int pos = piece.pos;

            if (!Move.NothingInTheWay(posKing, pos, this))
            {
                return Pin.Default();
            }

            pin.pinned = CheckSquares(squares, posKing, i < 4 ? Piece.Rook : Piece.Bishop, !piece.isWhite, pos, out pin.pinningPiece);

            if (pin.pinned)
            {
                pin.allowedDirections[Math.DivRem(i, 2, out _)] = true;
                return pin;
            }
            return Pin.Default();
        }

        static bool CheckSquares(Square[] squares, int posKing, int AttackingPiece, bool white, int pos, out int piece)
        {
            bool asc = pos > posKing; //bool to skim through array from posKing to pos
            piece = 0;
            for (int i = asc ? 0 : squares.Length - 1; asc ? i < squares.Length : i >= 0; i += asc ? 1 : -1)
            {
                if (squares[i].empty) continue;

                if (squares[i].piece != Piece.Queen && squares[i].piece != AttackingPiece) return false;

                if (squares[i].isWhite != white) return false;

                piece = squares[i].pos;
                return true;
            }
            return false;
        }

        public void NewPin(int piece, int move)
        {
            int posKing = OwnKing();
            LoopDirections(piece, posKing, move, CheckForPinnedPiece); //pos1 has to be the position of the king
        }

        public bool VerifyPin(Piece piece, int move)
        {
            int posKing = piece.isWhite ? WhiteKing : BlackKing;
            Func<int, int, bool>[] funcsAlignment = { Extensions.VerticalTo, Extensions.HorizontalTo, Extensions.Diagonal };

            if (!piece.pin.pinned) return false;

            if (move == piece.pin.pinningPiece) return false;

            foreach (Func<int, int, bool> funcAlignment in funcsAlignment)
            {
                if (funcAlignment(piece.pos, move))
                {
                    if (funcAlignment(piece.pos, posKing)) return true;
                }
            }

            return false;
        }

        public void RemovePin(int pinnedPiece)
        {
            if (pinnedPiece == -1) return;

            Board[pinnedPiece].pin = Board[pinnedPiece].IsPinned(this);
        }

        public void RecalculatePins(int pos, int move, bool capture = false)
        {
            List<int> positionsKings = new() { WhiteKing, BlackKing };

            if (Board[move].piece == Piece.King)
            {
                positionsKings.Remove(move);
            }

            Func<int, int, bool>[] funcsAlignment = { Extensions.VerticalTo, Extensions.HorizontalTo, Extensions.Diagonal };
            Func<int, int, bool>[] funcsPin = { VerticalPin, HorizontalPin, DiagonalPin, VerticalPin, HorizontalPin };

            foreach (int posKing in positionsKings)
            {
                bool aligned = false;
                for (int i = 0; i < 3; i++)
                {
                    if (funcsAlignment[i](pos, move))
                    {
                        if (funcsPin[i + 1](posKing, pos))
                        {
                            funcsPin[i + 2](posKing, move);
                        }
                        else if (funcsPin[i + 2](posKing, pos))
                        {
                            funcsPin[i + 1](posKing, move);
                        }
                        else if (funcsPin[i + 1](posKing, move)) { }
                        else if (funcsPin[i + 2](posKing, move)) { }

                        if (capture)
                        {
                            funcsPin[i](posKing, pos);
                            funcsPin[i](posKing, move);
                        }

                        aligned = true;
                        break;
                    }
                }

                if (aligned) continue;

                foreach (Func<int, int, bool> funcPin in funcsPin)
                {
                    if (funcPin(posKing, pos)) break;
                }

                foreach (Func<int, int, bool> funcPin in funcsPin)
                {
                    if (funcPin(posKing, move)) break;
                }
            }
        }

        bool VerticalPin(int posKing, int pos)
        {
            if (!posKing.VerticalTo(pos)) return false;
            CalculatePins(pos, pos > posKing ? 0 : 1, posKing);
            return true;
        }

        bool HorizontalPin(int posKing, int pos)
        {
            if (!posKing.HorizontalTo(pos)) return false;
            CalculatePins(pos, pos > posKing ? 3 : 2, posKing);
            return true;
        }

        bool DiagonalPin(int posKing, int pos)
        {
            if (!pos.Diagonal(posKing)) return false;

            Math.DivRem(pos - posKing, 9, out int remainder);
            if (remainder == 0)
            {
                CalculatePins(pos, pos > posKing ? 4 : 5, posKing);

            }
            else
            {
                CalculatePins(pos, pos > posKing ? 6 : 7, posKing);
            }
            return true;
        }

        void LoopDirections(int piece, int pos1, int pos2, Action<Square[], int, int, bool> func)
        {
            if (piece == Piece.Queen || piece == Piece.Rook)
            {
                if (pos1.VerticalTo(pos2))
                {
                    Square[] rank = GetFile(pos1, pos2);
                    func(rank, pos2, 0, pos2 > pos1);
                    return;
                }

                if (pos1.HorizontalTo(pos2))
                {
                    Square[] file = GetRank(pos1, pos2);
                    func(file, pos2, 1, pos2 > pos1);
                    return;
                }
            }

            if (piece == Piece.Queen || piece == Piece.Bishop)
            {
                if (pos1.Diagonal(pos2))
                {
                    Square[] diagonal = GetDiagonal(pos1, pos2);
                    Math.DivRem(pos1 - pos2, 9, out int remainder);
                    func(diagonal, pos2, remainder == 0 ? 2 : 3, pos2 > pos1);
                    return;
                }
            }
        }

        void CheckForPinnedPiece(Square[] squares, int move, int direction, bool asc)
        {
            int pinnedPiece = -1;

            for (int i = asc ? 0 : squares.Length - 1; asc ? i < squares.Length : i >= 0; i += asc ? 1 : -1)
            {
                Square square = squares[i];

                if (square.empty) continue;

                if (square.isWhite != WhitesTurn) return;

                if (pinnedPiece != -1) return; //Don't add new Piece because there are two pieces in the way

                pinnedPiece = square.pos;
            }

            if (pinnedPiece != -1)
            {
                Pin pin = new()
                {
                    pinningPiece = move,
                    pinned = true
                };
                pin.allowedDirections[direction] = true;
                Board[pinnedPiece].pin = pin;
                Board[pin.pinningPiece].pinnedPiece = pinnedPiece;
            }
        }
    }
}