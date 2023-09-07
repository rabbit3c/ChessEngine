
namespace ChessEngine
{
    public class Search
    {
        public static void Main(Position position, int depth, out int AmountPos)
        {
            AmountPos = 0;
            depth--;

            if (depth == 0) {
                GeneratePositions(position, out int n);
                AmountPos = n;
                return;
            }

            List<Position> newPositions = GeneratePositions(position, out int _);

            foreach (Position newPosition in newPositions)
            {
                /*if (depth == 2) {
                    Console.WriteLine(FEN.FormatFEN(newPosition));
                }*/
                int AmountNewPos = 0;
                ulong hash = newPosition.Hash();
                if (Transpositions.lookupTable.ContainsKey(hash)) { // checking if position is in look up table
                    TranspositionInfo transposInfo = Transpositions.lookupTable[hash];
                    if (transposInfo.depth == depth) { //checking if transposition is at right depth
                        AmountNewPos = transposInfo.resultingPositions;
                    }
                    else {
                        Main(newPosition, depth, out AmountNewPos);
                    }
                }
                else { //if not generate resulting positions and add to Transposition table
                    Main(newPosition, depth, out AmountNewPos);
                    Transpositions.Add(newPosition, AmountNewPos, depth, 0);
                }
                /*if (depth == 2) {
                    Console.WriteLine(AmountNewPos);
                }*/
                AmountPos += AmountNewPos;
            }
        }
        public static List<Position> GeneratePositions(Position pos, out int newPos)
        {
            newPos = 0;
            List<Square> Pieces = pos.Board;
            List<Position> newPositions = new();

            foreach (int i in pos.OwnPieces())
            {
                List<(int, int)> moves = Pieces[i].piece switch
                {
                    Piece.King => King.LegalMoves(Pieces[i], pos),
                    Piece.Queen => Queen.LegalMoves(Pieces[i], pos),
                    Piece.Bishop => Bishop.LegalMoves(Pieces[i], pos),
                    Piece.Rook => Rook.LegalMoves(Pieces[i], pos),
                    Piece.Knight => Knight.LegalMoves(Pieces[i], pos),
                    _ => Pawn.LegalMoves(Pieces[i], pos),
                };
                newPositions.AddRange(NewPos.New(pos, Pieces[i], moves, out int n));
                //Console.WriteLine($"{Pieces[i].piece}, {n}");
                newPos += n;
            }
            return newPositions;
        }
    }
}