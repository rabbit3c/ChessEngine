
using System.Diagnostics;

namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string positionFEN = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -";
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN); //Creating Position
            ZobristHashes.GenerateHashes(); //Generating Hashes for Transpositions
            Console.WriteLine(pos.FormatFEN());

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Search.Main(pos, 5, out int amount); //Search all moves, in given depth
            stopwatch.Stop();

            Console.WriteLine($"{amount}   -   time: {stopwatch.Elapsed}ms");
        }
    }
}