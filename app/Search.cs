
using System.Diagnostics;

namespace ChessEngine
{
    class Search
    {
        public static void Main(Position position, int depth, out int amountPos)
        {
            Generation(position, depth, out amountPos);
        }
        public static void Generation(Position pos, int depth, out int amountPos)
        {
            if (!pos.doubleCheck)
            {
                if (depth == 1)
                {
                    ParallelGeneration(pos, depth, out amountPos);
                }
                else
                {
                    SequentialGeneration(pos, depth, out amountPos);
                }
            }
            else //if there is a double check, the King has to move
            {
                int posKing = pos.OwnKing();
                GeneratePositions(pos, posKing, depth, out amountPos);
            }
        }

        public static void SequentialGeneration(Position pos, int depth, out int amountPos)
        {
            List<int> ownPieces = pos.OwnPieces();

            amountPos = 0;

            for (int i = 0; i < ownPieces.Count; i++)
            {
                GeneratePositions(pos, ownPieces[i], depth, out int amountNewPos);
                amountPos += amountNewPos;
            }
        }

        public static void ParallelGeneration(Position pos, int depth, out int amountPos)
        {
            if (Debugger.IsAttached) //Single Threading for Debug Mode
            {
                SequentialGeneration(pos, depth, out amountPos);
            }
            else //Multithreading
            {
                var syncRoot = new object();
                List<int> ownPieces = pos.OwnPieces();
                int amountNewPos = 0;

                Parallel.For(0, ownPieces.Count, i =>
                {
                    GeneratePositions(pos, ownPieces[i], depth, out int newPos);
                    lock (syncRoot)
                    {
                        amountNewPos += newPos;
                    }
                });
                amountPos = amountNewPos;
            }
        }

        public static void GeneratePositions(Position pos, int i, int depth, out int amountPos)
        {
            depth--;

            List<int> moves = GenerateMoves(pos, i);
            List<Position> newPositions = NewPos.New(pos, pos.Board[i], moves, depth == 0);

            if (depth == 0)
            {
                amountPos = newPositions.Count;
                return;
            }

            amountPos = 0;

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
                        Generation(newPosition, depth, out AmountNewPos);
                    }
                }
                else
                { //if not generate resulting positions and add to Transposition table
                    Generation(newPosition, depth, out AmountNewPos);
                    Transpositions.Add(newPosition, AmountNewPos, depth, 0);
                }
                amountPos += AmountNewPos;
            }

            return;
        }

        public static List<int> GenerateMoves(Position pos, int i)
        {

            Square[] Pieces = pos.Board;

            List<int> moves = Pieces[i].piece switch
            {
                Piece.Queen or Piece.Rook or Piece.Bishop => Pieces[i].LegalMoves(pos),
                Piece.King => King.LegalMoves(Pieces[i], pos),
                Piece.Knight => Knight.LegalMoves(Pieces[i], pos),
                _ => Pawn.LegalMoves(Pieces[i], pos),
            };
            return moves;
        }
    }
}