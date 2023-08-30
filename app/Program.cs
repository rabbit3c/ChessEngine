
namespace ChessEngine
{
    class Program {
        static void Main(string[] args) {
            string[] position = new string[] {"wQ23", "wK11", "bR32", "bK88"};
            Search.Main(position, white: true);
        }
    }
}