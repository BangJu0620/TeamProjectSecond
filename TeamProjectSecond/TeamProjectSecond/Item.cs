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

        static Item()
        {
            instance = new List<ItemData>();
            //드랍 전용 아이템 = 마지막에 false 추가
            //ID, "템이름", 아이템타입, 방어력, 속도, 최대체력, 최대마나, 데미지 배율, 데미지 보너스, "설명", "설정설명", 가격, 보유여부, 착용여부, 스킬, 상점 판매여부
            //(소모품) ID, "템이름", "설명", "설정설명", 가격, 보유여부, 착용여부, 상점 판매여부. 아이템 랭크 (0이면 드랍안함)

            //Weapon
            instance.Add(new ItemData(100, "주사위 뿅망치", ItemType.Weapon, 0, 0, 0, 0, 1.02f, 0, 0, "데미지+2%", "주사위 모양의 뿅망치.", 600, false, false));
            instance.Add(new ItemData(101, "구속의 삼단봉", ItemType.Weapon, 0, 0, 0, 0, 1.05f, 0, 0, "데미지+5%", "도박 경찰이 내기에서 잃은 경찰봉.", 900, false, false));
            instance.Add(new ItemData(102, "마술 단검", ItemType.Weapon, 0, 0, 0, 0, 1.03f, 1, 0, "데미지+3%, 추가데미지+1", "단검처럼 보이는 주사위.", 1500, false, false));
            instance.Add(new ItemData(103, "도박사의 지팡이", ItemType.Weapon, 0, 0, 0, 0, 1, 10, 0, "추가데미지+10", "주사위를 생성하는 지팡이.", 2700, false, false));
            instance.Add(new ItemData(104, "야구방망이", ItemType.Weapon, 0, 0, 0, 0, 1.05f, 5, 0, "데미지+5%, 추가데미지+5", "경비원이 사용하는 빠따.", 3000, false, false));
            instance.Add(new ItemData(105, "불운의 단검", ItemType.Weapon, 0, 0, -20, 0, 1.07f, 0, 0, "MaxHP-20, 데미지+7%", "운 좋은 자만이 다룰 수 있다.", 3000, false, false));
            instance.Add(new ItemData(106, "배팅 해머", ItemType.Weapon, 0, 0, 0, 0, 1.1f, 0, -1, "데미지+10%, 리롤-1", "인생은 한 방.", 3000, false, false));
            instance.Add(new ItemData(107, "추심자의 완드", ItemType.Weapon, 0, 0, 0, 0, 1, 10, 1, "추가데미지+10, 리롤+1", "놓치지 않는다.", 5000, false, false));

            //Armor
            instance.Add(new ItemData(200, "카지노 조끼", ItemType.Armor, 3, 0, 0, 0, 1, 0, 0, "방어력+3", "칩 넣는 주머니가 많다.", 700, false, false));
            instance.Add(new ItemData(201, "찢어진 경찰복", ItemType.Armor, 5, 0, 0, 0, 1, 0, 0, "방어력+5", "도박 경찰의 경찰복.", 1000, false, false));
            instance.Add(new ItemData(202, "딜러의 셔츠", ItemType.Armor, 7, 0, 0, 0, 1, 0, 0, "방어력+7", "건실한 사람들이 자주 입는다.", 1800, false, false));
            instance.Add(new ItemData(203, "VIP의 갑옷", ItemType.Armor, 15, 0, 0, 0, 1, 0, 0, "방어력+15", "어딘가의 VIP가 입는 갑옷.", 3500, false, false));
            instance.Add(new ItemData(204, "보석상의 정장", ItemType.Armor, 13, 2, 0, 0, 1, 0, 0, "방어력+13, 속도+2", "보석을 지킨다.", 3000, false, false));
            instance.Add(new ItemData(205, "회계사의 조끼", ItemType.Armor, 8, 0, 0, 20, 1, 0, 0, "방어력+8, MaxMP+20", "손실을 줄여야한다.", 2000, false, false));
            instance.Add(new ItemData(206, "세무관의 셔츠", ItemType.Armor, 10, 0, 0, 0, 1, -3, 0, "방어력+10, 추가데미지-3", "데미지 세금을 걷겠다.", 1500, false, false));
            instance.Add(new ItemData(207, "탈세자의 후드", ItemType.Armor, 3, 3, 0, 0, 1, 0, 0, "방어력+3, 속도+3", "탈세자가 사용한 후드.", 1500, false, false));

            //Accessory
            instance.Add(new ItemData(300, "도망자의 신발", ItemType.Accessory, 0, 3, 0, 0, 1, 0, 0, "속도+3", "속도를 높여주는 신발.", 800, false, false));
            instance.Add(new ItemData(301, "체력의 반지", ItemType.Accessory, 2, 0, 30, 0, 1, 0, 0, "방어력+2, MaxHP+30", "밤새 게임할 수 있게 된다.", 800, false, false));
            instance.Add(new ItemData(302, "파산자의 목걸이", ItemType.Accessory, -2, 0, 0, 0, 1, 5, 0, "방어력-2, 추가데미지+5", "따서 갚는다.", 800, false, false, null, false));
            instance.Add(new ItemData(303, "경찰 배지", ItemType.Accessory, 0, 0, 0, 50, 1, 3, 0, "MaxMP+50, 추가데미지+3", "이거까지 내기에 걸어?", 800, false, false, null, false));
            instance.Add(new ItemData(304, "행운의 칩", ItemType.Accessory, 0, 0, 0, 0, 1, 0, 1, "리롤+1", "일이 잘 풀릴 것 같다.", 800, false, false, null, false));
            instance.Add(new ItemData(305, "딜러의 카드덱", ItemType.Accessory, 0, 0, 0, 0, 1.04f, 0, 0, "데미지+4%", "딜러의 카드덱.", 800, false, false, null, false));
            instance.Add(new ItemData(306, "결혼 반지", ItemType.Accessory, 0, 0, 50, 0, 1, -6, 0, "MaxHP+50, 추가데미지-6", "누군가 내기로 건 반지.", 800, false, false, null, false));
            instance.Add(new ItemData(307, "무전기 이어폰", ItemType.Accessory, 0, 0, 0, 0, 1, 5, 0, "추가데미지+5", "도둑이 훔쳐간 이어폰.", 800, false, false, null, false));
            instance.Add(new ItemData(308, "마법사의 모자", ItemType.Accessory, 0, 0, 0, 30, 1, 5, 0, "MaxMP+30, 추가데미지+5", "마술 하나 보여줄까?", 800, false, false, null, false));
            instance.Add(new ItemData(309, "카지노 칩", ItemType.Accessory, 0, 0, 0, 0, 1, 0, 1, "리롤+1", "현금 대신 사용하는 칩.", 800, false, false, null, false));
            instance.Add(new ItemData(310, "괴도의 가면", ItemType.Accessory, 0, 3, 0, 20, 1, 0, 0, "속도+3, MaxMP+20", "단골 손님의 가면.", 800, false, false, null, false));
            instance.Add(new ItemData(311, "선글라스", ItemType.Accessory, 0, 0, 0, 0, 1, 7, 0, "추가데미지+7", "카리스마를 올려준다.", 800, false, false, null, false));
            instance.Add(new ItemData(312, "머니건", ItemType.Accessory, 0, 0, 0, 0, 1.02f, 4, 0, "데미지+2%, 추가데미지+4", "가폐를 날리는 장난감.", 800, false, false, null, false));
            instance.Add(new ItemData(313, "플레잉 카드", ItemType.Accessory, 0, 0, 0, 0, 1, 1, 1, "추가데미지+1, 리롤+1", "플레잉 카드.", 800, false, false, null, false));

            //consumables
            instance.Add(new ItemData(400, "HP 포션", "HP 30 회복", "HP를 30 회복시켜주는 포션.", 500, false, false, true, 1));
            instance.Add(new ItemData(401, "MP 포션", "MP 30 회복", "MP를 30 회복시켜주는 포션.", 500, false, false, true, 1));

            instance.Add(new ItemData(450, "수호의 영약", "방어력 +1", "방어력을 1 증가시켜주는 물약.", 1000, false, false, false,2));
            instance.Add(new ItemData(451, "신속의 영약", "속도 +1", "속도를 1 증가시켜주는 물약.", 1000, false, false, false,2));
            instance.Add(new ItemData(452, "강건의 영약", "MaxHP +5", "MaxHP를 5 증가시켜주는 물약.", 1000, false, false, false, 2));
            instance.Add(new ItemData(453, "정신의 영약", "MaxMP +5", "MaxMP를 5 증가시켜주는 물약.", 1000, false, false, false, 2));
            instance.Add(new ItemData(454, "힘의 영약", "데미지 +5%", "데미지를 5% 증가시켜주는 물약.", 1000, false, false, false, 2));
            instance.Add(new ItemData(455, "강타의 영약", "추가데미지 +1", "추가데미지를 1 증가시켜주는 물약.", 1000, false, false, false, 2));
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
                    EventManager.Announce(45,$"{itemName}을(를) {count}개 획득했습니다!");
                }

                return true;
            }
            else
            {
                if (showMessage)
                {
                    EventManager.Announce(45,"존재하지 않는 아이템입니다.");
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
        public float DamageMultiplier { get; set; } = 1.0f; // 배율 (예: 1.2f)
        public int DamageBonus { get; set; } = 0; // 고정 추가 데미지
        public int RerollBonus { get; set; } = 0;
        // 설명 및 효과
        public string ItemEffectDesc { get; set; } = "";
        public string ItemLoreDesc { get; set; } = "";
        // 상점 관련
        public int ItemPrice { get; set; }
        public bool IsOwned { get; set; }
        public bool IsEquipped { get; set; }
        public int Quantity { get; set; } = 0;
        public bool IsShopItem { get; set; } = true;
        public int ItemRank { get; set; }
        // 회복량 및 부여 스킬
        public int ItemHealHPAmount { get; set; }
        public int ItemHealMPAmount { get; set; }
        public string? SkillName { get; set; }


        public ItemData() { }

        public ItemData(int id, string name, ItemType type, int def, int speed = 0, int maxHP = 0, int maxMP = 0, float damageMultiplier = 1.0f, int damageBonus = 0, int rerollBonus = 0,
                        string effect = "", string lore = "", int price = 0, bool owned = false, bool equipped = false, string? skillName = null, bool isShopItem = true, int itemRank = 0)
        {
            ID = id;
            ItemName = name;
            ItemType = type;
            ItemDefensePoint = def;
            ItemSpeed = speed;
            ItemMaxHP = maxHP;
            ItemMaxMP = maxMP;
            DamageMultiplier = damageMultiplier;
            DamageBonus = damageBonus;
            RerollBonus = rerollBonus;
            ItemEffectDesc = effect;
            ItemLoreDesc = lore;
            ItemPrice = price;
            IsOwned = owned;
            IsEquipped = equipped;
            SkillName = skillName;
            IsShopItem = isShopItem;
            ItemRank = itemRank;
            ParseHealAmountFromDescription(effect);

        }

        // 소비 아이템(포션/영약) 전용 생성자
        public ItemData(int id, string name, string effect, string lore, int price, bool owned = false, bool equipped = false, bool isShopItem = true, int itemRank = 0)
        {
            ID = id;
            ItemName = name;
            ItemType = ItemType.Consumable;

            ItemDefensePoint = 0;
            ItemSpeed = 0;
            ItemMaxHP = 0;
            ItemMaxMP = 0;
            DamageBonus = 0;
            DamageMultiplier = 1.0f;

            ItemEffectDesc = effect;
            ItemLoreDesc = lore;
            ItemPrice = price;
            IsOwned = owned;
            IsEquipped = equipped;
            SkillName = null;
            IsShopItem = isShopItem;
            ItemRank = itemRank;
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
