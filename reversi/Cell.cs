using System;
namespace reversi
{
    public class Cell
    {
        public int state;

        public Cell()
        {
            this.state = 0;
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
