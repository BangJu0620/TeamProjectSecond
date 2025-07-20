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
        public static void RewardBoard(Dungeon dungeon,int stageRank)
        {
            var character = Character.Instance;

        GetExp(stageRank);
            if (stageRank <= 4)
            {
                character.NaeBaeCampCoin++;
                EventManager.Announce(50, "내배캠 코인을 1개 획득했습니다.");
            }
            else if (4 < stageRank && stageRank <= 6)
            {
                character.SpartaCoin++;
                EventManager.Announce(50, "스파르타 코인을 1개 획득했습니다.");
            }
            else if (6 < stageRank && stageRank <= 8)
            {
                character.BitCoin++;
                EventManager.Announce(50, "비트 코인을 1개 획득했습니다.");
            }
            else if (8 < stageRank && stageRank < 10)
            {
                character.BitCoin += 2;
                EventManager.Announce(50, "비트 코인을 2개 획득했습니다.");
            }
            EventManager.Clear();
            Console.SetCursorPosition(0, 12);
            EventManager.To(55,$"남은 계층 : {dungeon.Stages.Count}\n\n");
            EventManager.To(55, $"현재 도달 계층 : {dungeon.CurrentStageIndex}\n\n\n");
            EventManager.To(55, "1. 진행하기\n\n");
            EventManager.To(55, "2. 스킬사용\n\n");
            EventManager.To(55, "3. 포션사용\n\n");
            EventManager.To(55, "4. 포기하기");
            EventManager.Select();

            switch (EventManager.CheckInput())
            {
                case 1:
                    dungeon.ProceedToNextStage();
                    break;
                case 2:

                    break;
                case 3:
                    Inventory.UsePotionFlow();
                    break;
                case 4:
                    return;
                default:
                    EventManager.Wrong();
                    break;
            }
        }

        static void GetExp(int stageRank)
        {
            var character = Character.Instance;
            int ExpGain = stageRank * stageRank * 10;
            character.Exp += ExpGain;
        }
    }
}