using System;
using System.Collections.Generic;

namespace reversi
{
    public class Board
    {
        // 二次元配列のCellを持ったcells
        List<List<Cell>> cells = new List<List<Cell>>();

        public Board()
        {
            for (int row = 0; row < 8; row++)
            {
                this.cells.Add(new List<Cell>());
                for (int col = 0; col < 8; col++)
                {
                    this.cells[row].Add(new Cell());
                }
            }
            this.cells[3][3].state = this.cells[4][4].state = -1;
            this.cells[3][4].state = this.cells[4][3].state = 1;
        }

        public void show()
        {
            // ノーテーション用のアルファベットを表示
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(" {0}", (char)('a' + i));
            }
            Console.WriteLine();

            // 盤面を表示
            for (int row = 0; row < 8; row++)
            {
                Console.Write(" {0}", row + 1);
                for (int col = 0; col < 8; col++)
                {
                    Console.Write(" {0}", this.cells[row][col].getMark());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
