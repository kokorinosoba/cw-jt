﻿using System;
using System.Collections.Generic;

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

            List<Cell> flippableCells = board.EightWayScanning(this.PlayerNumber, baseCell);
            foreach (Cell flippableCell in flippableCells)
            {
                flippableCell.Flip();
            }
        }

        // 裏返せる石が最大の個数になる場所を探索
        public Cell AiSearch(Board board)
        {
            Cell bestCell;

            switch (AlgorithmNumber)
            {
                case 1:  bestCell = Algorithm1(board); break;
                case 2:  bestCell = Algorithm2(board); break;
                case 3:  bestCell = Algorithm3(board); break;
                case 4:  bestCell = Algorithm4(board); break;
                default: bestCell = Algorithm1(board); break;
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
                        nFlippableDisks = board.EightWayScanning(this.PlayerNumber, baseCell).Count;
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

        // 盤面評価によって裏返す石が最大のスコアになる場所を探索するアルゴリズム
        public Cell Algorithm2(Board board)
        {
            int[,] scoreArray = {
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
            };

            int score = 0, maxScore = (int) -1e+9;
            Cell bestCell = board.GetCell(0, 0);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Cell baseCell = board.GetCell(row, col);

                    // 石を置ける場合のみ計算を行う
                    if (baseCell != null && baseCell.CanPut())
                    {
                        List<Cell> flippableCells = board.EightWayScanning(this.PlayerNumber, baseCell);
                        score = 0;

                        foreach (Cell flippableCell in flippableCells)
                        {
                            score += scoreArray[flippableCell.Row, flippableCell.Col];
                        }

                        if (flippableCells.Count > 0 && score > maxScore)
                        {
                            maxScore = score;
                            bestCell = baseCell;
                        }
                    }
                }
            }

            return bestCell;
        }

        // 盤面評価によって相手とのスコアの差が最大になる場所を探索するアルゴリズム
        public Cell Algorithm3(Board board)
        {
            int[,] scoreArray = {
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
            };

            int score = 0, enemyScore = 0;
            int diffScore, maxDiffScore = (int)-1e+9;

            Cell bestCell = board.GetCell(0, 0);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Cell baseCell = board.GetCell(row, col);

                    // 石を置ける場合のみ計算を行う
                    if (baseCell != null && baseCell.CanPut())
                    {
                        // 裏返せるセルの取得
                        List<Cell> flippableCells = board.EightWayScanning(this.PlayerNumber, baseCell);
                        score = 0;

                        // 裏返せるセルを裏返す
                        baseCell.Put(PlayerNumber);
                        foreach (Cell flippableCell in flippableCells)
                        {
                            flippableCell.Flip();
                        }

                        for (int innerRow = 0; innerRow < 8; innerRow++)
                        {
                            for (int innerCol = 0; innerCol < 8; innerCol++)
                            {
                                if (board.GetCell(innerRow, innerCol).State == this.PlayerNumber)
                                {
                                    score += scoreArray[innerRow, innerCol];
                                }
                                else if (board.GetCell(innerRow, innerCol).State == -this.PlayerNumber)
                                {
                                    enemyScore += scoreArray[innerRow, innerCol];
                                }
                            }
                        }

                        // 裏返したものを元に戻す
                        baseCell.Put(0);
                        foreach (Cell flippableCell in flippableCells)
                        {
                            flippableCell.Flip();
                        }

                        diffScore = score - enemyScore;

                        if (flippableCells.Count > 0 && diffScore > maxDiffScore)
                        {
                            maxDiffScore = diffScore;
                            bestCell = baseCell;
                        }
                    }
                }
            }

            return bestCell;
        }

        // 盤面評価によって置く場所のスコアが最大になる場所を探索するアルゴリズム
        public Cell Algorithm4(Board board)
        {
            int[,] scoreArray = {
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1 },
                {   0,  -3,   0,  -1,  -1,   0,  -3,   0 },
                { -12, -15,  -3,  -3,  -3,  -3, -15, -12 },
                {  30, -12,   0,  -1,  -1,   0, -12,  30 },
            };

            int score = 0, maxScore = (int)-1e+9;
            Cell bestCell = board.GetCell(0, 0);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Cell baseCell = board.GetCell(row, col);

                    // 石を置ける場合のみ計算を行う
                    if (baseCell != null && baseCell.CanPut())
                    {
                        score = scoreArray[row, col];
                        List<Cell> flippableCells = board.EightWayScanning(this.PlayerNumber, baseCell);

                        if (flippableCells.Count > 0 && score > maxScore)
                        {
                            maxScore = score;
                            bestCell = baseCell;
                        }
                    }
                }
            }

            return bestCell;
        }


        // 置ける場所の入力を求める
        public Cell HumanInput(Board board)
        {
            Cell baseCell;

            while (true)
            {
                Console.Write("Enter the place to put a disk: ");
                try
                {
                    // 座標を入力させ、それをボード上の位置に変換する
                    string inputString = Console.ReadLine();
                    int col = int.Parse((inputString[0] - 'a').ToString());
                    int row = int.Parse(inputString[1].ToString()) - 1;
                    baseCell = board.GetCell(row, col);

                    // 入力した場所に裏返せる石があった場合ループを抜ける
                    if (baseCell != null && baseCell.CanPut() && board.EightWayScanning(this.PlayerNumber, baseCell).Count > 0)
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
