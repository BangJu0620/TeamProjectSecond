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
        Consumable,  // 소모품 (예: 포션)
        Accessory
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

        public static int GetTotalBonusReroll() //장착템 중 리롤값을 모두 더함
        {
            return Instance
                .Where(i => i.IsOwned && i.IsEquipped)
                .Sum(i => i.BonusReroll);
        }

        static Item()
        {
            instance = new List<ItemData>();
            //드랍 전용 아이템 = 마지막에 false 추가
            //ID, "템이름", 아이템타입, 방, 속도, 최대체력, 최대마나, 주사위 최소 눈, 주사위 갯수, 리롤, "설명", 가격, 보유여부, 착용여부, 스킬, 상점 판매여부
            //consumables
            instance.Add(new ItemData(1, "HP 포션", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "HP 30 회복", "HP를 30 회복시켜주는 포션.", 500, false, false));
            instance.Add(new ItemData(2, "MP 포션", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "MP 30 회복", "MP를 30 회복시켜주는 포션.", 500, false, false));

            instance.Add(new ItemData(50, "방어력 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "방어력 +1", "방어력을 1 증가시켜주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(51, "속도 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "속도 +1", "속도를 1 증가시켜주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(52, "체력 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "최대 체력 +5", "최대 체력을 5 증가시켜주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(53, "마나 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "최대 마나 +5", "최대 마나를 5 증가시켜주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(54, "최소 눈 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "최소 눈 +1", "최소 눈을 1 증가시켜주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(55, "주사위 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "주사위 +1", "주사위 개수를 1개 늘려주는 물약.", 1000, false, false, null, false));
            instance.Add(new ItemData(56, "리롤 증가 물약", ItemType.Consumable, 0, 0, 0, 0, 0, 0, 0, "리롤 +1", "리롤 횟수를 1 증가시켜주는 물약.", 1000, false, false, null, false));

            //Armor
            instance.Add(new ItemData(100, "천 갑옷", ItemType.Armor, 3, 0, 0, 0, 0, 0, 0, "방어력+3", "움직이기 쉬운 천 갑옷.", 700, false, false));
            instance.Add(new ItemData(101, "수련자 갑옷", ItemType.Armor, 5, 0, 0, 0, 0, 0, 0, "방어력+5", "수련에 도움을 주는 갑옷.", 1000, false, false));
            instance.Add(new ItemData(102, "무쇠갑옷", ItemType.Armor, 7, 0, 0, 0, 0, 0, 0, "방어력+7", "무쇠로 만들어져 튼튼한 갑옷.", 1800, false, false));
            instance.Add(new ItemData(103, "전설의 갑옷", ItemType.Armor, 15, 0, 0, 0, 0, 0, 0, "방어력+15", "전설적인 모험가의 갑옷.", 3500, false, false));

            //Weapon
            instance.Add(new ItemData(200, "낡은 검", ItemType.Weapon, 0, 0, 0, 0, 1, 0, 0, "최소 눈+1", "쉽게 볼 수 있는 낡은 검.", 600, false, false));
            instance.Add(new ItemData(201, "짧은 단검", ItemType.Weapon, 0, 0, 0, 0, 2, 0, 0, "최소 눈+2", "빠르고 가볍지만 위력이 낮은 단검.", 900, false, false));
            instance.Add(new ItemData(202, "청동 도끼", ItemType.Weapon, 0, 0, 0, 0, 0, 1, 1, "주사위+1, 리롤+1", "어디선가 사용됐던거 같은 도끼.", 1500, false, false));
            instance.Add(new ItemData(203, "전설의 창", ItemType.Weapon, 0, 0, 0, 0, 1, 1, 0, "주사위+1, 최소 눈+1", "전설적인 모험가의 창.", 2700, false, false));

            //Accessory
            instance.Add(new ItemData(300, "신속의 신발", ItemType.Accessory, 0, 3, 0, 0, 0, 0, 0, "속도+3", "속도를 높여주는 신발.", 800, false, false));
            instance.Add(new ItemData(301, "체력의 반지", ItemType.Accessory, 2, 0, 30, 0, 0, 0, 0, "MaxHP+30", "최대 체력을 높여주는 반지.", 800, false, false));
        }

        // 아이템 획득 로직
        // 활용 예시 Item.AddItem(item.ItemName);
        // Item.AddItem("HP포션", 1, false) <<획득 메세지 스킵
        public static bool AddItem(string itemName, int count = 1, bool showMessage = true)
        {
            var item = Instance.FirstOrDefault(i => i.ItemName == itemName);

            if (item != null)
            {
                item.IsOwned = true;
                item.Quantity += count;

                if (showMessage)
                {
                    Console.WriteLine($"{itemName}을(를) {count}개 획득했습니다!");
                }

                return true;
            }
            else
            {
                if (showMessage)
                {
                    Console.WriteLine("존재하지 않는 아이템입니다.");
                }
                return false;
            }
        }
    }

    public class ItemListData
    {
        public List<ItemData> Items { get; set; } = new List<ItemData>();
    }

    public class ItemData
    {
        // 기본 정보
        public int ID { get; set; }
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        // 스탯
        public int ItemDefensePoint { get; set; }
        public int ItemSpeed { get; set; } = 0;
        public int ItemMaxHP { get; set; } = 0;
        public int ItemMaxMP { get; set; } = 0;
        public int MinDiceValueBonus { get; set; } = 0;
        public int BonusDamageDice { get; set; } = 0;
        public int BonusReroll { get; set; } = 0;
        // 설명 및 효과
        public string ItemEffectDesc { get; set; } = "";
        public string ItemLoreDesc { get; set; } = "";
        // 상점 관련
        public int ItemPrice { get; set; }
        public bool IsOwned { get; set; }
        public bool IsEquipped { get; set; }
        public int Quantity { get; set; } = 0;
        public bool IsShopItem { get; set; } = true;
        // 회복량 및 부여 스킬
        public int ItemHealHPAmount { get; set; }
        public int ItemHealMPAmount { get; set; }
        public string? SkillName { get; set; }


        public ItemData() { }

        public ItemData(int id, string name, ItemType type, int def, int speed = 0, int maxHP = 0, int maxMP = 0, int minDiceBonus = 0, int bonusDD = 0, int bonusReroll = 0,
                        string effect = "", string lore = "", int price = 0, bool owned = false, bool equipped = false, string? skillName = null, bool isShopItem = true)
        {
            ID = id;
            ItemName = name;
            ItemType = type;
            ItemDefensePoint = def;
            ItemSpeed = speed;
            ItemMaxHP = maxHP;
            ItemMaxMP = maxMP;
            MinDiceValueBonus = minDiceBonus;
            BonusDamageDice = bonusDD;
            BonusReroll = bonusReroll;
            ItemEffectDesc = effect;
            ItemLoreDesc = lore;
            ItemPrice = price;
            IsOwned = owned;
            IsEquipped = equipped;
            SkillName = skillName;
            IsShopItem = isShopItem;

            ParseHealAmountFromDescription(effect);
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
            string stats = "";
            if (ItemDefensePoint > 0) stats += $"방어력 +{ItemDefensePoint} ";
            if (ItemSpeed > 0) stats += $"속도 +{ItemSpeed} ";
            if (ItemMaxHP > 0) stats += $"MaxHP +{ItemMaxHP} ";
            if (ItemMaxMP > 0) stats += $"MaxMP +{ItemMaxMP} ";
            return $"{stats}\n{ItemEffectDesc}\n{ItemLoreDesc}";
        }
    }

    //던전.cs 완료 시 활성화
    //public class DropEntry
    //{
    //    public string ItemName { get; set; }
    //    public DungeonDifficulty Difficulty { get; set; } //easy, normal, hard 던전 필요
    //    public int MinStage { get; set; }
    //    public int MaxStage { get; set; }
    //    public float DropChance { get; set; } // 0.0 ~ 1.0 사이
    //}


    //public static class DropTable
    //{
    //    private static Random rand = new();

    //    public static List<DropEntry> Entries = new() //아래에 드랍시킬 아이템 추가, 1.0f = 100%
    //    {
    //        new DropEntry { ItemName = "HP 포션", Difficulty = DungeonDifficulty.Easy, MinStage = 1, MaxStage = 3, DropChance = 0.6f },
    //        new DropEntry { ItemName = "MP 포션", Difficulty = DungeonDifficulty.Easy, MinStage = 1, MaxStage = 3, DropChance = 0.6f },

    //        new DropEntry { ItemName = "체력 증가 물약", Difficulty = DungeonDifficulty.Medium, MinStage = 2, MaxStage = 4, DropChance = 0.3f },
    //        new DropEntry { ItemName = "리롤 증가 물약", Difficulty = DungeonDifficulty.Hard, MinStage = 3, MaxStage = 5, DropChance = 0.2f },
    //    };

    //    public static List<ItemData> RollDrop(DungeonDifficulty difficulty, int stage) //난이도,스테이지번호에 따라 아이템 뽑기
    //    {
    //        List<ItemData> drops = new();

    //        foreach (var entry in Entries) //드랍 가능 조건 탐색
    //        {
    //            if (entry.Difficulty != difficulty) continue; //난이도 일치
    //            if (stage < entry.MinStage || stage > entry.MaxStage) continue; //스테이지 범위 일치

    //            if (rand.NextDouble() < entry.DropChance) //확률 계산(뽑기진행)
    //            {
    //                var item = Item.Instance.FirstOrDefault(i => i.ItemName == entry.ItemName); //DB찾기
    //                if (item != null) drops.Add(item); //drop추가
    //            }
    //        }

    //        return drops;
    //    }
    //}

}
