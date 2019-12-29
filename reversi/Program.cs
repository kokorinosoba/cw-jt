using System;

namespace reversi
{
    class MainClass
    {
        static Board board = new Board();

        public static void Main(string[] args)
        {
            // ゲーム開始
            PlayGame();

            // 結果を表示
            ShowResult();
        }

        public static void PlayGame()
        {
            //  0: 石なし
            //  1: player1
            // -1: player2

            // プレイヤー1から開始
            int player = 1;

            Player player1 = new Player( 1, true, 1);
            Player player2 = new Player(-1, true, 2);

            // 相手がパスをしたかを保存しておく変数
            bool enemyPassed = false;

            // ゲームが終了するまでループさせる
            while (true)
            {
                // 盤を表示
                board.Show();

                if (board.GetPlaceableCells(player).Count > 0) // 石を置く場所があるとき
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

        // ゲームの結果を表示する
        public static void ShowResult()
        {
            // ボード上に石がいくつあるか数える
            (int disksPlayer1, int disksPlayer2) = board.CountDisks();

            Console.WriteLine("------------------");
            Console.WriteLine("      Result");
            Console.WriteLine("------------------");
            Console.WriteLine("Player1: {0}, Player2: {1}", disksPlayer1, disksPlayer2);

            if (disksPlayer1 > disksPlayer2)
            {
                Console.WriteLine("Player1 win!");
            }
            else if (disksPlayer1 < disksPlayer2)
            {
                Console.WriteLine("Player1 lose...");
            }
            else
            {
                Console.WriteLine("Draw");
            }
        }
    }
}
