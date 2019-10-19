#include <stdio.h>
#include <stdbool.h>
// board[90] = {0}
// 盤状態:横9*縦10 y*9+xと使う。0行目と9行目は番兵

// direction[] = {-10, -9, -8, -1, 1, 8, 9, 10};
// 盤を走査する場合、縦横斜め方向に向かうために足されるべき数
int base_point, player, n_flipable_disks;
int board[90] = {0}, direction[] = {-10, -9, -8, -1, 1, 8, 9, 10};

void check(bool need_flip) {
  if (board[base_point] == 0)
    // それぞれの方向で走査して
    // 裏返せるコマがあったらその数をn_flipable_disksに追加・checking_placeを基準のコマに戻し
    // need_flipの値によってひっくり返す
    for (int i = 0; i < 8; i++) {
      // 8方向走査
      // direction[i]の方向の相手のコマの数を確認
      // direction[i]が方向を表す
      // checking_placeは走査している場所を表す
      // playerはそのターンのプレイヤー番号
      // n_enemy_disksは相手のコマの数を表す
      int n_enemy_disks, checking_place;
      // 走査している場所が相手の駒の間繰り返す
      for (n_enemy_disks = 0, checking_place = base_point + direction[i]; board[checking_place] == 3 - player; checking_place += direction[i]) {
        n_enemy_disks++;
      }

      if (n_enemy_disks && board[checking_place] == player) {
        // 1枚以上存在し、その上端が自分のコマなら
        n_flipable_disks += n_enemy_disks;
        checking_place = base_point;

        // need_flipがtrueの場合は、実際にひっくり返す
        if (need_flip)
          do {
            board[checking_place] = player, checking_place += direction[i];
          } while (board[checking_place] != player);
    }
  }
}

// boardに対応するオセロ駒＆改行
char *h = " - o x\n";

int main()
{
  // 0:コマ無し
  // 1:1player
  // 2:2player
  // 3:改行

  // 最初の石を置く
  board[40] = board[50] = 1;
  board[41] = board[49] = 2;

  // player1からスタートさせる
  player = 1;

  // enemy_passedをコントロールする変数
  bool enemy_passed = true, need_flip;

  for (int i = 1; i < 10; i++) {
    board[i * 9] = 3; // 9の倍数番目のマスに改行を入れる
  }

  // ここからゲーム開始
  while (true) {
    // 毎回n_flipable_disksとneed_flipを初期化
    n_flipable_disks = 0;
    need_flip = false;

    // 盤の表示
    for (base_point = 9; base_point < 82; base_point++) {
      check(need_flip); // ここで全マスのひっくり返せる場所を計算している
      printf("%.2s", &h[board[base_point] * 2]);
    }

    if (n_flipable_disks) {
      // 1枚でも駒が置けた場合はcomは左上から走査
      // 置けた(=n_flipable_disksの値が変わった)らターン終了
      for (need_flip = enemy_passed = true, n_flipable_disks = 0, base_point = 8; n_flipable_disks == 0; check(need_flip)) {
        if (player == 2) {
          base_point++;
        } else {
          int tmp;
          scanf("%d %d", &base_point, &tmp);
          base_point += tmp * 9;
        }
      }
    } else if (enemy_passed) {
      // 駒は置けない
      enemy_passed = false;
      printf("enemy_passed");
    } else {
      // 両者とも駒を置けないので終了
      break;
    }
    // player交代
    player = 3 - player;
  }
  return 0;
}
