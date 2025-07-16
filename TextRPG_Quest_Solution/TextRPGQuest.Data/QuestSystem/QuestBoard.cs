using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPGQuest.Data.TextRPG;
using TextRPGQuest.Data.QuestSystem;
using TextRPGQuest.Data.TextRPG.Data;


// 퀘스트 UI


namespace TextRPG_Quest_Solution.QuestSystem
{
    public static class QuestBoard
    {
        public static void Show(Player player)
        {
            Console.WriteLine("\n[퀘스트 게시판]");
            foreach (var q in QuestDatabase.AllQuests)
            {
                Console.WriteLine($"{q.Id}. {q.Title} - [{q.Status}]");
            }

            Console.Write("수락할 퀘스트 ID 입력 (0: 나가기): ");
            if (int.TryParse(Console.ReadLine(), out int id) && id != 0)
            {
                var quest = QuestDatabase.AllQuests.FirstOrDefault(q => q.Id == id);
                if (quest != null && quest.Status == QuestStatus.NotAccepted)
                {
                    quest.Status = QuestStatus.InProgress;
                    Console.WriteLine($"'{quest.Title}' 퀘스트를 수락했습니다!");
                }
            }
        }

        public static void ReportProgress(Player player, int monsterKill = 0, int dungeonClear = 0)
        {
            foreach (var quest in QuestDatabase.AllQuests.Where(q => q.Status == QuestStatus.InProgress))
            {
                if (quest.Category == QuestCategory.KillMonster)
                    quest.Progress(monsterKill);
                else if (quest.Category == QuestCategory.ClearDungeon)
                    quest.Progress(dungeonClear);

                if (quest.Status == QuestStatus.Completed)
                {
                    player.ReceiveReward(quest.RewardGold, quest.RewardExp);
                    Console.WriteLine($"'{quest.Title}' 완료! 보상 획득!");
                }
            }
        }
    }
}

