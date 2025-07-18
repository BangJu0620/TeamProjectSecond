using TextRPGQuest.QuestSystem;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트의 기본 정보와 상태를 관리합니다.
    /// </summary>
    public class Quest
    {
          public int ID { get; set; }
          public string Title { get; set; }
          public string Description { get; set; }
          public QuestCategory Category { get; set; }
          public QuestStatus Status { get; set; }
          public int GoalCount { get; set; }
          public int CurrentCount { get; set; }
          public int RewardGold { get; set; }
          public int RewardExp { get; set; }

          public Quest() { }
 
           public Quest(int id, string title, string description, QuestCategory category, int goalCount, int rewardGold, int rewardExp)
           {
              ID = id;
              Title = title;
              Description = description;
              Category = category;
              GoalCount = goalCount;
              RewardGold = rewardGold;
              RewardExp = rewardExp;
              Status = QuestStatus.NotStarted;
              CurrentCount = 0;
           }

            /// <summary>
            /// 목표 달성 여부를 체크합니다.
            /// </summary>
          public void CheckComplete()
          {
               if (CurrentCount >= GoalCount)
               {
                   Status = QuestStatus.Completed;
               }
          }
    }
}

