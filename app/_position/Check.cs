
namespace ChessEngine
{

    public partial class Position
    {
        public bool check;
        public bool doubleCheck;
        public bool Illegal() //Can the Enemy King be taken?
        {
            return KingThreat(EnemyKing(), OwnPieces(), out _);
        }

        public bool Check(int position, out ulong checkBitboard)
        {
            return KingThreat(position, EnemyPieces(), out checkBitboard);
        }

        public bool Check(Piece piece, out ulong bitboard)
        {
            int posKing = OwnKing();
            bitboard = 0;

            if (piece.piece == Piece.Queen || piece.piece == Piece.Rook)
            {
                if (piece.pos.VerticalTo(posKing) || piece.pos.HorizontalTo(posKing))
                {
                    if (Move.NothingInTheWay(posKing, piece.pos, this))
                    {
                        bitboard = Bitboards.MaskLine(posKing, piece.pos, out _, true);
                        return true;
                    }
                    return false;
                }
            }
            if (piece.piece == Piece.Queen || piece.piece == Piece.Bishop)
            {
                if (piece.pos.Diagonal(posKing))
                {
                    if (Move.NothingInTheWay(posKing, piece.pos, this))
                    {
                        bitboard = Bitboards.MaskLine(posKing, piece.pos, out _, true);
                        return true;
                    }
                    return false;
                }
            }
            else if (piece.piece == Piece.Pawn)
            {
                int modifier = 1;
                if (!piece.isWhite)
                    modifier = -1;
                if (piece.pos + 9 * modifier == posKing || piece.pos + 7 * modifier == posKing)
                {
                    bitboard = Bitboards.MaskLine(posKing, piece.pos, out _, true);
                    return true;
                }
            }
            else if (piece.piece == Piece.Knight)
            {
                List<int> moves = Knight.Moves(piece, this);
                foreach (int knightMove in moves)
                {
                    if (knightMove == posKing)
                    {
                        bitboard = (ulong)1 << posKing;
                        bitboard |= (ulong)1 << piece.pos;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool KingThreat(int posKing, List<int> Pieces, out ulong bitboard)
        {
            bitboard = 0;

            foreach (int i in Pieces)
            {
                if (i.VerticalTo(posKing) && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                    {
                        bitboard = Bitboards.MaskLine(posKing, i, out _, true);
                        return true;
                    }
                }
                else if (i.HorizontalTo(posKing) && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                    {
                        bitboard = Bitboards.MaskLine(posKing, i, out _, true);
                        return true;
                    }
                }
                else if (i.Diagonal(posKing) && (Board[i].piece == Piece.Bishop || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                    {
                        bitboard = Bitboards.MaskLine(posKing, i, out _, true);
                        return true;
                    }
                }
                else if (Board[i].piece == Piece.Knight)
                {
                    foreach (int moveN in Knight.Moves(Board[i], this))
                        if (moveN == posKing)
                        {
                            bitboard = (ulong)1 << posKing;
                            bitboard |= (ulong)1 << i;
                            return true;
                        }
                }
                else if (Board[i].piece == Piece.Pawn)
                {
                    foreach (int moveP in Pawn.DiagonalMoves(Board[i], this, -1))
                        if (moveP == posKing)
                        {
                            bitboard = Bitboards.MaskLine(posKing, i, out _, true);
                            return true;
                        }
                }
            }
            return false;
        }
    }
}