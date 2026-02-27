using System;
using System.Linq;

Console.WriteLine("Hello, Blackjack");

string str;
bool restart = true;

Play player = new Play("플레이어");
Play dealer = new Play("딜러");

Console.WriteLine("=== 블랙잭 게임===");
Play.CardShupple();

while (restart)
{
    if (Play.cardCount >= 48)
        Play.CardShupple();

    player.playInitialize();
    dealer.playInitialize();

    Console.WriteLine();
    Console.WriteLine("=== 초기 패 ===");
    Console.WriteLine($"{dealer.Name}의 패: {dealer.Carddraw()} {dealer.Carddraw()}");
    Console.WriteLine($"딜러 점수: ?");
    Console.WriteLine();
    Console.WriteLine($"{player.Name}의 패: {player.Carddraw()} {player.Carddraw()}");
    Console.WriteLine($"플레이어 점수: {player.score}");
    Console.WriteLine();

    if(player.score > 21 || dealer.score > 21)
    {
        str = "Stand";
    }
    else
    {
        str = player.HitorStand();
    }
        

    // 플레이어 진행
    while(str == "Hit")
    {

        // 뽑은 카드 출력
        player.ShowCard(player.Carddraw());
        if(player.score > 21)
        {
            break;
        }
        // 패 출력
        player.ShowDeck();


        str = player.HitorStand();
    }

    // 딜러 진행
    while(dealer.score <= 17)
    {
        dealer.ShowCard(dealer.invisibleCard);
        dealer.ShowDeck();

        dealer.ShowCard(dealer.Carddraw());
        dealer.ShowDeck();


    }





    Console.WriteLine("=== 게임 결과 ===");
    Console.WriteLine($"플레이어: {player.score}");
    Console.WriteLine($"딜러: {dealer.score}");
    Console.WriteLine();
    restart = Play.Judge(player.score, dealer.score);
    
    
}

Console.WriteLine("게임을 종료합니다.");