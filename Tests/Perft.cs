namespace Tests;

public class Perft
{
    private readonly Search _search;
    public Perft()
    {
        _search = new Search();
    }

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 4, 197281)]
    [InlineData("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -", 3, 97862)]
    [InlineData("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -", 5, 674624)]
    [InlineData("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", 4, 422333)]
    [InlineData("r2q1rk1/pP1p2pp/Q4n2/bbp1p3/Np6/1B3NBn/pPPP1PPP/R3K2R b KQ - 0 1", 4, 422333)]
    [InlineData("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8", 3, 62379)]
    [InlineData("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10", 3, 89890)]
    public void Test(string positionFEN, int depth, int expectedAmountPos)
    {
        Console.WriteLine("Starting...");
        FEN pos = new();
        pos.FormatPosition(positionFEN);
        Console.WriteLine(pos.FormatFEN());
        List<Position> positions = new() { pos };
        Search.Main(positions, depth, out int amountPos);
        Assert.True(expectedAmountPos == amountPos, $"Perft({depth}): {amountPos} - Expected: {expectedAmountPos})");
    }
}