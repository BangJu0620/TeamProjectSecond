namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 진행 상황을 추적합니다.
    /// </summary>
    public class QuestProgress
    {
        public Quest Quest { get; set; }
        public int CurrentCount { get; set; }

        public QuestProgress(Quest quest)
        {
            Quest = quest;
            CurrentCount = 0;
        }

        public bool IsCompleted(int requiredCount)
        {
            return CurrentCount >= requiredCount;
        }
    }
}

