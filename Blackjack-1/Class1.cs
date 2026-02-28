using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Text;

namespace 블랙잭
{
    using System;
    using System.Collections;
    using System.Security.Cryptography.X509Certificates;
    class Player
    {
        // static 필드
        static int[,] cards;
        static string[] playCard;
        static int totalDrawCount;

        // instance 필드
        int drawCount;
        string[] drawCard;
        int usedCardCount;
        public string hiddenCard;
        public string name;
        public int score;

        // 생성자
        public Player(string name)
        {
            this.name = name;
            drawCard = new string[52];
        }

        // static 메소드
        public static void ShuffleCards()
        {
            cards = new int[52, 2];
            for (int i = 0; i < 52; i++)
            {
                cards[i, 0] = i / 13;
                cards[i, 1] = i % 13 + 1;
            }
            Random rand = new Random();
            for (int i = 51; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                int temp = cards[i, 0];
                cards[i, 0] = cards[j, 0];
                cards[j, 0] = temp;
                int temp2 = cards[i, 1];
                cards[i, 1] = cards[j, 1];
                cards[j, 1] = temp2;
            }
            playCard = new string[52];
            totalDrawCount = 0;
        }
        public static bool JudgeResult(Player dealer, Player player)
        {
            Console.WriteLine("\n=== 게임 결과 ===");
            Console.WriteLine($"{player.name}: {player.score}점");
            Console.WriteLine($"딜러: {dealer.score}점");

            if (player.score > 21)
                Console.WriteLine("플레이어 버스트! 딜러 승리!");
            else if (dealer.score > 21)
                Console.WriteLine("딜러 버스트! 플레이어 승리!");
            else if (player.score > dealer.score)
                Console.WriteLine($"플레이어 승리!");
            else if (player.score < dealer.score)
                Console.WriteLine("딜러 승리!");
            else
                Console.WriteLine("무승부!");

            while (true)
            {
                Console.Write("\n새 게임을 하시겠습니까? (Y/N): ");
                string input = Console.ReadLine().ToUpper();
                if (input == "Y") return true;
                if (input == "N") return false;
                Console.WriteLine("Y 또는 N을 입력해주세요.");
            }
        }


        // instance 메소드
        public string DrawCard()
        {
            int suit = cards[totalDrawCount, 0];
            int number = cards[totalDrawCount, 1];
            string card = ConvertSuit(suit) + ConvertNumber(number);
            playCard[totalDrawCount] = card;
            drawCard[drawCount] = card;
            drawCount++;
            totalDrawCount++;
            usedCardCount++;
            return card;
        }

        string ConvertSuit(int suitNum)
        {
            string[] suits = { "♠", "♥", "◆", "♣" };
            return suits[suitNum];  // 인덱스로 바로 접근!
        }
        string ConvertNumber(int num)
        {
            string[] numbers = { "", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            return numbers[num];  // num이 1~13 이므로 0번은 빈칸으로!
        }
        public void CalcScore()
        {
            score = 0;
            int aceCount = 0;
            for (int i = 0; i < drawCount; i++)
            {
                string numPart = drawCard[i].Substring(1);

                if (numPart == "A")
                {
                    score += 11;
                    aceCount++;
                }
                else if (numPart == "J" || numPart == "Q" || numPart == "K")
                    score += 10;
                else
                    score += int.Parse(numPart);
            }


            // 버스트면 Ace를 11→1로 전환
            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }
        }
        public string GetHitOrStand()
        {
            while (true)
            {
                Console.Write("Hit(H) / Stand(S): ");
                string input = Console.ReadLine().ToUpper();
                if (input == "H" || input == "S")
                {
                    return input;
                }
                Console.WriteLine("H 또는 S를 입력하세요.");

            }
        }
        public void ShowDrawnCard()
        {
            Console.WriteLine($"{name} 가 뽑은 카드: {drawCard[drawCount - 1]}");
        }

        // idden = true 면 첫 카드 ?? 처리
        public void ShowHand(bool hidden)
        {
            Console.Write($"{name}의 패: ");

            if (hidden)
            {
                Console.Write("[??] ");
                for (int i = 0; i < drawCount - 1; i++)
                    Console.Write($"[{drawCard[i]}] ");
                Console.WriteLine($"\n{name} 점수: ?");
            }
            else
            {
                for (int i = 0; i < drawCount; i++)
                    Console.Write($"[{drawCard[i]}] ");
                Console.WriteLine($"\n{name} 점수: {score}");
            }
        }

        public void ResetValues()
        {
            drawCount = 0;
            usedCardCount = 0;
            score = 0;
            hiddenCard = "";
            drawCard = new string[52];
        }
    }

}
