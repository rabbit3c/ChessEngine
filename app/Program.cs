
namespace ChessEngine
{
    class Program {
        static void Main(string[] args) {
            string[] position = new string[] {"wQ23", "wK11", "w12", "bR32", "bK88"};
            Search.Main(position, whitesTurn: true);
        }
    }
}