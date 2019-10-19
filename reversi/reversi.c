#include <stdio.h>
#include <stdbool.h>
// map[90] = {0}
// 盤状態:横9*縦10 y*9+xと使う。0行目と9行目は番兵

// dir[] = {-10, -9, -8, -1, 1, 8, 9, 10};
// 盤を走査する場合、縦横斜め方向に向かうために足されるべき数
int put, turn, all, count, value, i;
bool done, pass;
int map[90] = {0}, dir[] = {-10, -9, -8, -1, 1, 8, 9, 10};

void check() {
  if (map[put] == 0)
    // それぞれの方向で走査して
    // 裏返せるコマがあったらその数をallに追加・valueを基準のコマに戻し
    // doneの値によってひっくり返す
    for (i = 0; i < 8; i++) {
      // 8方向走査
      // dir[i]の方向の相手のコマの数を確認
      // iが方向を表す
      // valueは走査している場所を表す
      // turnはそのターンのプレイヤー番号
      // countは相手のコマの数を表す

      // 走査している場所が相手の駒の間繰り返す
      for (count = 0, value = put + dir[i]; map[value] == 3 - turn; value += dir[i]) {
        count++;
      }

      if (count && map[value] == turn) {
        // 1枚以上存在し、その上端が自分のコマなら
        all += count;
        value = put;

        // doneがtrueの場合は、実際にひっくり返す
        if (done)
          do {
            map[value] = turn, value += dir[i];
          } while (map[value] != turn);
    }
  }
}

// mapに対応するオセロ駒＆改行
char *h = " - o x\n";

int main()
{
  // 0:コマ無し
  // 1:1player
  // 2:2player
  // 3:改行

  // 最初の石を置く
  map[40] = map[50] = 1;
  map[41] = map[49] = 2;

  // player1からスタートさせる
  turn = 1;

  // passをコントロールする変数
  pass = true;

  for (i = 1; i < 10; i++) {
    map[i * 9] = 3; // 9の倍数番目のマスに改行を入れる
  }

  // ここからゲーム開始
  while (true) {
    // 毎回allとdoneを初期化
    all = 0;
    done = false;

    // 盤の表示
    for (put = 9; put < 82; put++) {
      check(); // ここで全マスのひっくり返せる場所を計算している
      printf("%.2s", &h[map[put] * 2]);
    }

    if (all) {
      // 1枚でも駒が置けた場合はcomは左上から走査
      // 置けた(=allの値が変わった)らturn終了
      for (done = pass = true, all = put = 8; all == 8; check()) {
        if (turn == 2) {
          put++;
        } else {
          scanf("%d %d", &put, &i);
          put += i * 9;
        }
      }
    } else if (pass) {
      // 駒は置けない
      pass = false;
      printf("pass");
    } else {
      // 両者とも駒を置けないので終了
      break;
    }
    // turn交代
    turn = 3 - turn;
  }
  return 0;
}
