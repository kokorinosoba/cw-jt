using System;

namespace reversi
{
    class MainClass
    {
        public static int[] board = new int[91];
        public static int[] direction = { -10, -9, -8, -1, 1, 8, 9, 10 };

        public static void Main(string[] args)
        {
            // ボードの初期化
            board[40] = board[50] = 1;
            board[41] = board[49] = 2;

            for (int i = 1; i < 10; i++)
                board[i * 9] = 3;

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
                // 探索するときの基準となるマスを表す
                int base_point;
                // 返せる石を保存しておく変数
                int n_flippable_disks = 0;
                // 石を返す必要があるかを制御する変数
                bool need_flip = false;

                show_board();

                // 置けるマスの個数をカウントする
                n_flippable_disks = count_flippable_disks(player);

                if (n_flippable_disks > 0)
                {
                    // 石を置く場所がある
                    if (player == 1)
                    {
                        // プレイヤーが人間の場合
                        base_point = get_placeable_place(player);
                    }
                }
                else if (!enemy_passed)
                {
                    // 石が置けないとき
                    enemy_passed = true;
                    Console.WriteLine("passed");
                }
                else
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

        public static int count_flippable_disks(int player)
        {
            int n_flippable_disks = 0;
            for (int base_point = 9; base_point < 82; base_point++)
            {
                n_flippable_disks += eight_way_scanning(base_point, player);
            }
            return n_flippable_disks;
        }

        // 8方向を走査して、相手の石の個数を数える
        public static int eight_way_scanning(int base_point, int player)
        {
            int n_enemy_disks = 0;
            if (board[base_point] == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int scanning_point = base_point + direction[i]; board[scanning_point] == 3 - player; scanning_point += direction[i])
                    {
                        n_enemy_disks++;
                    }
                }
            }
            return n_enemy_disks;
        }

        public static int get_placeable_place(int player)
        {
            int base_point;

            do
            {
                string input_string = Console.ReadLine();
                string[] string_array = input_string.Split();
                int column = int.Parse(string_array[0]);
                int raw = int.Parse(string_array[1]);
                base_point = column + raw * 9;
            } while (eight_way_scanning(base_point, player) <= 0);

            return base_point;
        }
    }
}
