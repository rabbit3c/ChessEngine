namespace ChessEngine {
    
    class Knight {

        public static List<(int, int)> LegalMoves((int x, int y) posKnight, Position pos) {
            List<(int, int)> legalMoves = new();
            
            for (int x = -2; x <= 2; x+=4) {
                for (int y = -1; y <= 1; y+=2) {
                    (int, int) move = (posKnight.x + x, posKnight.y + y);
                    if (Legal(move, pos))
                        legalMoves.Add(move);
                }
            }
            for (int y = -2; y <= 2; y+=4) {
                for (int x = -1; x <= 1; x+=2) {
                    (int, int) move = (posKnight.x + x, posKnight.y + y);
                    if (Legal(move, pos))
                        legalMoves.Add(move);
                }
            }
            
            string combinedString = string.Join(", ", legalMoves);
            Console.WriteLine($"Knight at {posKnight} to {combinedString}");
            return legalMoves;
        }

        public static bool Legal((int, int) move, Position pos) {
            if (Move.Unobstructed(move, pos.OwnPieces()) && Move.NotInCheck(move, pos) && Move.Inbound(move)) 
                return true;
            return false;
        }
    }
}