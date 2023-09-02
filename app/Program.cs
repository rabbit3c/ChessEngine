
namespace ChessEngine
{
    class Program {
        static void Main() {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            FEM pos = new();
            pos.FormatPosition(position);
            List<Position> positions= new() {pos};
            Search.Main(positions, 3);
        }
    }
}