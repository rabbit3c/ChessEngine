
using System.Runtime.CompilerServices;

namespace ChessEngine {

    public class Piece{
        public (int x, int y) pos;
        public bool isWhite;

        public int piece;
        public const int Pawn = 0;
        public const int Knight = 1;
        public const int Bishop = 2;
        public const int Rook = 3;
        public const int Queen = 4;
        public const int King = 5;

        public Piece((int, int) posPiece = new(), bool whitePiece = true, int pieceType = 0)
        {
            pos = posPiece;
            piece = pieceType;
            isWhite = whitePiece;
        }

        public object Copy()
        {
            Piece copy = new(pos, isWhite, piece);
            return copy;
        }

        public bool IsPinned((int x, int y) move, Position position) {
            if (piece != Piece.King)
            {
                (int x, int y) posKing = (0, 0);
                foreach (Piece p in position.OwnPieces())
                {
                    if (p.piece == Piece.King)
                    {
                        posKing = p.pos;
                        break;
                    }
                }

                if (pos.x == posKing.x && move.x != posKing.x)
                {
                    foreach (Piece p in position.EnemyPieces())
                        if (p.piece == Piece.Queen || p.piece == Piece.Rook)
                            if (p.pos.x == posKing.x)
                                if (Move.NothingInTheWay(posKing, p.pos, position))
                                    return false;
                }

                else if (pos.y == posKing.y && move.y != posKing.y)
                {
                    foreach (Piece p in position.EnemyPieces())
                        if (p.piece == Piece.Queen || p.piece == Piece.Rook)
                            if (p.pos.y == posKing.y)
                                if (Move.NothingInTheWay(posKing, p.pos, position))
                                    return false;
                }


                else
                    for (int i = -6; i <= 6; i++)
                        for (int x = -1; x <= 1; x += 2)
                            for (int y = -1; y <= 1; y += 2)
                                if (pos.y + i * y == posKing.y && pos.x + i * x == posKing.x)
                                    if (move.y + i * y != posKing.y && move.x + i * x != posKing.x)
                                        foreach (Piece p in position.EnemyPieces())
                                            if (p.piece == Queen || p.piece == Bishop)
                                                if (p.Diagonal(posKing))
                                                    if (Move.NothingInTheWay(posKing, p.pos, position))
                                                        return false;
            }
            return true;
        }

        public bool Diagonal((int x, int y) pos2) {
            for (int i = -6; i <= 6; i++)
                            for (int x = -1; x <= 1; x += 2)
                                for (int y = -1; y <= 1; y += 2)
                                    if (pos.y + i * y == pos2.y && pos.x + i * x == pos2.x) 
                                        return true;
            return false;
        }
    }
}