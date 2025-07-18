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
                        EventManager.Clear();
                        SaveCharacterData("character.json");
                        SaveItemData("item.json");
                        SaveQuestData("quest.json");
                        EventManager.Announce(51, "세이브가 완료되었습니다.");
                        break;
                    case 2: // 캐릭터, 아이템, 퀘스트 정보를 삭제
                        EventManager.Clear();
                        if (CheckExistSaveData())   // 셋 중 하나라도 없으면 없다고 출력, 이후 세이브 화면으로 돌아감
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

        public static void CheckDeleteSaveData()
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
                        EventManager.Clear();
                        File.Delete("character.json");
                        File.Delete("item.json");
                        File.Delete("quest.json");
                        EventManager.Announce(50, "세이브가 삭제되었습니다.");
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void SaveCharacterData(string filePath) // 캐릭터 데이터 저장
        {
            CharacterData characterData = Character.Instance.ToData();
            string json = JsonSerializer.Serialize(characterData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void SaveItemData(string filePath) // 아이템 데이터 저장
        {
            var itemListData = new ItemListData
            {
                Items = Item.Instance
            };

            string json = JsonSerializer.Serialize(itemListData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void SaveQuestData(string filePath)   // 퀘스트 데이터 저장
        {
            string json = JsonSerializer.Serialize(QuestDatabase.AllQuests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void LoadCharacterData(string filePath) // 캐릭터 데이터 불러오기
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            string json = File.ReadAllText(filePath);
            CharacterData loadedCharacterData = JsonSerializer.Deserialize<CharacterData>(json);

            if(loadedCharacterData != null)
            {
                Character.Instance.LoadFromData(loadedCharacterData);
            }
        }

        public static void LoadItemData(string filePath) // 아이템 데이터 불러오기
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            string json = File.ReadAllText(filePath);
            ItemListData loadedItemListData = JsonSerializer.Deserialize<ItemListData>(json);

            if(loadedItemListData != null)
            {
                Item.Instance.Clear();
                Item.Instance.AddRange(loadedItemListData.Items);
            }
        }

        public static void LoadQuestData(string filePath)   // 퀘스트 데이터 불러오기
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            string json = File.ReadAllText(filePath);
            var loadedQuestData = JsonSerializer.Deserialize<List<Quest>>(json);

            if(loadedQuestData != null)
            {
                QuestDatabase.AllQuests.Clear();
                QuestDatabase.AllQuests.AddRange(loadedQuestData);
            }
        }

        public static bool CheckExistSaveData() // 세이브 파일 3개 있는지 확인용
        {
            if (!File.Exists("character.json") || !File.Exists("item.json") || !File.Exists("quest.json")) return true;
            else return false;
        }
    }
}
