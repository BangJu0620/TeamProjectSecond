using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Console.WriteLine("저장된 파일이 없습니다.");
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
                Console.WriteLine("저장된 파일이 없습니다.");
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
    }

    public class SaveLoadUI
    {
        public void DisplaySaveLoadUI()
        {
            while (true)
            {
                Console.WriteLine("1. 저장하기");
                Console.WriteLine("2. 불러오기");
                Console.WriteLine("0. 나가기");

                string input = Console.ReadLine();
                int userSelect;
                bool isInt = int.TryParse(input, out userSelect);
                if (isInt)
                {
                    if (userSelect == 0)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (userSelect == 1)
                    {
                        Console.Clear();
                        SaveLoadManager.SaveCharacterData("character.json");
                        SaveLoadManager.SaveItemData("item.json");
                        Console.WriteLine("저장하기 완료");
                    }
                    else if(userSelect == 2)
                    {
                        Console.Clear();
                        SaveLoadManager.LoadCharacterData("character.json");
                        SaveLoadManager.LoadItemData("item.json");
                        Console.WriteLine("불러오기 완료");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
