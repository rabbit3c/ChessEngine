
namespace ChessEngine
{
    public partial class Position
    {
        public ulong occupiedBB = 0;
        public ulong emptyBB = 0;

        public void InitializeBitBoards()
        {
            occupiedBB = 0;
            emptyBB = 0;
            for (int i = 0; i < 64; i++)
            {
                if (!Board[i].empty)
                {
                    occupiedBB |= (ulong)1 << i;
                }
            }
            emptyBB = ~occupiedBB;
        }

        public ulong OccupiedBB()
        {
            ulong occupiedBBCopy = 0;
            for (int i = 0; i < 64; i++)
            {
                if (!Board[i].empty)
                {
                    occupiedBBCopy |= (ulong)1 << i;
                }
            }
            return occupiedBBCopy;
        }

        public void UpdateBitBoard(int pos, int move)
        {
            occupiedBB ^= (ulong)1 << pos;
            emptyBB |= (ulong)1 << pos;
            occupiedBB |= (ulong)1 << move;
            emptyBB ^= (ulong)1 << move;
        }

        public void EnPassantBB(bool isWhite)
        {
            int modifier = isWhite ? -8 : 8;
            occupiedBB ^= (ulong)1 << (EnPassantTarget + modifier);
            emptyBB |= (ulong)1 << (EnPassantTarget + modifier);
        }
    }

    public class Bitboards
    {
        public const ulong file = 0x01_01_01_01_01_01_01_01;
        public const ulong rank = 0x00_00_00_00_00_00_00_FF;
        public const ulong diagonalP = 0x80_40_20_10_08_04_02_01;
        public const ulong diagonalS = 0x01_02_04_08_10_20_40_80;

        public static ulong MaskLine(int pos1, int pos2, out bool NotInLine)
        { //Line doesn't include pos1 and pos 2
            NotInLine = false;
            ulong mask = (ulong)Math.Pow(2, Math.Max(pos1, pos2)) - (ulong)Math.Pow(2, Math.Min(pos2, pos1) + 1); //mask of every square between two indices
            //Console.WriteLine(mask);
            if (pos1.X() == pos2.X())
            {
                mask &= file << pos1.X();
            }
            else if (pos1.Y() == pos2.Y())
            {
                mask &= rank << pos1.Y() * 8;
            }
            else if (Math.Abs(pos1.X() - pos2.X()) == Math.Abs(pos1.Y() - pos2.Y()))
            {
                Math.DivRem(pos1 - pos2, 9, out int rem9); //if the position are on the primary diagonal, their diffrence must be a multiple of nine
                int x = pos1.X();
                int y = pos1.Y();
                if (rem9 == 0)
                {
                    if (x >= y)
                        mask &= diagonalP >> (x - y) * 8;
                    else
                        mask &= diagonalP << (y - x) * 8;
                }
                else
                {
                    int z = 7 - x;
                    if (z >= y)
                    {
                        mask &= diagonalS >> (z - y) * 8;
                    }
                    else
                    {
                        mask &= diagonalS << (y - z) * 8;
                    }
                }
            }
            else
            {
                NotInLine = true;
            }
            return mask;
        }
    }
}