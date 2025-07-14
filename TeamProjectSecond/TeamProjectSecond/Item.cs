using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
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
            instance.Add(new ItemData("천 갑옷", "Armor", 0, 3, "얇지만 움직이기 쉬운 천 갑옷입니다.", 700, false, false));
            instance.Add(new ItemData("수련자 갑옷", "Armor", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, false, false));
            instance.Add(new ItemData("무쇠갑옷", "Armor", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, false, false));
            instance.Add(new ItemData("스파르타의 갑옷", "Armor", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, false, false));
            instance.Add(new ItemData("낡은 검", "Weapon", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 600, false, false));
            instance.Add(new ItemData("짧은 단검", "Weapon", 3, 0, "빠르고 가볍지만 위력이 낮은 단검입니다.", 900, false, false));
            instance.Add(new ItemData("청동 도끼", "Weapon", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500, false, false));
            instance.Add(new ItemData("스파르타의 창", "Weapon", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700, false, false));
        }
    }

    public class ItemData
    {
        public string itemName { get; set; }
        public string itemType {  get; set; }
        public int itemAttackPoint {  get; set; }
        public int itemDefensePoint {  get; set; }
        public string itemDescription {  get; set; }
        public int itemPrice {  get; set; }
        public bool isOwned {  get; set; }
        public bool isEquipped {  get; set; }

        public ItemData(string iName, string iType, int iAP, int iDP, string iDescription, int iPrice, bool iOwned, bool iEquipped)
        {
            itemName = iName;
            itemType = iType;
            itemAttackPoint = iAP;
            itemDefensePoint = iDP;
            itemDescription = iDescription;
            itemPrice = iPrice;
            isOwned = iOwned;
            isEquipped = iEquipped;
        }
    }
}
