using System;
using TextRPGQuest;
namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 게시판에서 퀘스트를 확인하고 수락하며, 진행도를 업데이트하고 보상을 지급합니다.
    /// </summary>
    public static class QuestBoard
    {
        /// <summary>
        /// 퀘스트 목록을 보여줍니다. 진행률과 보상 정보를 함께 출력합니다.
        /// </summary>
        public static void ShowQuests()
        {
            Console.WriteLine("\n====== [ 퀘스트 목록 ] ======\n");

            foreach (var q in QuestDatabase.PlayerQuests)
            {
                SetStatusColor(q.Status);

                Console.WriteLine($"[{q.ID}] {q.Title}");
                Console.WriteLine($"설명: {q.Description}");
                Console.WriteLine($"상태: {q.Status}");

                Console.WriteLine($"진행도: {q.CurrentCount} / {q.GoalCount}");
                Console.WriteLine($"보상: 골드 {q.RewardGold}, 경험치 {q.RewardExp}");
                Console.WriteLine("-------------------------------");

                Console.ResetColor();
            }
        }

        /// <summary>
        /// 퀘스트를 수락합니다.
        /// </summary>
        public static void AcceptQuest(int id)
        {
            var quest = QuestDatabase.AllQuests.Find(q => q.ID == id);
            if (quest != null)
            {
                if (QuestDatabase.PlayerQuests.Exists(q => q.ID == id))
                {
                    Console.WriteLine("이미 수락한 퀘스트입니다.");
                    return;
                }

                Quest newQuest = new Quest(quest.ID, quest.Title, quest.Description, quest.Category, quest.GoalCount, quest.RewardGold, quest.RewardExp);
                QuestDatabase.PlayerQuests.Add(newQuest);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{quest.Title} 퀘스트를 수락했습니다.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("해당 ID의 퀘스트를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 퀘스트 진행도를 업데이트하고 완료 여부를 체크합니다.
        /// </summary>
        public static void UpdateQuestProgress(QuestCategory category, int amount)
        {
            foreach (var quest in QuestDatabase.PlayerQuests)
            {
                if (quest.Status == QuestStatus.InProgress && quest.Category == category)
                {
                    quest.CurrentCount += amount;

                    if (quest.CurrentCount >= quest.GoalCount)
                    {
                        quest.CurrentCount = quest.GoalCount;
                        quest.Status = QuestStatus.Completed;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[완료] {quest.Title}");
                        Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// 보상을 지급하고 퀘스트를 Rewarded로 변경합니다.
        /// </summary>
        public static void ReceiveReward(int id)
        {
            var quest = QuestDatabase.PlayerQuests.Find(q => q.ID == id);
            if (quest != null && quest.Status == QuestStatus.Completed)
            {
                Player.Instance.Gold += quest.RewardGold;
                Player.Instance.Exp += quest.RewardExp;
                quest.Status = QuestStatus.Rewarded;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[보상 지급] 골드 {quest.RewardGold}, 경험치 {quest.RewardExp}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("보상을 받을 수 없습니다. 퀘스트가 완료되지 않았습니다.");
            }
        }

        /// <summary>
        /// 상태에 따라 콘솔 색상을 변경합니다.
        /// </summary>
        private static void SetStatusColor(QuestStatus status)
        {
            switch (status)
            {
                case QuestStatus.NotStarted:
                    Console.ForegroundColor = ConsoleColor.Gray; break;
                case QuestStatus.InProgress:
                    Console.ForegroundColor = ConsoleColor.Cyan; break;
                case QuestStatus.Completed:
                    Console.ForegroundColor = ConsoleColor.Green; break;
                case QuestStatus.Rewarded:
                    Console.ForegroundColor = ConsoleColor.DarkGray; break;
            }
        }
    }
}
