using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


class Play
{
    // 카드를 숫자와 문양별로 받기위한 이중 배열
    // random함수를 이용해서 숫자 정렬로 카드를 무작위로 뽑은 후
    // 실제 플레이할 카드 양식을 담아줌
    public static int[,] cards;
    public static string[] playCard;

    // 카드가 뽑힌 횟수. 공통인수이기 때문에 static으로 관리
    public static int cardCount = 0;


    //플레이어 이름
    public string Name;
    
    // 딜러가 숨긴 카드
    public string invisibleCard;


    // 드로우한 카드.
    // Hit stand 에 따라 뽑는 횟수가 다르기에 인스턴스변수로 관리
    public string[] drawCard;
    public int drawCount;

    // 앞서 사용한 카드 횟수를 세는 변수
    public int skipNumber = 0;


    // 최종 점수
    public int score;
    


    // 생성자로 이름, 뽑은 카드 관리하는 배열 초기화
    // 양쪽에서 번갈아서 뽑기 때문에 반드시 전체 덱 수 보다 작음 >> index 에러 없음
    public Play(string name)
    {
        Name = name;
        drawCard = new string[52];
    }

    // 재시작 됐을 때 초기화 해야하는 인자들 관리
    public void playInitialize()
    {
        score = 0;
        skipNumber = drawCount;
    }

    // 랜덤함수로 무작위 조합 생성해서 카드 위치 시키기
    public static void CardShupple()
    {
        
        Console.WriteLine("\n카드를 섞는 중...");
        playCard = new string[52];
        cards = new int[52, 2];

        int count = 0;

        Random random = new Random();

        while (cards[51, 1] == 0)
        {
            int i = random.Next(1, 5);
            int j = random.Next(1, 14);
            bool isCount = false;

            for (int k = 0; k < 52; k++)
            {
                if (cards[k, 0] == j && cards[k, 1] == i)
                {
                    isCount = true;
                    break;
                }

            }

            if (!isCount)
            {
                cards[count, 0] = j;
                cards[count, 1] = i;
                count++;

            }

        }

    }


    // 카드 셔플에 양식을 같이 넣어줘도 되지만
    // int 값 그대로 점수계산에 쓰고싶어서 뽑을 때 변환 하도록 진행.
    
    public string Carddraw()
    {
        string mark;
        string card;

        // 점수 계산
        ScoreCalculate(cards[cardCount, 0]);

        // 마크 생성
        mark = ConvertMark(cards[cardCount, 1]);

        // 그림카드 분리
        card = ConvertNumber(cards[cardCount, 0]);

        // 실제 플레이 카드에 양식 맞춰서 등록
        playCard[cardCount] = $"[{mark}{card}]";

        // 딜러 카드 숨기기
        if (Name == "딜러" && drawCount - skipNumber == 0)
        {
            invisibleCard = playCard[cardCount];
            playCard[cardCount] = "[??]";

        }

        //뽑은 카드 인덱스 등록
        drawCard[drawCount] = playCard[cardCount];
        drawCount++;



        //플레이어 카드 출력
        

        return playCard[cardCount++];
    }

    // 마크 전환함수
    public string ConvertMark(int number)
    {
        string mark;

        switch (number)
        {
            case 1:
                mark = "♠";
                break;
            case 2:
                mark = "◆";
                break;
            case 3:
                mark = "♥";
                break;
            case 4:
                mark = "♣";
                break;
            default:
                mark = "unknown";
                break;
        }

        return mark;
    }

    // 숫자 전환함수
    public string ConvertNumber(int number)
    {
        string card;

        switch (number)
        {
            case 1:
                card = "A";
                break;
            case 11:
                card = "J";
                break;
            case 12:
                card = "Q";
                break;
            case 13:
                card = "K";
                break;
            default:
                card = $"{number}";
                break;

        }


        return card;
    }

    // 점수 계산
    public void ScoreCalculate(int score)
    {
        if (score == 1)
        {
            // A 알아서 11, 1 중 좋은걸로
            if(this.score > 10)
            {
                this.score += 1;
            }
            else
            {
                this.score += 11;
            }
        }
        else 
        {
            this.score += score;
        }

    }


    // Hit or Stand 결정
    public string HitorStand()
    {
        string HorS;
        Console.Write("H(Hit) 또는 S(Stand)를 선택하세요:");
        HorS = Console.ReadLine().ToUpper() == "H" ? "Hit" : "Stand";

        Console.WriteLine($"플레이어가 {HorS}를 선택했습니다");
        Console.WriteLine();
        return HorS;
    }


    // 카드 보여주는 메서드
    public void ShowCard(string card)
    {
        if(card == invisibleCard)
        {
            Console.WriteLine($"딜러의 숨겨진 카드: {invisibleCard}");

            // 초기카드는 항상 2장이기 때문에 하드 코딩
            drawCard[drawCount - 2] = invisibleCard;
        }
        else
        {
            Console.WriteLine($"{Name}가 카드를 받았습니다: {card}");
        }
            
    }


    // 보유한 패 공개
    public void ShowDeck()
    {
        // 진행된 카드뽑기 수만큼 foreach 구문 스킵
        Console.Write($"{Name}의 패: ");
        foreach (string strs in drawCard.Skip(skipNumber))
        {
            if (strs != "")
                Console.Write(strs + " ");
        }
        Console.WriteLine();
        Console.WriteLine($"딜러 점수: {score}");
        Console.WriteLine();
    }


    // 승패 판단
    public static bool Judge(int player, int dealer)
    {
        if (dealer > 21 && player < 21)
        {
            Console.WriteLine("딜러 버스트!\n플레이어 승리");
        }
        else if (player > 21 && dealer < 21)
        {
            Console.WriteLine("플레이어 버스트!\n 승리");
        }
        else if (dealer > 21 && player > 21 || dealer == player)
        {
            Console.WriteLine("무승부!");
        }
        else
        {
            string victory = player > dealer ? "플레이어 승리!" : "플레이어 패배 ㅜㅜ";
            Console.WriteLine(victory);
        }


        Console.WriteLine();
        Console.Write("새 게임을 하시겠습니까? (Y/N):");
        return Console.ReadLine().ToUpper() == "Y" ? true : false;
    }
}

   