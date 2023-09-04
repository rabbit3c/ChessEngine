
namespace ChessEngine
{
    class Program
    {
        static void Main()
        {
            string position = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -";
            /*List<string> positionsFEN = new() {
                "1r2k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w k -",
                "2r1k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w k -",
                "3rk2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w k -",
                "4k2r/rppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w k -",
                "r2k3r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w - -",
                "2kr3r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w - -",
                "r3k1r1/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w q -",
                "r3kr2/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w q -",
                "r3k2r/Pp1p1ppp/1bp2nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Ppp2ppp/1b1p1nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Ppp2ppp/1b3nbN/nPBp4/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq d6",
                "r3k2r/Pppp1p1p/1b3nbp/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/5nbN/nPb5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/bppp1ppp/5nbN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b4bN/nPBn4/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b4bN/nPB4n/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b4bN/nPB5/B1P1n3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b4bN/nPB5/B1P1P1n1/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k1nr/Pppp1ppp/1b4bN/nPB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3n1N/nPB4b/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3n1N/nPB2b2/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3n1N/nPB5/B1P1b3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/1PB5/B1n1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1bn2nbN/1PB5/B1P1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/1PB5/B1P1P3/qn3N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/1q3N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/2q2N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/3q1N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/4qN2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/5q2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/q1P1P3/5N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/5N2/qp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/BqP1P3/5N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPq5/B1P1P3/5N2/Pp1P2PP/R2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/Rq1Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/Rn1Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/Rb1Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/Rr1Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/q2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/n2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/b2Q1RK1 w kq -",
                "r3k2r/Pppp1ppp/1b3nbN/nPB5/B1P1P3/q4N2/P2P2PP/r2Q1RK1 w kq -"
            };*/
            //foreach (string position in positionsFEN) {
            Console.WriteLine("Starting...");
            FEN pos = new();
            pos.FormatPosition(position);
            Console.WriteLine(pos.FormatFEN());
            List<Position> positions = new() { pos };
            Search.Main(positions, 3);
            //}
        }
    }
}