using TextRPGQuest.QuestSystem;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// [퀘스트 자동 생성 템플릿]
    /// 퀘스트를 추가할 때 아래 형식을 복사해서 사용하세요.
    /// 반드시 QuestDatabase.InitDefaultQuests() 안에서 호출하거나 별도 메서드에서 호출하세요.
    /// </summary>
    public static class QuestTemplate
    {
        public static void AddNewQuest()
        {
            QuestDatabase.Quests.Add(new Quest(
                IDManager.GetNextID(),          // ID 자동 부여
                "퀘스트 제목",                   // 퀘스트 제목
                "퀘스트 설명",                   // 퀘스트 설명
                QuestCategory.Hunt,              // 퀘스트 종류 (Collect, Hunt, Explore 중 선택)
                5,                               // 목표 개수
                100,                             // 보상 골드
                50                               // 보상 경험치
            ));

            QuestDatabase.Save(); // 작성 후 Save를 꼭 호출
        }
    }
}

