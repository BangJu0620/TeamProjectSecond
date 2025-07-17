using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


// 퀘스트 UI



namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 게시판을 통해 퀘스트를 확인하고 수락합니다.
    /// </summary>
    public static class QuestBoard
    {
        public static void ShowQuests()
        {
            foreach (var q in QuestDatabase.Quests)
            {
                Console.WriteLine($"{q.ID}. {q.Title} - {q.Description} [상태: {q.Status}]");
            }
        }

        public static void AcceptQuest(int id)
        {
            var quest = QuestDatabase.Quests.Find(q => q.ID == id);
            if (quest != null && quest.Status == QuestStatus.NotStarted)
            {
                quest.Status = QuestStatus.InProgress;
                Console.WriteLine($"{quest.Title} 퀘스트를 수락했습니다.");
            }
        }
    }
}
