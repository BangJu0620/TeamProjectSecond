using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public static class Inventory
    {
        public static void ShowInventory()
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.Background();
                Console.SetCursorPosition(0, 2);
                EventManager.To(56,"인 벤 토 리");
                Console.WriteLine();
                EventManager.To(44,"보유 중인 아이템을 관리할 수 있습니다.\n\n");

                Item.Instance.Sort((x, y) => y.IsEquipped.CompareTo(x.IsEquipped)); // 장착 중인 아이템을 맨 위로 정렬
                //UI 만들어지면 불러오기

                EventManager.To(55,"[아이템 목록]\n\n");
                //
                bool hasItem = false; //아이템 존재 확인

                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    var item = Item.Instance[i];
                    if (item.IsOwned && (item.ItemType != ItemType.Consumable || item.Quantity > 0))
                    {
                        hasItem = true;
                        string equipped = item.IsEquipped ? "[E]" : "";
                        EventManager.To(25); Console.Write($"- {i + 1} {equipped}{item.ToString()} x{item.Quantity}\n\n");
                    }
                }

                //UI 만들어지면 불러오기
                if (!hasItem)
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46,"- 보유 중인 아이템이 없습니다.");
                }
                //
                Console.SetCursorPosition(0, 24);
                EventManager.To(40,$"1. 장착 관리   2. 포션 사용  Enter. 돌아가기\n\n");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case 1:
                        ManageEquippedItems();
                        break;
                    case 2:
                        UsePotionFlow();
                        break;
                    case null:
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void ManageEquippedItems()
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.Background();
                Console.SetCursorPosition(0, 2);
                EventManager.To(56,"인 벤 토 리");
                Console.WriteLine();
                EventManager.To(44,"장비를 장착하거나 해제할 수 있습니다.\n\n");
                EventManager.To(55,"[아이템 목록]\n\n");

                var ownedItems = new List<ItemData>(); //보유 중인 아이템만
                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    if (Item.Instance[i].IsOwned && Item.Instance[i].ItemType != ItemType.Consumable)
                    {
                        ownedItems.Add(Item.Instance[i]);
                    }
                }

                if (ownedItems.Count == 0) //보유 중인 아이템이 없으면
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46,"- 장착 가능한 아이템이 없습니다.");
                }

                for (int i = 0; i < ownedItems.Count; i++) //보유템 나열
                {
                    var item = ownedItems[i];
                    string equipped = item.IsEquipped ? "[E]" : "";
                    EventManager.To(25,$"- {i + 1} {equipped}{item.ItemName} | {item.ItemDescription}");
                }
                Console.SetCursorPosition(0, 24);
                EventManager.To(35,$"장착/해제할 아이템 번호를 선택해주세요.   Enter. 돌아가기\n\n");
                EventManager.Select();

                int? input = EventManager.CheckInput();
                if (input == null)
                    return;

                else if (input >= 1 && input <= ownedItems.Count)
                {
                    var selectedItem = ownedItems[(int)input - 1]; //선택 아이템 저장

                    if (selectedItem.IsEquipped) //기존 장착 해제
                    {
                        selectedItem.IsEquipped = false;

                        EventManager.Announce(45, $"{selectedItem.ItemName}을(를) 장착 해제했습니다.");
                    }
                    else
                    {
                        // 타입별로 하나만 장착되도록 제한
                        foreach (var item in ownedItems)
                        {
                            if (item.ItemType == selectedItem.ItemType && item.IsEquipped)
                            {
                                item.IsEquipped = false;
                            }
                        }

                        selectedItem.IsEquipped = true;

                        EventManager.Announce(45, $"\n{selectedItem.ItemName}을(를) 장착했습니다.");
                    }
                }

                else
                {
                    EventManager.Wrong();
                }
            }
        }


        private static void UsePotionFlow()
        {
            while (true)
            {
                var potions = Item.Instance
                    .Where(i => i.IsOwned && i.ItemType == ItemType.Consumable && i.Quantity > 0)
                    .ToList();

                EventManager.Clear();
                EventManager.Background();
                Console.SetCursorPosition(0, 2);
                EventManager.To(56,"인 벤 토 리");
                Console.WriteLine();
                EventManager.To(44,"보유중인 포션을 사용할 수 있습니다.\n\n");
                EventManager.To(55,"[포션 목록]\n\n");

                if (potions.Count == 0)
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46, "- 사용 가능한 포션이 없습니다.");
                }

                for (int i = 0; i < potions.Count; i++)
                {
                    EventManager.To(25,$"{i + 1}. {potions[i].ItemName} (보유 수량: {potions[i].Quantity})");
                }

                Console.SetCursorPosition(0, 24);
                EventManager.To(35,$"사용할 아이템 번호를 선택해주세요.   Enter. 돌아가기\n\n");
                EventManager.Select();

                int? input = EventManager.CheckInput();
                if (input == null) break;
                else if (input >=1 && input <= potions.Count)
                {
                    Inventory.UsePotion(potions[(int)input - 1].ItemName);
                }
                else
                {
                    EventManager.Wrong();
                }
            }
        }

        // 포션 사용 기능
        // 활용 예시 Inventory.UsePotion("MP포션");

        public static void UsePotion(string potionName)
        {
            var item = Item.Instance.FirstOrDefault(i =>
                i.ItemName == potionName &&
                i.ItemType == ItemType.Consumable &&
                i.Quantity > 0);

            if (item != null)
            {
                item.Quantity--;

                if (item.ItemHealHPAmount > 0) // HP회복로직
                {
                    int beforeHP = Character.Instance.HealthPoint;
                    Character.Instance.HealthPoint = Math.Min(
                        beforeHP + item.ItemHealHPAmount,
                        Character.Instance.MaxHealthPoint);

                    EventManager.Clear();
                    Console.SetCursorPosition(0, 14);
                    EventManager.Announce(45,$"HP를 {Character.Instance.HealthPoint - beforeHP}" +
                        $" 회복했습니다. (현재 HP: {Character.Instance.HealthPoint}/{Character.Instance.MaxHealthPoint})\n");
                }

                if (item.ItemHealMPAmount > 0) // MP회복 로직
                {
                    int beforeMP = Character.Instance.ManaPoint;
                    Character.Instance.ManaPoint = Math.Min(
                        beforeMP + item.ItemHealMPAmount,
                        Character.Instance.MaxManaPoint);

                    EventManager.Clear();
                    Console.SetCursorPosition(0, 14);
                    EventManager.Announce(45,$"MP를 {Character.Instance.ManaPoint - beforeMP}" +
                        $" 회복했습니다. (현재 MP: {Character.Instance.ManaPoint}/{Character.Instance.MaxManaPoint})");
                }
            }
        }
    }
}
