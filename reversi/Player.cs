using System;
namespace reversi
{
    public class Player
    {
        int PlayerNumber;
        bool IsComputer;
        int AlgorithmNumber;

        public Player(int playerNumber, bool isComputer, int algorithmNumber = 1)
        {
            this.PlayerNumber = playerNumber;
            this.IsComputer = isComputer;
            this.AlgorithmNumber = algorithmNumber;
        }

        // 石を置く
        public void Put(Board board)
        {
            Cell baseCell;

            if (this.IsComputer)
            {
                baseCell = this.AiSearch(board);
            }
            else
            {
                baseCell = this.HumanInput(board);
            }

            baseCell.Put(this.PlayerNumber);
            board.EightWayScanning(this.PlayerNumber, baseCell, true);
        }

        // 裏返せる石が最大の個数になる場所を探索
        public Cell AiSearch(Board board)
        {
            Cell bestCell;

            if (AlgorithmNumber == 1)
            {
                bestCell = Algorithm1(board);
            }
            else
            {
                bestCell = Algorithm2(board);
            }

            return bestCell;
        }

        // 裏返せる石が最大の個数になる場所を探索するアルゴリズム
        public Cell Algorithm1(Board board)
        {
            int nFlippableDisks = 0, maxFlippableDisks = 0;
            Cell bestCell = board.GetCell(0, 0);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Cell baseCell = board.GetCell(row, col);

                    // 石を置ける場合のみ計算を行う
                    if (baseCell != null && baseCell.CanPut())
                    {
                        nFlippableDisks = board.EightWayScanning(this.PlayerNumber, baseCell);
                        if (nFlippableDisks > maxFlippableDisks)
                        {
                            maxFlippableDisks = nFlippableDisks;
                            bestCell = board.GetCell(row, col);
                        }
                    }
                }
            }

            return bestCell;
        }

        // 盤面評価によって最大のスコアになる場所を探索するアルゴリズム
        public Cell Algorithm2(Board board)
        {
            int[,] score = {
                             {  30, -12,   0,  -1,  -1,   0, -12,  30 },
                             { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                             {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                             {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                             {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                             {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                             { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                             {  30, -12,   0,  -1,  -1,   0, -12,  30 },
                           };

            return new Cell(0, 0);
        }

        // 置ける場所の入力を求める
        public Cell HumanInput(Board board)
        {
            Cell baseCell;

            while (true)
            {
                Console.Write("Enter place to put a disk: ");
                try
                {
                    // 座標を入力させ、それをボード上の位置に変換する
                    string inputString = Console.ReadLine();
                    int col = int.Parse((inputString[0] - 'a').ToString());
                    int row = int.Parse(inputString[1].ToString()) - 1;
                    baseCell = board.GetCell(row, col);

                    // 入力した場所に裏返せる石があった場合ループを抜ける
                    if (baseCell != null && baseCell.CanPut() && board.EightWayScanning(this.PlayerNumber, baseCell) > 0)
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                Console.WriteLine("Disk cannot be placed there.");
            }

            return baseCell;
        }
    }
}
