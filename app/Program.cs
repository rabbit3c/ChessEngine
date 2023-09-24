
using System.Diagnostics;

namespace ChessEngine
{
    public class Program
    {
        static void Main()
        {
            string positionFEN = "r2q1rk1/pP1p2pp/Q4n2/bbp1p3/Np6/1B3NBn/pPPP1PPP/R3K2R b KQ - 0";
            PerftMain(positionFEN, 4, out int _);
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