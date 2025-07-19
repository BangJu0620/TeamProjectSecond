using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextRPGQuest.QuestSystem;


namespace TeamProjectSecond
{
    public class SaveLoadManager
    {
        public static void DisplaySaveUI()
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.To(58, "세 이 브\n\n");
                EventManager.To(41, "이곳에서 플레이 데이터를 관리할 수 있습니다.\n\n\n\n\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                EventManager.To(42, "1. 세이브\n\n\n\n\n");
                EventManager.To(42, "2. 세이브 삭제\n\n\n\n\n");
                EventManager.To(42, "Enter. 돌아가기");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case null:
                        return;
                    case 1: // 캐릭터, 아이템, 퀘스트 정보를 세이브
                        if (!CheckExistSaveData())  // 세이브 파일이 존재하는지 확인
                        {
                            ConfirmOverwriteSaveData();
                        }
                        else
                        {
                            SaveLoadData.SaveAllData("save.json");
                            EventManager.Announce(51, "세이브가 완료되었습니다.");
                        }
                        break;
                    case 2: // 세이브 정보를 삭제
                        if (CheckExistSaveData())   // 없으면 없다고 출력, 이후 세이브 화면으로 돌아감
                        {
                            EventManager.Announce(51, "세이브 파일이 없습니다.");
                            break;
                        }
                        CheckDeleteSaveData();  // 삭제할건지 재차 확인
                        break;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void ConfirmOverwriteSaveData()   // 세이브 덮어씌울지 확인
        {
            EventManager.Clear();
            EventManager.To(31, "세이브파일이 존재해 세이브를 할 경우 기존 데이터는 삭제됩니다.\n\n\n\n");
            EventManager.To(50, "정말로 세이브하시겠습니까?\n\n\n");

            Console.SetCursorPosition(0, 24);
            Console.ForegroundColor = ConsoleColor.White;
            EventManager.To(43, "1. 세이브           Enter. 돌아가기");
            EventManager.Select();

            switch (EventManager.CheckInput())
            {
                case null: return;  // Enter 입력시 돌아감
                case 1: // 1 입력시 세이브
                    SaveLoadData.SaveAllData("save.json");
                    EventManager.Announce(50, "세이브가 완료되었습니다.");
                    return;
                default:
                    EventManager.Wrong();
                    break;
            }
        }

        public static void CheckDeleteSaveData()    // 세이브 삭제할건지 확인
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.To(55, "세이브 삭제\n\n\n\n");
                EventManager.To(49, "정말로 삭제하시겠습니까?\n\n\n");

                Console.SetCursorPosition(0, 24);
                Console.ForegroundColor = ConsoleColor.White;
                EventManager.To(43, "1. 세이브 삭제       Enter. 돌아가기");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case null: return;  // Enter 입력시 돌아감
                    case 1: // 1 입력시 세이브 삭제
                        File.Delete("save.json");
                        EventManager.Announce(50, "세이브가 삭제되었습니다.");
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static bool CheckExistSaveData() // true 면 없는 상태, false 면 있는 상태
        {
            if (!File.Exists("save.json")) return true;
            else return false;
        }

        public static bool CheckEmptySaveData() // 비어있으면 true, 있으면 false
        {
            string json = File.ReadAllText("save.json");
            if (string.IsNullOrWhiteSpace(json)) return true;

            var saveData = JsonSerializer.Deserialize<SaveLoadData>(json);
            if (saveData == null) return true;
            return false;
        }
    }

    public class SaveLoadData
    {
        public CharacterData CharacterData { get; set; }
        public ItemListData ItemListData { get; set; }
        public List<Quest> Quests { get; set; }

        public static void SaveAllData(string filePath)
        {
            SaveLoadData saveLoadData = new SaveLoadData
            {
                CharacterData = Character.Instance.ToData(),
                ItemListData = new ItemListData { Items = Item.Instance },
                Quests = QuestDatabase.AllQuests
            };

            string json = JsonSerializer.Serialize(saveLoadData, new JsonSerializerOptions{ WriteIndented = true});
            File.WriteAllText(filePath, json);
        }

        public static void LoadAllData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var saveLoadData = JsonSerializer.Deserialize<SaveLoadData>(json);
            Character.Instance.LoadFromData(saveLoadData.CharacterData);
            Item.Instance.Clear();
            Item.Instance.AddRange(saveLoadData.ItemListData.Items);
            QuestDatabase.AllQuests.Clear();
            QuestDatabase.AllQuests.AddRange(saveLoadData.Quests);
        }
    }
}
