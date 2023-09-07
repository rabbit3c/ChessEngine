
using System.Diagnostics;

namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string positionFEN = "r2q1rk1/pP1p2pp/Q4n2/bbp1p3/Np6/1B3NBn/pPPP1PPP/R3K2R b KQ - 0 1 ";
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN); //Creating Position
            ZobristHashes.GenerateHashes(); //Generating Hashes for Transpositions
            Console.WriteLine(FEN.FormatFEN(pos));

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Search.Main(pos, 4, out int amount); //Search all moves, in given depth
            stopwatch.Stop();

            Console.WriteLine($"{amount}   -   time: {stopwatch.Elapsed}");
        }
    }
}