
namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string position = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10";
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(position);
            Console.WriteLine(pos.FormatFEN());
            List<Position> positions = new() { pos };
            Search.Main(positions, 4);
        }
    }
}