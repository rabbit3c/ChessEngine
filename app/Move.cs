using System.Security.Cryptography.X509Certificates;

namespace ChessEngine {
    class Move {
        public static bool Inbound((int x, int y) pos) {
            if (pos.x >= 1 && pos.x <= 8 && pos.y >= 1 && pos.y <= 8) 
                return true;
            return false;
        }

        public static bool Unprotected((int x, int y) pos) {
            // needs to be implemented
            return true;
        }
    }
}