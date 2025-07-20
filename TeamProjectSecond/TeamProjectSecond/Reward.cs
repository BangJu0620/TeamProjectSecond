using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public class Reward
    {
        static void RewardBoard(int stageRank)
        {
            int total = 0;

            EventManager.Clear();
            Console.SetCursorPosition(0, 8);
            EventManager.To(55, "보상을 획득하세요.");
            EventManager.To(45, "주사위 값이 클 수록 좋은 보상을 얻을 확률이 높아집니다.");

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
                    GetRewardTmm(stageRank);
                else
                    EventManager.Announce(45, "꽝.. 아무 것도 얻지 못했습니다.");
            }

            else if (4 <= total && total < 8)
            {
                if (roll <= 0.4)        // T - 2
                    GetRewardTmm(stageRank);
                else if (4 < roll && roll <= 0.6) // T - 1
                    GetRewardTm(stageRank);
                else
                    EventManager.Announce(45, "꽝.. 아무 것도 얻지 못했습니다.");
            }

            else if (8 <= total && total < 12)
            {
                if (roll <= 0.2)    // T - 2
                    GetRewardTmm(stageRank);
                else if (0.2 < roll && roll < 0.6)  // T - 1
                    GetRewardTm(stageRank);
                else    // T
                    GetRewardT(stageRank);
            }

            else if (12 <= total && total < 18)
            {
                if (roll < 0.6)    // T - 1
                    GetRewardTm(stageRank);
                else if (0.6 <= roll && roll <= 1)  // T
                    GetRewardT(stageRank);
            }

            else // 잭팟
            {
                if (roll <= 1)      // T
                    GetRewardT(stageRank);
            }
        }
        static void GetRewardTmm(int stageRank) // 티어 -2의 아이템 획득
        {
            var tmmItems = Item.Instance.Where(item => item.ItemRank == Math.Min(1, (stageRank) - 2)).ToList();

            Random rand = new Random();
            int index = rand.Next(tmmItems.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = tmmItems[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }

        static void GetRewardTm(int stageRank) // 티어 -1의 아이템 획득
        {
            var tmmItems = Item.Instance.Where(item => item.ItemRank == Math.Min(1, (stageRank) - 1)).ToList();

            Random rand = new Random();
            int index = rand.Next(tmmItems.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = tmmItems[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }

        static void GetRewardT(int stageRank) // 해당 티어의 아이템 획득
        {
            var tmmItems = Item.Instance.Where(item => item.ItemRank == Math.Min(1, (Math.Max(3,stageRank)))).ToList();

            Random rand = new Random();
            int index = rand.Next(tmmItems.Count);  // 0부터 (갯수-1) 사이 랜덤 인덱스

            var randomItem = tmmItems[index];
            randomItem.IsOwned = true;
            randomItem.Quantity += 1;
            EventManager.Announce(45, $"{randomItem.ItemName} 아이템을 획득했습니다!");
        }
    }
}

