using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using TextRPGQuest.QuestSystem;


//퀘스트 등록 / 저장



namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 목록을 관리하고 저장/불러오기를 담당합니다.
    /// </summary>
    public static class QuestDatabase
    {

        public static List<Quest> Quests = new List<Quest>();
        private static string FilePath = "TextRPGQuest.json";

        /// <summary>
        /// 퀘스트를 불러옵니다. 파일이 없으면 기본 퀘스트 생성.
        /// </summary>
        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                Quests = JsonSerializer.Deserialize<List<Quest>>(json);
            }
        }

        public static List<Quest> AllQuests = new();
        

        public static void Register()
        {
            AllQuests.Add(new Quest(IDManager.GetNextID(), "슬라임 5마리 처치", "슬라임을 5마리 잡으세요", QuestCategory.KillMonster, 5, 100, 50));
            AllQuests.Add(new Quest(IDManager.GetNextID(), "던전 1층 클리어", "던전 1층을 클리어하세요", QuestCategory.ClearDungeon, 1, 300, 150));
        }

        public static void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(AllQuests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                AllQuests = JsonSerializer.Deserialize<List<Quest>>(json);

            }
            else
            {
                InitDefaultQuests();
                Save();
            }
        }

        /// <summary>
        /// 퀘스트를 저장합니다.
        /// </summary>
        public static void Save()
        {
            string json = JsonSerializer.Serialize(Quests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }


        private static void InitDefaultQuests()
        {
            Quests.Add(new Quest(IDManager.GetNextID(), "약초 3개 수집", "약초를 3개 모으세요.", QuestCategory.Collect, 3, 100, 20));
            Quests.Add(new Quest(IDManager.GetNextID(), "슬라임 5마리 처치", "슬라임을 5마리 처치하세요.", QuestCategory.Hunt, 5, 150, 30));
            Quests.Add(new Quest(IDManager.GetNextID(), "초급 던전 탐험", "던전을 1회 클리어하세요.", QuestCategory.Explore, 1, 200, 50));
        }
    }
}
