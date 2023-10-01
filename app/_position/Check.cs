
namespace ChessEngine
{

    public partial class Position
    {
        public bool check;
        public bool doubleCheck;
        public bool Illegal() //Can the Enemy King be taken?
        {
            return KingThreat(EnemyKing(), OwnPieces());
        }

        public bool Check(int position)
        {
            return KingThreat(position, EnemyPieces());
        }

        public bool Check(Piece piece)
        {
            int posKing = OwnKing();
            if (piece.piece == Piece.Queen || piece.piece == Piece.Rook)
            {
                if (piece.pos.VerticalTo(posKing) || piece.pos.HorizontalTo(posKing))
                {
                    return Move.NothingInTheWay(posKing, piece.pos, this);
                }
            }
            if (piece.piece == Piece.Queen || piece.piece == Piece.Bishop)
            {
                if (piece.pos.Diagonal(posKing))
                {
                    return Move.NothingInTheWay(posKing, piece.pos, this);
                }
            }
            else if (piece.piece == Piece.Pawn)
            {
                int modifier = 1;
                if (!piece.isWhite)
                    modifier = -1;
                if (piece.pos + 9 * modifier == posKing || piece.pos + 7 * modifier == posKing)
                {
                    return true;
                }
            }
            else if (piece.piece == Piece.Knight)
            {
                List<int> moves = Knight.Moves(piece, this);
                foreach (int knightMove in moves)
                {
                    if (knightMove == posKing) return true;
                }
            }
            return false;
        }

        public bool KingThreat(int posKing, List<int> Pieces)
        {
            foreach (int i in Pieces)
            {
                if (i.VerticalTo(posKing) && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                        return true;
                }
                else if (i.HorizontalTo(posKing) && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                    {
                        return true;
                    }
                }
                else if (i.Diagonal(posKing) && (Board[i].piece == Piece.Bishop || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                        return true;
                }
                else if (Board[i].piece == Piece.Knight)
                {
                    foreach (int moveN in Knight.Moves(Board[i], this))
                        if (moveN == posKing)
                            return true;
                }
                else if (Board[i].piece == Piece.Pawn)
                {
                    foreach (int moveP in Pawn.DiagonalMoves(Board[i], this, new[]{true, true, true, true}))
                        if (moveP == posKing)
                            return true;
                }
            }
            return false;
        }
    }
}