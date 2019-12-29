using System;
namespace reversi
{
    public class Cell
    {
        public int State;
        public int Row;
        public int Col;

        public Cell(int row, int col)
        {
            this.State = 0;
            this.Row = row;
            this.Col = col;
        }

        public char GetMark()
        {
            return "o-x"[this.State + 1];
        }

        public void Put(int player)
        {
            this.State = player;
        }

        public void Flip()
        {
            this.State *= -1;
        }

        public bool CanPut()
        {
            return this.State == 0;
        }
    }
}
