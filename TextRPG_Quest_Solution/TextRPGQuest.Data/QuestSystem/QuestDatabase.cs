using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TextRPGQuest.SaveSystem;


namespace TextRPGQuest.QuestSystem
{
    /// <summary>
    /// 퀘스트 데이터를 관리합니다.
    /// AllQuests : 게임의 모든 퀘스트 템플릿
    /// PlayerQuests : 플레이어가 수락한 퀘스트
    /// </summary>
    public static class QuestDatabase
    {
        public static List<Quest> AllQuests = new();
        public static List<Quest> PlayerQuests = new();

        public static void Initialize()
        {
            if (!File.Exists("AllQuests.json"))
            {
                RegisterDefaultQuests();
                SaveAllQuests();
            }
            LoadAllQuests();
            LoadPlayerQuests();
        }

       
        /// <summary>
        /// 기본 퀘스트를 코드에서 등록하는 함수입니다.
        /// </summary>
        public static void RegisterDefaultQuests()
        {
            AllQuests.Add(new Quest(IDManager.GetNextID(), "주사위 슬라임 5마리 처치", "주사위 슬라임을 5마리 잡으세요", QuestCategory.KillMonster, 5, 500, 250));
            AllQuests.Add(new Quest(IDManager.GetNextID(), "쉬움 던전 클리어", "쉬움 던전을 클리어하세요", QuestCategory.ClearDungeon, 1, 1500, 750));
            AllQuests.Add(new Quest(IDManager.GetNextID(), "MP 포션 3개 수집", "MP 포션을 3개 모으세요", QuestCategory.Collect, 3, 500, 100));
        }




        public static void SaveAllQuests()
        {
            var json = JsonSerializer.Serialize(AllQuests, new JsonSerializerOptions { WriteIndented = true });
            SaveManager.Save("AllQuests.json", json);
        }

        public static void SavePlayerQuests()
        {
            var json = JsonSerializer.Serialize(PlayerQuests, new JsonSerializerOptions { WriteIndented = true });
            SaveManager.Save("PlayerQuests.json", json);
        }

        public static void LoadAllQuests()
        {
            var json = SaveManager.Load("AllQuests.json");
            if (!string.IsNullOrEmpty(json))
            { AllQuests = JsonSerializer.Deserialize<List<Quest>>(json); }
            else
            { AllQuests = new List<Quest>(); }
        }

        public static void LoadPlayerQuests()
        {
            var json = SaveManager.Load("PlayerQuests.json");
            if (json != null)
            {
                PlayerQuests = JsonSerializer.Deserialize<List<Quest>>(json);
            }
        }
    }
}
