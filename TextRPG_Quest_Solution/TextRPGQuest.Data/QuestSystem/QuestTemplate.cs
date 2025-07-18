using TextRPGQuest.QuestSystem;

namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// [퀘스트 자동 생성 템플릿]
    /// 퀘스트를 추가할 때 아래 형식을 복사해서 사용하세요.
    /// 반드시 QuestDatabase.RegisterDefaultQuests() 안에서 호출하거나 별도 메서드에서 호출하세요.
    /// </summary>
    public static class QuestTemplate
    {
        /// <summary>
        /// 기본 퀘스트 템플릿들을 한꺼번에 등록합니다.
        /// 필요 시 이 메서드 내에 퀘스트 추가 구문을 복사/붙여넣기 하여 작성하세요.
        /// </summary>
        public static void RegisterAllTemplates()
        {
            AddQuest(
                "슬라임 5마리 처치",
                "슬라임을 5마리 잡으세요",
                QuestCategory.KillMonster,
                5,
                100,
                50
            );

            AddQuest(
                "던전 1층 클리어",
                "던전 1층을 클리어하세요",
                QuestCategory.ClearDungeon,
                1,
                300,
                150
            );

            AddQuest(
                "약초 3개 수집",
                "약초를 3개 모으세요",
                QuestCategory.Collect,
                3,
                100,
                20
            );

            QuestDatabase.SaveAllQuests(); // 변경사항 저장
        }

        /// <summary>
        /// 새로운 퀘스트를 생성하여 QuestDatabase.AllQuests에 추가합니다.
        /// </summary>
        /// <param name="title">퀘스트 제목</param>
        /// <param name="description">퀘스트 설명</param>
        /// <param name="category">퀘스트 종류 (Collect, KillMonster, ClearDungeon 등)</param>
        /// <param name="goalCount">목표 수량</param>
        /// <param name="rewardGold">골드 보상</param>
        /// <param name="rewardExp">경험치 보상</param>
        private static void AddQuest(string title, string description, QuestCategory category, int goalCount, int rewardGold, int rewardExp)
        {
            QuestDatabase.AllQuests.Add(new Quest(
                IDManager.GetNextID(),
                title,
                description,
                category,
                goalCount,
                rewardGold,
                rewardExp
            ));
        }
    }
}

