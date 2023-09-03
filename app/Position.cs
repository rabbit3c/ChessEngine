
namespace ChessEngine
{
    public class Position
    {

        public List<Piece> Pieces { get; set; } = new();
        public List<Piece> PiecesWhite { get; set; } = new();
        public List<Piece> PiecesBlack { get; set; } = new();

        public (int x, int y) EnPassantTarget { get; set; } = new();

        public bool WhitesTurn = true;

        public bool WShortCastle = true;
        public bool WLongCastle = true;
        public bool BShortCastle = true;
        public bool BLongCastle = true;

        public List<Piece> OwnPieces()
        {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }

        public List<Piece> EnemyPieces()
        {
            return WhitesTurn ? PiecesBlack : PiecesWhite;
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

            foreach (Piece piece in Pieces)
            {
                if (piece.isWhite)
                    PiecesWhite.Add(piece);
                else
                    PiecesBlack.Add(piece);
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
            if (piece.isWhite)
                WShortCastle = false;
            else
                BShortCastle = false;

        }

        public void NoShortCastle(Piece piece)
        {
            if (piece.isWhite)
                WShortCastle = false;
            else
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

        public static (int, int) GetClone(this (int, int) source)
        {
            return (source.Item1, source.Item2);
        }
    }
}