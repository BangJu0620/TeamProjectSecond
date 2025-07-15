using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public enum ItemType
    {
        Weapon,     // 무기
        Armor,      // 방어구
        Consumable  // 소모품 (예: 포션)
    }
    // 다른 클래스에서 사용하시려면
    // Console.WriteLine(Item.Instance[0].itemName); > 출력: 천 갑옷
    // 저런 식으로 접근하면 됩니다.
    // 아이템을 수정, 추가하고 싶으시면 static Item(){} 안에 있는 요소들을 수정, 추가하시면 됩니다.
    public class Item
    {
        private static List<ItemData> instance;

        public static List<ItemData> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new List<ItemData>();
                }
                return instance;
            }
        }

        static Item()
        {
            instance = new List<ItemData>();
            //드랍 전용 아이템 = 마지막에 false 추가
            //Armor 종류
            instance.Add(new ItemData("천 갑옷", ItemType.Armor, 0, 3, "얇지만 움직이기 쉬운 천 갑옷입니다.", 700, false, false));
            instance.Add(new ItemData("수련자 갑옷", ItemType.Armor, 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, false, false));
            instance.Add(new ItemData("무쇠갑옷", ItemType.Armor, 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, false, false));
            instance.Add(new ItemData("스파르타의 갑옷", ItemType.Armor, 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, false, false));

            //Weapon 종류
            instance.Add(new ItemData("낡은 검", ItemType.Weapon, 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 600, false, false));
            instance.Add(new ItemData("짧은 단검", ItemType.Weapon, 3, 0, "빠르고 가볍지만 위력이 낮은 단검입니다.", 900, false, false));
            instance.Add(new ItemData("청동 도끼", ItemType.Weapon, 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500, false, false));
            instance.Add(new ItemData("스파르타의 창", ItemType.Weapon, 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700, false, false));

            //consumables 종류
            instance.Add(new ItemData("HP 포션", ItemType.Consumable, 0, 0, "HP를 30 회복시켜주는 포션입니다.", 500, false, false));
            instance.Add(new ItemData("MP 포션", ItemType.Consumable, 0, 0, "MP를 30 회복시켜주는 포션입니다.", 500, false, false));
        }

        // 아이템 획득 로직
        // 활용 예시 Item.AddItem(item.ItemName);
        public static bool AddItem(string itemName, int count)
        {
            var item = Instance.FirstOrDefault(i => i.ItemName == itemName);

            if (item != null)
            {
                item.IsOwned = true;

                if (item.ItemType == ItemType.Consumable)
                {
                    item.Quantity += count;
                    Console.WriteLine($"{itemName}을(를) {count}개 획득했습니다!");
                }
                else
                {
                    Console.WriteLine($"{itemName}을(를) 획득했습니다!");
                }

                return true;
            }
            else
            {
                Console.WriteLine("존재하지 않는 아이템입니다.");
                return false;
            }
        }
    }

    public class ItemData
    {
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        public int ItemAttackPoint { get; set; }
        public int ItemDefensePoint { get; set; }
        public string ItemDescription { get; set; }
        public int ItemPrice { get; set; }
        public bool IsOwned { get; set; }
        public bool IsEquipped { get; set; }
        public int ItemHealHPAmount { get; set; }
        public int ItemHealMPAmount { get; set; }
        public int Quantity { get; set; } = 0; //소모품 갯수
        public bool IsShopItem { get; set; } = true; // 상점 구매 가능 여부 (기본값: 가능)

        public ItemData(string name, ItemType type, int atk, int def, string description, int price, bool owned, bool equipped, bool isShopItem = true)
        {
            ItemName = name;
            ItemType = type;
            ItemAttackPoint = atk;
            ItemDefensePoint = def;
            ItemDescription = description;
            ItemPrice = price;
            IsOwned = owned;
            IsEquipped = equipped;
            IsShopItem = isShopItem;
            ParseHealAmountFromDescription(description);
        }

        //HP, MP 회복 구분
        private void ParseHealAmountFromDescription(string description)
        {
            if (description.Contains("HP"))
            {
                ItemHealHPAmount = ExtractFirstNumber(description);
            }
            else if (description.Contains("MP"))
            {
                ItemHealMPAmount = ExtractFirstNumber(description);
            }
        }

        //회복량 추출
        private int ExtractFirstNumber(string text)
        {
            foreach (var word in text.Split(' '))
            {
                if (int.TryParse(word, out int result))
                {
                    return result;
                }
            }
            return 0;
        }

        //아이템 판매 가격 계산 (구매가 85%)
        public int GetSellPrice()
        {
            return (int)(ItemPrice * 0.85f);
        }

        public override string ToString()
        {
            string stats = ItemType == ItemType.Weapon
                ? $"공격력 +{ItemAttackPoint}"
                : ItemType == ItemType.Armor
                    ? $"방어력 +{ItemDefensePoint}"
                    : "";

            return $"{ItemName} | {stats} | {ItemDescription}";
        }
    }
}
