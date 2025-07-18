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
        NotStarted,
        InProgress,
        Completed,
        Rewarded
    }
}


