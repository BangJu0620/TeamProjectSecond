
//퀘스트 정의

namespace TextRPG_Quest_Solution.QuestSystem
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public QuestCategory Category { get; set; }
        public int TargetCount { get; set; }
        public int CurrentCount { get; set; }
        public QuestStatus Status { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }

        public Quest() { }

        public Quest(int id, string title, string description, QuestCategory category, int targetCount, int gold, int exp)
        {
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            TargetCount = targetCount;
            RewardGold = gold;
            RewardExp = exp;
            Status = QuestStatus.NotAccepted;
        }

        public void Progress(int count)
        {
            if (Status != QuestStatus.InProgress) return;

            CurrentCount += count;
            if (CurrentCount >= TargetCount)
            {
                CurrentCount = TargetCount;
                Status = QuestStatus.Completed;
            }
        }
    }
}
