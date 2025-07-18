

namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 데이터 클래스입니다.
    /// </summary>
    public class Quest
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public QuestCategory Category { get; set; }
        public int GoalCount { get; set; }
        public int CurrentCount { get; set; } = 0;
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }
        public QuestStatus Status { get; set; } = QuestStatus.NotStarted;

        public Quest() { }

        public Quest(int id, string title, string desc, QuestCategory category, int goal, int gold, int exp)
        {
            ID = id;
            Title = title;
            Description = desc;
            Category = category;
            GoalCount = goal;
            CurrentCount = 0;
            RewardGold = gold;
            RewardExp = exp;
            Status = QuestStatus.NotStarted;
        }
    }
}
