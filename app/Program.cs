
using System.Diagnostics;

namespace ChessEngine
{
    public class Program
    {
        static void Main()
        {
            string positionFEN = "7k/8/8/7q/8/8/7B/7K w - - 0 1";
            PerftMain(positionFEN, 1, out int _);
        }

        public static void PerftMain(string positionFEN, int depth, out int amountPos) {
            Transpositions.lookupTable.Clear();
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN); //Creating Position
            PrecomputedData.Precompute(); //Generating Hashes for Transpositions
            Console.WriteLine(FEN.FormatFEN(pos));
            pos.Hash();

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Search.Main(pos, depth, out int amount); //Search all moves, in given depth
            stopwatch.Stop();

            amountPos = amount;
            Console.WriteLine($"{amount}   -   time: {stopwatch.Elapsed}");
        }
    }
}