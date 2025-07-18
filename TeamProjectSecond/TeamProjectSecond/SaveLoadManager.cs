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
                EventManager.To(42, "1. 세이브\n\n\n\n\n");
                EventManager.To(42, "2. 세이브 삭제\n\n\n\n\n");
                EventManager.To(42, "Enter. 돌아가기");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case null:
                        return;
                    case 1: // 캐릭터, 아이템 정보를 세이브
                        EventManager.Clear();
                        SaveCharacterData("character.json");
                        SaveItemData("item.json");
                        SaveQuestData("quest.json");
                        EventManager.Announce(51, "세이브가 완료되었습니다.");
                        break;
                    case 2: // 캐릭터, 아이템 정보를 로드
                        EventManager.Clear();
                        if (CheckExistSaveData())   // 셋 중 하나라도 없으면 없다고 출력
                        {
                            EventManager.Announce(51, "세이브 파일이 없습니다.");
                            break;
                        }
                        CheckDeleteSaveData();
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
                EventManager.To(43, "1. 세이브 삭제       Enter. 돌아가기");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case null: return;
                    case 1:
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

        public static void SaveQuestData(string filePath)
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

        public static void LoadQuestData(string filePath)
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

        public static bool CheckExistSaveData()
        {
            if (!File.Exists("character.json") || !File.Exists("item.json") || !File.Exists("quest.json")) return true;
            else return false;
        }
    }
}
