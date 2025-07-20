using System;
using System.IO;
using System.Text.Json;

namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 유일한 ID를 자동으로 관리합니다. 저장/불러오기 가능.
    /// </summary>
    public static class IDManager
    {
        private static int currentID = 0;
        private static string FilePath = "IDManager.json";

        public static int GetNextID()
        {
            currentID++;
            Save();  // ID가 변경될 때마다 바로 저장해서 중복 방지 안정성 강화
            return currentID;
        }

        public static void Save()
        {
            var json = JsonSerializer.Serialize(currentID);
            File.WriteAllText(FilePath, json);
        }

        //public static void Load()
        //{
        //    if (File.Exists(FilePath))
        //    {
        //        var json = File.ReadAllText(FilePath);
        //        currentID = JsonSerializer.Deserialize<int>(json);
        //    }
        //    else
        //    {
        //        currentID = 0;
        //    }
        //}
    }
}




