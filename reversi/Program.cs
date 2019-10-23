using System;

namespace reversi
{
    class MainClass
    {
        // 盤: 横9*縦10 y*9+xと使う。0行目と9行目は番兵
        public static int[] board = new int[91];
        // 走査する方向を記憶する配列
        public static readonly int[] direction = { -10, -9, -8, -1, 1, 8, 9, 10 };

        public static void Main(string[] args)
        {
            // ボードの初期化
            board[40] = board[50] = 1;
            board[41] = board[49] = 2;


            for (int i = 1; i < 10; i++)
            {
                board[i * 9] = 3;
            }

            // ゲーム開始
            play_game();
        }

        public static void play_game()
        {
            // プレイヤー1から開始
            int player = 1;
            // 相手がパスをしたかを保存しておく変数
            bool enemy_passed = false;

            // ゲームが終了するまでループさせる
            while (true)
            {
                // 返せる石の数を保存しておく変数
                int n_flippable_disks = 0;

                // 盤面を表示
                show_board();

                // 置けるマスの個数をカウントする
                n_flippable_disks = count_flippable_disks(player);

                if (n_flippable_disks > 0) // 石を置く場所があるとき
                {
                    // 探索するときの基準となるマスを表す
                    int base_point;

                    if (player == 1) // プレイヤーが人間の場合
                    {
                        // 入力を求める
                        base_point = get_placeable_place(player);
                        flip_disks(base_point, player);
                    }
                    else // プレイヤーがNPCの場合
                    {
                        // 置ける場所を探索する
                        for (n_flippable_disks = 0, base_point = 8; n_flippable_disks == 0; n_flippable_disks = eight_way_scanning(base_point, player))
                        {
                            base_point++;
                        }
                        flip_disks(base_point, player);
                    }
                }
                else if (!enemy_passed) // 石が置けないとき、かつ、相手がパスしていないとき
                {
                    enemy_passed = true;
                    Console.WriteLine("passed");
                }
                else // 石が置けず、相手もパスしているとき
                {

                    Console.WriteLine("end");
                    break;
                }

                // プレーヤーを交代する
                player = 3 - player;
            }
        }

        // 盤を表示するためだけのメソッド
        public static void show_board()
        {
            string[] disk_marks = { "-", "o", "x", "\n" };
            for (int base_point = 9; base_point < 82; base_point++)
            {
                Console.Write(" {0}", disk_marks[board[base_point]]);
            }
        }

        // 盤の全体から裏返せる石を数える
        public static int count_flippable_disks(int player)
        {
            int n_flippable_disks = 0;
            for (int base_point = 9; base_point < 82; base_point++)
            {
                n_flippable_disks += eight_way_scanning(base_point, player);
            }
            return n_flippable_disks;
        }

        // 8方向を走査して、裏返せる石の個数を数える
        public static int eight_way_scanning(int base_point, int player)
        {
            int n_flippable_disks = 0;
            if (board[base_point] == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    int scanning_point, n_enemy_disks = 0;
                    for (scanning_point = base_point + direction[i]; board[scanning_point] == 3 - player; scanning_point += direction[i])
                    {
                        n_enemy_disks++;
                    }
                    if (n_enemy_disks > 0 && board[scanning_point] == player)
                    {
                        n_flippable_disks += n_enemy_disks;
                    }
                }
            }
            return n_flippable_disks;
        }

        // 石を置く場所の入力を求める関数
        public static int get_placeable_place(int player)
        {
            int base_point;
            while (true)
            {
                Console.WriteLine("石を置く場所：");
                string input_string = Console.ReadLine();
                string[] string_array = input_string.Split();
                int column = int.Parse(string_array[0]);
                int raw = int.Parse(string_array[1]);
                base_point = column + raw * 9;
                if (eight_way_scanning(base_point, player) > 0)
                {
                    break;
                }
                Console.WriteLine("石を置ける場所を入力してください");
            }

            return base_point;
        }

        // 実際に石を返す
        public static void flip_disks(int base_point, int player)
        {
            for (int i = 0; i < 8; i++)
            {
                int scanning_point, n_enemy_disks = 0;
                for (scanning_point = base_point + direction[i]; board[scanning_point] == 3 - player; scanning_point += direction[i])
                {
                    n_enemy_disks++;
                }
                if (n_enemy_disks > 0 && board[scanning_point] == player)
                {
                    scanning_point = base_point;
                    do
                    {
                        board[scanning_point] = player;
                        scanning_point += direction[i];
                    } while (board[scanning_point] != player);
                }
            }
        }
    }
}
