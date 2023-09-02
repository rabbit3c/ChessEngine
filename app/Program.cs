
namespace ChessEngine
{
    class Program {
        static void Main(string[] args) {
            string[] position = new string[] {$"w{Piece.Queen}33", $"w{Piece.King}51", $"w{Piece.Pawn}12", $"b{Piece.Pawn}13", $"w{Piece.Rook}81", $"b{Piece.King}88", $"w{Piece.Bishop}23", $"w{Piece.Knight}75", $"w{Piece.Rook}11"};
            Position pos = new(whitesTurn: true, bLongCastle: false, bShortCastle: false);
            pos.FormatPosition(position);
            Search.Main(pos);
        }
    }
}