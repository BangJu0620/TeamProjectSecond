namespace TextRPGQuest.Data
{
    namespace TextRPG.Data
    {
        public class QuestProgress
        {
            public int QuestId { get; set; }
            public int CurrentAmount { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}