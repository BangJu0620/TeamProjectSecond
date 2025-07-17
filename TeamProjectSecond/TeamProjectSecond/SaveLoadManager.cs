using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextRPG_Quest_Solution.QuestSystem;


namespace TeamProjectSecond
{
    public class SaveLoadManager
    {
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

        public static bool CheckExistSaveData()
        {
            if (!File.Exists("character.json") || !File.Exists("item.json") || !File.Exists("quest.json")) return true;
            else return false;
        }
    }
}
