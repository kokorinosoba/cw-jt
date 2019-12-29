using System;
namespace reversi
{
    public class Cell
    {
        public int state;
        public int row;
        public int col;

        public Cell(int row, int col)
        {
            this.state = 0;
            this.row = row;
            this.col = col;
        }

        public char getMark()
        {
            return "o-x"[this.state + 1];
        }

        public void put(int playerNumber)
        {
            this.state = playerNumber;
        }

        public void flip()
        {
            this.state *= -1;
        }
    }
}
