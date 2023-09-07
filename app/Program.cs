
using System.Diagnostics;

namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string positionFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN); //Creating Position
            PrecomputedData.Precompute(); //Generating Hashes for Transpositions
            Console.WriteLine(FEN.FormatFEN(pos));

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Search.Main(pos, 4, out int amount); //Search all moves, in given depth
            stopwatch.Stop();

            Console.WriteLine($"{amount}   -   time: {stopwatch.Elapsed}");
        }
    }
}