
namespace ChessEngine
{
    class Search
    {
        public static void Main(Position position, int depth, out int AmountPos)
        {
            AmountPos = 0;
            depth--;

            if (depth == 0)
            {
                GeneratePositions(position, out int n, true);
                AmountPos = n;
                return;
            }

            List<Position> newPositions = GeneratePositions(position, out int _);

            foreach (Position newPosition in newPositions)
            {
                int AmountNewPos = 0;

                if (Transpositions.lookupTable.ContainsKey(newPosition.hash))
                { // checking if position is in look up table
                    TranspositionInfo transposInfo = Transpositions.lookupTable[newPosition.hash];
                    if (transposInfo.depth == depth)
                    { //checking if transposition is at right depth
                        AmountNewPos = transposInfo.resultingPositions;
                    }
                    else
                    {
                        Main(newPosition, depth, out AmountNewPos);
                    }
                }
                else
                { //if not generate resulting positions and add to Transposition table
                    Main(newPosition, depth, out AmountNewPos);
                    Transpositions.Add(newPosition, AmountNewPos, depth, 0);
                }
                AmountPos += AmountNewPos;
            }
        }
        public static List<Position> GeneratePositions(Position pos, out int newPos, bool lastDepth = false)
        {
            Square[] Pieces = pos.Board;
            List<Position> newPositions = new();

            if (!pos.doubleCheck)
            {
                newPositions.AddRange(ParallelGeneration(pos, lastDepth));
            }
            else //if there is a double check, the King has to move
            {
                int posKing = pos.OwnKing();
                List<int> moves = King.LegalMoves(Pieces[posKing], pos);
                newPositions.AddRange(NewPos.New(pos, Pieces[posKing], moves, lastDepth));
            }
            newPos = newPositions.Count;
            return newPositions;
        }

        public static List<Position> ParallelGeneration(Position pos, bool lastDepth)
        {
            var syncRoot = new object();
            List<Position> newPositions = new();
            List<int> ownPieces = pos.OwnPieces();

            //Multithreading
            Parallel.For(0, ownPieces.Count, i =>
            {
                List<Position> positions = GenerateMoves(pos, ownPieces[i], lastDepth);
                lock (syncRoot)
                {
                    newPositions.AddRange(positions);
                }
            });

            //Single Thread
            /*for( int i = 0; i < ownPieces.Count; i ++)
            {
                List<Position> positions = GenerateMoves(pos, ownPieces[i]);
                newPositions.AddRange(positions);
            }*/
            
            return newPositions;
        }

        public static List<Position> GenerateMoves(Position pos, int i, bool lastDepth)
        {

            Square[] Pieces = pos.Board;

            List<int> moves = Pieces[i].piece switch
            {
                Piece.Queen or Piece.Rook or Piece.Bishop => Pieces[i].LegalMoves(pos),
                Piece.King => King.LegalMoves(Pieces[i], pos),
                Piece.Knight => Knight.LegalMoves(Pieces[i], pos),
                _ => Pawn.LegalMoves(Pieces[i], pos),
            };
            return NewPos.New(pos, Pieces[i], moves, lastDepth);
        }
    }
}