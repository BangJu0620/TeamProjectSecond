using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGQuest.QuestSystem;

namespace TeamProjectSecond
{
    public class Casino
    {
        public static void EnterCasino()
        {
            var character = Character.Instance;

            //ShowEnter();

            while (true)
            {
                EventManager.Clear();
                Console.SetCursorPosition(0, 8);
                EventManager.To(50, "지불할 코인을 선택해주세요.");
                EventManager.To(40, "코인의 등급이 높을 수록 좋은 보상을 얻을 확률이 높아집니다.");
                Console.SetCursorPosition(0, 12);
                EventManager.To(42, "1. 내배캠 코인\n\n");
                EventManager.To(42, "2. 스파르타 코인\n\n");
                EventManager.To(42, "3. 비트 코인\n\n");
                EventManager.To(42, "Enter. 돌아가기");
                EventManager.Select();

                int? choice = EventManager.CheckInput();
                switch (choice)
                {
                    case 1:
                        Coin1(choice);
                        break;
                    case 2:
                        // 소지품 확인
                        break;
                    case 3:
                        // 상점
                        break;
                    case null:
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }
        public static void Coin1(int? choice)
        {
            while (true)
            {
                var character = Character.Instance;
                EventManager.Clear();
                Console.SetCursorPosition(0, 8);
                switch (choice)
                {
                    case 1:
                        EventManager.To(50, "내배캠 코인을 지불하고 도박판에 뛰어듭니다.");
                        break;
                    case 2:
                        EventManager.To(50, "스파르타 코인을 지불하고 도박판에 뛰어듭니다.");
                        break;
                    case 3:
                        EventManager.To(50, "비트 코인을 지불하고 도박판에 뛰어듭니다.");
                        break;
                }
                EventManager.To(40, $"남은 코인 수 {character.NaeBaeCampCoin}");
                Console.SetCursorPosition(0, 12);
                EventManager.To(42, "1. Dice Roll!!\n\n");
                EventManager.To(42, "Enter. 돌아가기");
                EventManager.Select();

                int? choice2 = EventManager.CheckInput();
                if (choice2 == 1)
                {
                    if (choice == 1)
                    {
                        if (character.NaeBaeCampCoin > 0)
                        {
                            character.NaeBaeCampCoin--;
                            GambleDiceRoll(1);
                        }
                        else
                        {
                            EventManager.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(0, 14);
                            EventManager.To(54, "코인이 부족합니다!");
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    else if (choice == 2)
                    {
                        if (character.SpartaCoin > 0)
                        {
                            character.SpartaCoin--;
                            GambleDiceRoll(2);
                        }
                        else
                        {
                            EventManager.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(0, 14);
                            EventManager.To(54, "코인이 부족합니다!");
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    else if (choice == 3)
                    {
                        if (character.BitCoin > 0)
                        {
                            character.BitCoin--;
                            GambleDiceRoll(3);
                        }
                    }
                }
                else if (choice2 == null)
                    return;
                else
                {
                    EventManager.Wrong();
                    break;
                }
            }
        }
        static void GambleDiceRoll(int coinRank)
        {
            int total = 0;

            List<Dice> dicelist = new List<Dice>   // 주사위 3개를 설정합니다.
            {
                new Dice(1, 6, DiceType.ID, 0),
                new Dice(1, 6, DiceType.ID, 1),
                new Dice(1, 6, DiceType.ID, 2)
            };

            foreach (var dice in dicelist)          // 주사위를 차례대로 굴려
                total += dice.Roll();               // 그 값을 total에 저장합니다

            Random rand = new Random();             // 랜덤을 생성합니다
            double roll = rand.NextDouble();        // roll은 0 ~ 1의 수를 가집니다.

            if (1 <= total && total < 4)
            {
                if (roll <= 0.2)        // T - 2
                    GetRewardT1(coinRank,total);
                else
                    EventManager.Announce(45, "꽝.. 아무 것도 얻지 못했습니다.");
            }

            else if (4 <= total && total < 8)
            {
                if (roll <= 0.4)        // T - 2
                    GetRewardT1(coinRank,total);
                else if (4 < roll && roll <= 0.6) // T - 1
                    GetRewardT2(coinRank, total);
                else
                    EventManager.Announce(45, "꽝.. 아무 것도 얻지 못했습니다.");
            }

            else if (8 <= total && total < 12)
            {
                if (roll <= 0.2)    // T - 2
                    GetRewardT1(coinRank, total);
                else if (0.2 < roll && roll < 0.6)  // T - 1
                    GetRewardT2(coinRank, total);
                else    // T
                    GetRewardT3(coinRank, total);
            }

            else if (12 <= total && total < 18)
            {
                if (roll < 0.6)    // T - 1
                    GetRewardT2(coinRank, total);
                else if (0.6 <= roll && roll <= 1)  // T
                    GetRewardT3(coinRank, total);
            }

            else // 잭팟
            {
                if (roll <= 1)      // T
                    GetRewardT3(coinRank, total);
            }
        }
        static void GetRewardT1(int coinRank, int total) // 티어 1의 아이템 획득
        {
            var character = Character.Instance;
            var t1Items = Item.Instance.Where(item => item.ItemRank == Math.Min(1,coinRank-2)).ToList();

            Random rand = new Random();
            int index = rand.Next(t1Items.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = t1Items[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            character.Gold += 20 * total;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }

        static void GetRewardT2(int coinRank, int total) // 티어 2의 아이템 획득
        {
            var character = Character.Instance;

            var t2Items = Item.Instance.Where(item => item.ItemRank == Math.Min(2,coinRank-1)).ToList();

            Random rand = new Random();
            int index = rand.Next(t2Items.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = t2Items[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            character.Gold += 30 * total;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }

        static void GetRewardT3(int coinRank, int total) // 티어3 아이템 획득
        {
            var character = Character.Instance;
            var t3Items = Item.Instance.Where(item => item.ItemRank == Math.Min(3,coinRank)).ToList();

            Random rand = new Random();
            int index = rand.Next(t3Items.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = t3Items[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            character.Gold += 40 * total;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }
    }
}
