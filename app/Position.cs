
namespace ChessEngine
{
    public class Position
    {

        public List<Piece> Pieces { get; set; } = new();
        List<int> PiecesWhite { get; set; } = new();
        List<int> PiecesBlack { get; set; } = new();

        public (int x, int y) EnPassantTarget { get; set; } = new();

        public bool WhitesTurn = true;

        public bool WShortCastle = true;
        public bool WLongCastle = true;
        public bool BShortCastle = true;
        public bool BLongCastle = true;

        public List<Piece> OwnPieces()
        {
            List<Piece> ownPieces = new();
            if (WhitesTurn) {
                foreach (int i in PiecesWhite) {
                    ownPieces.Add(Pieces[i]);
                }
            }
            else {
                foreach (int i in PiecesBlack) {
                    ownPieces.Add(Pieces[i]);
                }
            }
            return ownPieces;
        }

        public List<Piece> EnemyPieces()
        {
            List<Piece> enemyPieces = new();
            if (WhitesTurn) {
                foreach (int i in PiecesBlack) {
                    enemyPieces.Add(Pieces[i]);
                }
            }
            else {
                foreach (int i in PiecesWhite) {
                    enemyPieces.Add(Pieces[i]);
                }
            }
            return enemyPieces;
        }

        public object Copy()
        {
            Position copy = new()
            {
                Pieces = Pieces.GetClone(),
                PiecesBlack = PiecesBlack.GetClone(),
                PiecesWhite = PiecesWhite.GetClone(),
                EnPassantTarget = EnPassantTarget.GetClone(),
                WhitesTurn = WhitesTurn,
                WLongCastle = WLongCastle,
                WShortCastle = WShortCastle,
                BLongCastle = BLongCastle,
                BShortCastle = BShortCastle
            };
            return copy;
        }

        public object Convert()
        {
            FEN copy = new()
            {
                Pieces = Pieces.GetClone(),
                PiecesBlack = PiecesBlack.GetClone(),
                PiecesWhite = PiecesWhite.GetClone(),
                EnPassantTarget = EnPassantTarget.GetClone(),
                WhitesTurn = WhitesTurn,
                WLongCastle = WLongCastle,
                WShortCastle = WShortCastle,
                BLongCastle = BLongCastle,
                BShortCastle = BShortCastle
            };
            return copy;
        }

        public void RemoveAt(int i)
        {
            Pieces.RemoveAt(i);
        }

        public void SplitColors()
        {

            PiecesWhite.Clear();
            PiecesBlack.Clear();

            for (int i = 0; i < Pieces.Count; i++) 
            {
                if (Pieces[i].isWhite)
                    PiecesWhite.Add(i);
                else
                    PiecesBlack.Add(i);
            }
        }

        public void ToggleTurn()
        {
            WhitesTurn = !WhitesTurn;
        }

        public void Add(Piece piece)
        {
            Pieces.Add(piece);
        }

        public void NoLongCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.y == 1)
                WLongCastle = false;
            else if (piece.pos.y == 8)
                BLongCastle = false;

        }

        public void NoShortCastle(Piece piece)
        {
            if (piece.isWhite && piece.pos.y == 1)
                WShortCastle = false;
            else if (piece.pos.y == 8)
                BShortCastle = false;
        }

        public void NoCastle(Piece piece)
        {
            if (piece.piece == Piece.King)
            {
                NoShortCastle(piece);
                NoLongCastle(piece);
            }
            else if (piece.piece == Piece.Rook)
            {
                if (piece.pos.x == 8)
                    NoShortCastle(piece);
                else if (piece.pos.x == 1)
                    NoLongCastle(piece);
            }
        }
    }

    public static class Extensions
    {
        public static List<Piece> GetClone(this List<Piece> source)
        {
            return source.Select(item => (Piece)item.Copy())
                    .ToList();
        }

        public static List<int> GetClone(this List<int> source)
        {
            return source.ToList();
        }

        public static (int, int) GetClone(this (int, int) source)
        {
            return (source.Item1, source.Item2);
        }
    }
}