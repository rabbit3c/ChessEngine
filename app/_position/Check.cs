namespace ChessEngine
{

    public partial class Position
    {
        public bool Illegal() //Can the Enemy King be taken?
        {
            return KingThreat(EnemyKing(), OwnPieces());
        }

        public bool Check()
        {
            return KingThreat(OwnKing(), EnemyPieces());
        }

        public bool KingThreat(int posKing, List<int> Pieces)
        {
            foreach (int i in Pieces)
            {
                if (i.X() == posKing.X() && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                        return true;
                }
                else if (i.Y() == posKing.Y() && (Board[i].piece == Piece.Rook || Board[i].piece == Piece.Queen))
                {
                    if (Move.NothingInTheWay(posKing, i, this))
                    {
                        return true;
                    }
                }
                else if (Math.Abs(i.X() - posKing.X()) == Math.Abs(i.Y() - posKing.Y()) && (Board[i].piece == Piece.Bishop || Board[i].piece == Piece.Queen))
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
                    foreach (int moveP in Pawn.Moves(Board[i], this))
                        if (moveP == posKing)
                            return true;
                }
            }
            return false;
        }
    }
}