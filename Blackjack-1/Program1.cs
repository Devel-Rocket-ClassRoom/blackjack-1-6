using System;
using System.Numerics;
using blackjack;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== 블랙잭 게임 ===");
        Console.Write("이름을 입력하세요: ");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName);
        Player dealer = new Player("딜러");

        bool restart = true;
        string input = "";

        while (restart)
        {
            Console.WriteLine("\n카드를 섞는 중...");
            Player.ShuffleCards();

            player.ResetValues();
            dealer.ResetValues();
            input = "";

            // 초기 패 배분
            player.DrawCard(); player.CalcScore();
            player.DrawCard(); player.CalcScore();
            dealer.DrawCard(); dealer.CalcScore();
            dealer.hiddenCard = dealer.DrawCard();
            dealer.CalcScore();

            // 초기 패 출력
            Console.WriteLine("\n=== 초기 패 ===");
            Console.WriteLine("\n=== 초기 패 ===");
            dealer.ShowHand(true);   // true = 히든 모드
            player.ShowHand(false);  // false = 전체 공개

            // 21 즉시 체크
            if (player.score >= 21 || dealer.score >= 21)
                input = "S";

            // 플레이어 턴
            while (input != "S")
            {
                Console.Write("\nH(Hit) 또는 S(Stand)를 선택하세요: ");
                input = player.GetHitOrStand();

                if (input == "H")
                {
                    string card = player.DrawCard();
                    player.CalcScore();
                    Console.WriteLine($"{player.name}가 카드를 받았습니다: [{card}]");
                    player.ShowHand(false);

                    if (player.score > 21)
                        input = "S";
                }
                else
                {
                    Console.WriteLine($"{player.name}가 Stand를 선택했습니다.");
                }
            }

            // 딜러 턴
            Console.WriteLine($"\n딜러의 숨겨진 카드: [{dealer.hiddenCard}]");
            dealer.ShowHand(false);

            while (dealer.score < 17)
            {
                string card = dealer.DrawCard();
                dealer.CalcScore();
                Console.WriteLine($"딜러가 카드를 받습니다: [{card}]");
                dealer.ShowHand(false);
            }

            // 결과
            restart = Player.JudgeResult(dealer, player);
        }

        Console.WriteLine("게임을 종료합니다.");
    }
}