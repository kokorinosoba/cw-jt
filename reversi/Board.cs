using System;
using System.Collections.Generic;

namespace reversi
{
    public class Board
    {
        // 二次元配列のCellを持ったCells
        List<List<Cell>> Cells = new List<List<Cell>>();

        public Board()
        {
            for (int row = 0; row < 8; row++)
            {
                this.Cells.Add(new List<Cell>());
                for (int col = 0; col < 8; col++)
                {
                    this.Cells[row].Add(new Cell(row, col));
                }
            }

            // 初期位置にディスクをセット
            this.GetCell(3, 3).State = this.GetCell(4, 4).State = -1;
            this.GetCell(3, 4).State = this.GetCell(4, 3).State = 1;
        }

        public void Show()
        {
            // ノーテーション用のアルファベットを表示
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                // a b c d e ... h と順番に表示する
                Console.Write(" {0}", (char)('a' + i));
            }
            Console.WriteLine();

            // 盤面を表示
            for (int row = 0; row < 8; row++)
            {
                Console.Write(" {0}", row + 1);
                for (int col = 0; col < 8; col++)
                {
                    Console.Write(" {0}", this.GetCell(row, col).GetMark());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // 指定した位置のCellを返す関数
        // ただし範囲外の場合はnullを返す
        public Cell GetCell(int row, int col)
        {
            if (row < 0 || 7 < row || col < 0 || 7 < col)
            {
                return null;
            }
            return this.Cells[row][col];
        }

        // 盤全体で裏返せる石の数を返す
        public int CountFlippableDisks(int player)
        {
            int nFlippableDisks = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Cell baseCell = this.GetCell(row, col);

                    if (baseCell.CanPut())
                    {
                        nFlippableDisks += this.EightWayScanning(player, baseCell);
                    }
                }
            }
            return nFlippableDisks;
        }

        // ８方向に走査し、裏返せる石の数を返す
        public int EightWayScanning(int player, Cell baseCell, bool flip = false)
        {
            int nFlippableDisks = 0;

            int[] directionRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] directionCols = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int directionRow = directionRows[i];
                int directionCol = directionCols[i];

                int oneWayFlippableDisks = this.OneWayScanning(player, baseCell, directionRow, directionCol);
                nFlippableDisks += oneWayFlippableDisks;

                if (flip && oneWayFlippableDisks > 0)
                {
                    this.OneWayScanning(player, baseCell, directionRow, directionCol, flip);
                }
            }

            return nFlippableDisks;
        }

        // １方向に走査し、裏返せる石の数を返す
        public int OneWayScanning(int player, Cell baseCell, int directionRow, int directionCol, bool flip = false)
        {
            int nEnemyDisks = 0;
            int row, col;

            for (row = baseCell.Row + directionRow, col = baseCell.Col + directionCol; this.GetCell(row, col) != null && this.GetCell(row, col).State == -player; row += directionRow, col += directionCol)
            {
                if (flip)
                {
                    this.GetCell(row, col).Flip();
                }
                nEnemyDisks++;
            }            

            if (this.GetCell(row, col) != null && this.GetCell(row, col).State == player)
            {
                return nEnemyDisks;
            }

            return 0;
        }
    }
}
