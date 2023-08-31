
namespace ChessEngine
{
    class Program {
        static void Main(string[] args) {
            string[] position = new string[] {"wQ33", "wK11", "w12", "b13", "wR32", "bK88", "wB23"};
            Position pos = new(whitesTurn: true);
            pos.FormatPosition(position);
            Search.Main(pos);
        }
    }
}