using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TextRPGQuest.Data.QuestSystem;
using TextRPGQuest.Data.TextRPG.Data;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;


//퀘스트 등록 / 저장

namespace TextRPG_Quest_Solution.QuestSystem
{
    public static class QuestDatabase
    {
        public static List<Quest> AllQuests = new();

        public static void Register()
        {
            AllQuests.Add(new Quest(IDManager.Generate(), "슬라임 5마리 처치", "슬라임을 5마리 잡으세요", QuestCategory.KillMonster, 5, 100, 50));
            AllQuests.Add(new Quest(IDManager.Generate(), "던전 1층 클리어", "던전 1층을 클리어하세요", QuestCategory.ClearDungeon, 1, 300, 150));
        }

        public static void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(AllQuests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static bool Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                AllQuests = JsonSerializer.Deserialize<List<Quest>>(json);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

