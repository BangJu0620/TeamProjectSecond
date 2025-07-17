using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// ID를 자동으로 관리합니다.
    /// 퀘스트 제작 시 IDManager.GetNextID()를 사용하세요.
    /// </summary>
    public static class IDManager
    {
        private static int currentID = 0;

        public static int GetNextID()
        {
            return ++currentID;
        }
    }
}



