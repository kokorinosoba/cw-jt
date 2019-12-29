using System;

namespace reversi
{
    class MainClass
    {
        // ボード: 横9*縦10 y*9+xと使う。0行目と9行目は番兵
        public static int[] board = new int[91];
        // 走査する方向を記憶する配列
        public static readonly int[] direction = { -10, -9, -8, -1, 1, 8, 9, 10 };

        public static void Main(string[] args)
        {
            PlayGame();
            // 0: 石なし
            // 1: Player1
            // 2: Player2
            // 3: 改行

            // ボードの初期化
            board[40] = board[50] = 1;
            board[41] = board[49] = 2;


            for (int i = 1; i < 10; i++)
            {
                board[i * 9] = 3;
            }

            // ゲーム開始
            play_game();
            // 結果を表示
            show_result();
        }

        public static void PlayGame()
        {
            Board board = new Board();
            int player = 1;
            Player player1 = new Player( 1, true);
            Player player2 = new Player(-1, true);
            bool enemyPassed = false;

            while (true)
            {
                board.Show();
                if (board.CountFlippableDisks(player) > 0) // 石を置く場所があるとき
                {
                    // 石を置く
                    if (player == 1)
                    {
                        player1.Put(board);
                    }
                    else
                    {
                        player2.Put(board);
                    }
                    enemyPassed = false;
                }
                else if (!enemyPassed) // 石が置けないとき、かつ、相手がパスしていないとき
                {
                    // パスをする
                    Console.WriteLine("passed\n");
                    enemyPassed = true;
                }
                else // 石が置けず、相手もパスしているとき
                {
                    // ゲーム終了
                    Console.WriteLine("end\n");
                    break;
                }

                // プレーヤーを交代する
                player *= -1;
            }
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
                        base_point = input_place(player);
                        flip_disks(base_point, player);
                    }
                    else // プレイヤーがNPCの場合
                    {
                        // 置ける場所を探索する
                        n_flippable_disks = 0;
                        int max_flippable_disks = 0, best_place = 0;

                        // 裏返せる石が最大の個数になる場所を探索
                        for (base_point = 9; base_point < 81; base_point++)
                        {
                            n_flippable_disks = eight_way_scanning(base_point, player);
                            if (n_flippable_disks > max_flippable_disks)
                            {
                                max_flippable_disks = n_flippable_disks;
                                best_place = base_point;
                            }
                        }
                        flip_disks(best_place, player);
                    }
                    enemy_passed = false;
                }
                else if (!enemy_passed) // 石が置けないとき、かつ、相手がパスしていないとき
                {
                    // パスをする
                    Console.WriteLine("passed\n");
                    enemy_passed = true;
                }
                else // 石が置けず、相手もパスしているとき
                {
                    // ゲーム終了
                    Console.WriteLine("end\n");
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

            // ノーテーション用のアルファベットを表示
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(" {0}", (char)('a' + i));
            }
            // 盤面を表示
            for (int base_point = 9; base_point < 81; base_point++)
            {
                Console.Write(" {0}", disk_marks[board[base_point]]);
                if (base_point % 9 == 0)
                {
                    Console.Write(" {0}", base_point / 9);
                }
            }
            Console.WriteLine("\n");
        }

        // 盤の全体を走査して、裏返せる石を数える
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
                    // 調べる方向に相手の石があったとき、n_enemy_disksを増やす
                    for (scanning_point = base_point + direction[i]; board[scanning_point] == 3 - player; scanning_point += direction[i])
                    {
                        n_enemy_disks++;
                    }
                    if (n_enemy_disks > 0 && board[scanning_point] == player) // 相手の石が1枚以上あり、その上端が相手の駒だったとき
                    {
                        n_flippable_disks += n_enemy_disks;
                    }
                }
            }
            return n_flippable_disks;
        }

        // 石を置く場所の入力を求めるメソッド
        public static int input_place(int player)
        {
            int base_point;

            while (true)
            {
                Console.Write("Enter place to put a disk: ");
                try
                {
                    // 座標を入力させ、それをボード上の位置に変換する
                    string input_string = Console.ReadLine();
                    int column = int.Parse((input_string[0] - 'a' + 1).ToString());
                    int raw = int.Parse(input_string[1].ToString());
                    base_point = column + raw * 9;
                    if (eight_way_scanning(base_point, player) > 0) // 入力した場所に裏返せる石があった場合ループを抜ける
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

            return base_point;
        }

        // 実際に石を返す
        public static void flip_disks(int base_point, int player)
        {
            for (int i = 0; i < 8; i++)
            {
                // 8方向走査とほとんど同じ
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
                        // ここで裏返す
                        board[scanning_point] = player;
                        scanning_point += direction[i];
                    } while (board[scanning_point] != player);
                }
            }
        }

        // ゲームの結果を表示する
        public static void show_result()
        {
            int[] n_player_disks = new int[4];
            // ボード上に石がいくつあるか数える
            foreach (int disk in board)
            {
                n_player_disks[disk]++;
            }
            Console.WriteLine("------------------");
            Console.WriteLine("      Result");
            Console.WriteLine("------------------");
            Console.WriteLine("You: {0}, Enemy: {1}", n_player_disks[1], n_player_disks[2]);
            if (n_player_disks[1] > n_player_disks[2])
            {
                Console.WriteLine("You win!");
            }
            else if (n_player_disks[1] < n_player_disks[2])
            {
                Console.WriteLine("You lose...");
            }
            else
            {
                Console.WriteLine("Draw");
            }
        }
    }
}
