using System;
using TextRPGQuest;
using SharedBridge;
using System.Data;
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
            while (true)
            {
                EventBridge.OnClear?.Invoke();
                CheckCurrentCountCollect();

                //Console.WriteLine("\n====== [ 퀘스트 목록 ] ======\n");
                Console.SetCursorPosition(0, 2);
                EventBridge.OnToWithText?.Invoke(58, "퀘 스 트\n\n");

                foreach (var q in QuestDatabase.AllQuests)
                {
                    if (q.CurrentCount >= q.GoalCount && q.Status != QuestStatus.Rewarded)
                    {
                        q.CurrentCount = q.GoalCount;
                        q.Status = QuestStatus.Completed;
                    }
                    SetStatusColor(q.Status);

                    //Console.WriteLine($"[{q.ID}] {q.Title}");
                    EventBridge.OnToWithText?.Invoke(46, $"[{q.ID}] {q.Title}\n");
                    //Console.WriteLine($"설명: {q.Description}");
                    EventBridge.OnToWithText?.Invoke(46, $"설명: {q.Description}\n");
                    //Console.WriteLine($"상태: {q.Status}");
                    EventBridge.OnToWithText?.Invoke(46, $"상태: {q.Status}\n");

                    //Console.WriteLine($"진행도: {q.CurrentCount} / {q.GoalCount}");
                    if(q.Status == QuestStatus.NotStarted)
                    {
                        EventBridge.OnToWithText?.Invoke(46, $"진행도: 0 / {q.GoalCount}\n");
                    }
                    else
                    {
                        EventBridge.OnToWithText?.Invoke(46, $"진행도: {q.CurrentCount} / {q.GoalCount}\n");
                    }
                    //Console.WriteLine($"보상: 골드 {q.RewardGold}, 경험치 {q.RewardExp}");
                    EventBridge.OnToWithText?.Invoke(46, $"보상: 골드 {q.RewardGold}, 경험치 {q.RewardExp}\n");
                    //Console.WriteLine("-------------------------------");
                    Console.ResetColor();

                    EventBridge.OnToWithText?.Invoke(46, "-------------------------------\n");
                }
                Console.SetCursorPosition(0, 24);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                EventBridge.OnToWithText?.Invoke(54, "Enter. 돌아가기");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 26);
                EventBridge.OnToWithText?.Invoke(44, "관리할 퀘스트의 번호를 입력해주세요.\n");
                EventBridge.OnToWithText?.Invoke(44, "▶▶ ");
                Console.ResetColor();
                switch (EventBridge.OnCheckInput?.Invoke())
                {
                    case null:
                        return;
                    case 1:
                        ManageQuest(1);
                        break;
                    case 2:
                        ManageQuest(2);
                        break;
                    case 3:
                        ManageQuest(3);
                        break;
                    default:
                        EventBridge.OnWrong?.Invoke();
                        break;
                }
            }
        }

        public static void ManageQuest(int i)
        {
            var quest = QuestDatabase.AllQuests.Find(q => q.ID == i);
            if (quest.Status == QuestStatus.NotStarted)
            {
                Accept(i);
            }
            else if (quest.Status == QuestStatus.InProgress || quest.Status == QuestStatus.Completed)
            {
                ReceiveReward(i);
            }
            else if (quest.Status == QuestStatus.Rewarded)
            {
                EventBridge.OnAnnounce?.Invoke(49, "이미 완료한 퀘스트입니다.");
            }
        }

        public static void CheckCurrentCountCollect()
        {
            var mpPotionCount = EventBridge.OnGetMpPotionCount();
            QuestDatabase.AllQuests[2].CurrentCount = mpPotionCount;
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

        public static void Accept(int id)
        {
            var quest = QuestDatabase.AllQuests.Find(q => q.ID == id);
            if (quest != null)
            {
                Quest newQuest = new Quest(quest.ID, quest.Title, quest.Description, quest.Category, quest.GoalCount, quest.RewardGold, quest.RewardExp);
                QuestDatabase.PlayerQuests.Add(newQuest);
                quest.Status = QuestStatus.InProgress;
                Console.ForegroundColor = ConsoleColor.Cyan;
                EventBridge.OnAnnounce?.Invoke(44, $"{quest.Title} 퀘스트를 수락했습니다.");
                Console.ResetColor();
            }
            else
            {
                EventBridge.OnAnnounce?.Invoke(44, "해당 ID의 퀘스트를 찾을 수 없습니다.");
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
            var quest = QuestDatabase.AllQuests.Find(q => q.ID == id);
            if (quest != null && quest.Status == QuestStatus.Completed)
            {
                //Player.Instance.Gold += quest.RewardGold;
                EventBridge.OnAddGold?.Invoke(quest.RewardGold);
                //Player.Instance.Exp += quest.RewardExp;
                EventBridge.OnAddExp?.Invoke(quest.RewardExp);
                quest.Status = QuestStatus.Rewarded;

                Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"[보상 지급] 골드 {quest.RewardGold}, 경험치 {quest.RewardExp}");
                EventBridge.OnAnnounce(45, $"[보상 지급] 골드 {quest.RewardGold}, 경험치 {quest.RewardExp}");
                Console.ResetColor();
            }
            else
            {
                //Console.WriteLine("보상을 받을 수 없습니다. 퀘스트가 완료되지 않았습니다.");
                EventBridge.OnAnnounce(34, "보상을 받을 수 없습니다. 퀘스트가 완료되지 않았습니다.");
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
