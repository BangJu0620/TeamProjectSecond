using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//열거형 타입 관리

namespace TextRPG_Quest_Solution.QuestSystem
{
    public enum QuestCategory { KillMonster, ClearDungeon }
    public enum QuestStatus { NotAccepted, InProgress, Completed }
}
