using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using TextRPG_Quest_Solution.QuestSystem;


// 저장/ 불러오기

namespace TextRPG_Quest_Solution.SaveSystem
{
    public static class SaveManager
    {
        public static void SaveAll(Player player)
        {
            QuestDatabase.Save();
            var json = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("PlayerData.json", json);
        }

        public static Player LoadPlayer()
        {
            if (File.Exists("PlayerData.json"))
            {
                var json = File.ReadAllText("PlayerData.json");
                return JsonSerializer.Deserialize<Player>(json);
            }
            return new Player("이름없는 모험가");
        }
    }
}

