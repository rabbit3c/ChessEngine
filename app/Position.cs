namespace ChessEngine {
    public class Position {
        public List<string> PiecesWhite {get;set;} = new();
        public List<string> PiecesBlack {get;set;} = new();
        public bool WhitesTurn;

        public Position(bool whitesTurn)
        {
            WhitesTurn = whitesTurn;
        }

        public List<string> OwnPieces() {
            return WhitesTurn ? PiecesWhite : PiecesBlack;
        }
    }
}