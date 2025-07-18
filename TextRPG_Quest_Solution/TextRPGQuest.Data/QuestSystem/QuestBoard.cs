using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPGQuest.QuestSystem;


// 퀘스트 UI



namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 게시판을 통해 퀘스트를 확인하고 수락합니다.
    /// 진행률과 보상도 함께 출력합니다.
    /// </summary>
    public static class QuestBoard
    {
        /// <summary>
        /// 퀘스트 목록을 보여줍니다. 진행률과 보상 정보를 함께 출력합니다.
        /// </summary>
        public static void ShowQuests()
        {
            Console.WriteLine("\n====== [ 퀘스트 목록 ] ======\n");

            foreach (var q in QuestDatabase.Quests)
            {
                // 상태에 따라 색상 출력 (콘솔 꾸미기, Windows에서만 정상작동)
                SetStatusColor(q.Status);

                Console.WriteLine($"[{q.ID}] {q.Title}");
                Console.WriteLine($"설명: {q.Description}");
                Console.WriteLine($"상태: {q.Status}");

                // 진행률 출력
                Console.WriteLine($"진행도: {q.CurrentCount} / {q.GoalCount}");

                // 보상 출력
                Console.WriteLine($"보상: 골드 {q.RewardGold}, 경험치 {q.RewardExp}");
                Console.WriteLine("-------------------------------");

                Console.ResetColor(); // 색상 초기화
            }
        }

        /// <summary>
        /// 퀘스트를 수락합니다.
        /// </summary>
        public static void AcceptQuest(int id)
        {
            var quest = QuestDatabase.Quests.Find(q => q.ID == id);
            if (quest != null && quest.Status == QuestStatus.NotStarted)
            {
                quest.Status = QuestStatus.InProgress;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{quest.Title} 퀘스트를 수락했습니다.");
                Console.ResetColor();
            }
            else if (quest != null)
            {
                Console.WriteLine("이미 진행 중이거나 완료된 퀘스트입니다.");
            }
            else
            {
                Console.WriteLine("해당 ID의 퀘스트를 찾을 수 없습니다.");
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
