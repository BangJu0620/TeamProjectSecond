using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGQuest.QuestSystem;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트의 상태를 나타냅니다.
    /// </summary>
    public enum QuestStatus
    {
        NotStarted, // 시작하지 않음
        InProgress, // 진행 중
        Completed, // 완료됨
        Rewarded // 보상 받음
    }
}


