using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TextRPGQuest.QuestSystem;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트의 종류를 구분합니다.
    /// </summary>
    public enum QuestCategory
    {
        Collect, // 아이템 수집
        Hunt, // 몬스터 사냥
        Explore, // 던전 탐험
        KillMonster,   // 몬스터 처치
        ClearDungeon   // 던전 클리어

    }
}


