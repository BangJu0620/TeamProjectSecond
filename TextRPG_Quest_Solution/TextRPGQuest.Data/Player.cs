using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//퀘스트 보상 지급 스크립트

namespace TextRPGQuest.PlayerSystem
{
    using System.Text.Json;

    /// <summary>
    /// 플레이어의 데이터(골드, 경험치 등)를 관리합니다.
    /// </summary>
    public class Player
    {
        public int Gold { get; set; }
        public int Exp { get; set; }

        public void AddGold(int amount) => Gold += amount;
        public void AddExp(int amount) => Exp += amount;

        private const string FilePath = "PlayerData.json";

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public void Load()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonSerializer.Deserialize<Player>(json);
                Gold = data.Gold;
                Exp = data.Exp;
            }
        }
    }
}




