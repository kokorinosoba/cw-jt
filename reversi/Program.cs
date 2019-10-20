using System;

namespace reversi
{
    class MainClass
    {
        public static int player, n_flipable_disks;
        public static int[] board = new int[91], direction = { -10, -9, -8, -1, 1, 8, 9, 10 };
        public static string disk_marks = "- o x \n";

        public static void Main(string[] args)
        {
            board[40] = board[50] = 1;
            board[41] = board[49] = 2;

            player = 1;
            bool enemy_passed = false;

            for (int i = 1; i < 10; i++)
            {
                board[i * 9] = 3;
            }

            while (true)
            {
                int base_point;
                n_flipable_disks = 0;
                bool need_flip = false;

                for (base_point = 9; base_point < 82; base_point++)
                {
                    check(base_point, need_flip);
                    Console.Write(" {0}", disk_marks[board[base_point] * 2]);
                }

                if (n_flipable_disks > 0)
                {
                    need_flip = true;

                    for (n_flipable_disks = 0, base_point = 8; n_flipable_disks == 0; check(base_point,need_flip))
                    {
                        if (player == 2)
                        {
                            base_point++;
                        }
                        else
                        {
                            string[] tmp_str = Console.ReadLine().Split();
                            int column = int.Parse(tmp_str[0]);
                            int row = int.Parse(tmp_str[1]);
                            base_point = column + row * 9;
                        }
                    }
                }
                else if (!enemy_passed)
                {
                    enemy_passed = true;
                    Console.WriteLine("passed");
                }
                else
                {
                    break;
                }
                player = 3 - player;
            }
        }

        public static void check(int base_point, bool need_flip)
        {
            if (board[base_point] == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    int n_enemy_disks, checking_place;

                    for (n_enemy_disks = 0, checking_place = base_point + direction[i]; board[checking_place] == 3 - player; checking_place += direction[i])
                    {
                        n_enemy_disks++;
                    }

                    if (n_enemy_disks > 0 && board[checking_place] == player)
                    {
                        n_flipable_disks += n_enemy_disks;
                        checking_place = base_point;

                        if (need_flip)
                        {
                            do
                            {
                                board[checking_place] = player;
                                checking_place += direction[i];
                            } while (board[checking_place] != player);
                        }
                    }
                }
            }
        }
    }
}
