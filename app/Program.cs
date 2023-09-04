
namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string positionFEN = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -";
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN);
            Console.WriteLine(pos.FormatFEN());
            List<Position> positions = new() { pos };
            Search.Main(positions, 3, out int _);
        }
    }
}