
using System.Diagnostics;

namespace ChessEngine
{
    public class Program
    {
        static void Main()
        {
            string positionFEN = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -";
            PerftMain(positionFEN, 5, out int _);
        }

        public static void PerftMain(string positionFEN, int depth, out int amountPos)
        {
            Transpositions.lookupTable.Clear();
            PrecomputedData.Precompute(); //Generating Hashes for Transpositions
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(positionFEN); //Creating Position
            
            if (pos.Illegal())
            {
                Console.WriteLine("This Position is illegal!");
                amountPos = 0;
                return;
            }

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