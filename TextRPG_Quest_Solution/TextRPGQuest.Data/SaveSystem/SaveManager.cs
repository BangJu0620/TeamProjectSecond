using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TextRPGQuest.SaveSystem
{
    using TextRPGQuest.QuestSystem;
    using TextRPGQuest.PlayerSystem;

    /// <summary>
    /// 게임의 저장과 불러오기를 관리합니다.
    /// </summary>
    public static class SaveManager
    {
        public static void Save(Player player)
        {
            QuestDatabase.Save();
            player.Save();
            Console.WriteLine("게임이 저장되었습니다.");
        }

        public static void Load(Player player)
        {
            QuestDatabase.Load();
            player.Load();
            Console.WriteLine("게임이 불러와졌습니다.");
        }
    }
}



