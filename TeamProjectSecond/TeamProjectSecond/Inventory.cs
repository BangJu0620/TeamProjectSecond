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
                EventManager.To(57,"인 벤 토 리\n\n");
                EventManager.To(45,"보유 중인 아이템을 관리할 수 있습니다.\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                var sortedItems = Item.Instance
                .Where(i => i.IsOwned)
                .OrderByDescending(i => i.IsEquipped)
                .ThenBy(i => i.ID)
                .ToList();
                
                bool hasItem = false; //아이템 존재 확인

                for (int i = 0; i < sortedItems.Count; i++)
                {
                    var item = sortedItems[i];

                    bool isConsumable = item.ItemType == ItemType.Consumable;
                    if (item.IsOwned && (isConsumable ? item.Quantity > 0 : true))
                    {
                        hasItem = true;
                        string equipped = item.IsEquipped ? "[E]" : "";
                        EventManager.To(33); Console.Write($"- {equipped}{item.ItemName} | {item.ItemEffectDesc} | {item.ItemLoreDesc} (보유: {item.Quantity})\n\n");
                    }
                }

                if (!hasItem)
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46,"- 보유 중인 아이템이 없습니다.");
                }
                
                Console.SetCursorPosition(0, 24);
                EventManager.ToS(40,$"1. 장착 관리   2. 포션 사용  Enter. 돌아가기\n\n");
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
                EventManager.To(57,"인 벤 토 리\n\n");
                EventManager.To(45,"장비를 장착하거나 해제할 수 있습니다.\n\n");
                Console.ForegroundColor = ConsoleColor.White;

                var ownedItems = Item.Instance
                .Where(i => i.IsOwned && i.ItemType != ItemType.Consumable)
                .OrderByDescending(i => i.IsEquipped)
                .ThenBy(i => i.ID)
                .ToList();

                if (ownedItems.Count == 0) //보유 중인 아이템이 없으면
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46,"- 장착 가능한 아이템이 없습니다.");
                }

                for (int i = 0; i < ownedItems.Count; i++) //보유템 나열
                {
                    var item = ownedItems[i];
                    string equipped = item.IsEquipped ? "[E]" : "";
                    EventManager.To(33, $"- {i + 1} {equipped}{item.ItemName} | {item.ItemEffectDesc} | {item.ItemLoreDesc} (보유: {item.Quantity})\n\n");
                }
                Console.SetCursorPosition(0, 24);
                EventManager.ToS(35,$"장착 / 해제할 아이템 번호를 선택해주세요.   Enter. 돌아가기\n\n");
                EventManager.Select();

                int? input = EventManager.CheckInput();
                if (input == null) return;

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
                        if (selectedItem.ItemType == ItemType.Accessory)
                        {
                            // 액세서리는 최대 5개까지 중복 착용 가능
                            int equippedCount = ownedItems.Count(i => i.ItemType == ItemType.Accessory && i.IsEquipped);
                            if (equippedCount >= 5)
                            {
                                EventManager.Announce(45,"액세서리는 최대 5개까지 착용할 수 있습니다.");
                                continue;
                            }
                        }
                        else
                        {
                            // 무기 또는 방어구는 1개만 장착되도록 제한
                            foreach (var item in ownedItems)
                            {
                                if (item.ItemType == selectedItem.ItemType && item.IsEquipped)
                                    item.IsEquipped = false;
                            }
                        }
                        selectedItem.IsEquipped = true;
                        EventManager.Announce(45, $"{selectedItem.ItemName}을(를) 장착했습니다.");
                    }
                }
                else
                    EventManager.Wrong();
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
                EventManager.To(57,"인 벤 토 리\n\n");
                EventManager.To(45,"보유 중인 포션을 사용할 수 있습니다.\n\n");
                Console.ForegroundColor = ConsoleColor.White;

                if (potions.Count == 0)
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46, "- 사용 가능한 포션이 없습니다.");
                }

                for (int i = 0; i < potions.Count; i++)
                {
                    var potion = potions[i];
                    EventManager.To(33, $"{i + 1}. {potion.ItemName} | {potion.ItemEffectDesc} | {potion.ItemLoreDesc} (보유: {potion.Quantity})\n\n");
                }

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(36,$"사용할 아이템 번호를 선택해주세요.   Enter. 돌아가기\n\n");
                EventManager.Select();

                int? input = EventManager.CheckInput();
                if (input == null) break;
                else if (input >=1 && input <= potions.Count)
                {
                    Inventory.UsePotion(potions[(int)input - 1]);
                }
                else
                {
                    EventManager.Wrong();
                }
            }
        }

        // 포션 사용 기능
        // 활용 예시 Inventory.UsePotion("MP포션");

        public static void UsePotion(ItemData item)
        {
            if (item == null || item.Quantity <= 0) return;

            item.Quantity--;

            var c = Character.Instance;

            if (item.ItemHealHPAmount > 0)
            {
                int beforeHP = c.HealthPoint;
                c.HealthPoint = Math.Min(beforeHP + item.ItemHealHPAmount, c.MaxHealthPoint);

                EventManager.Announce(45, $"HP를 {c.HealthPoint - beforeHP} 회복했습니다. (현재 HP: {c.HealthPoint}/{c.MaxHealthPoint}");
            }

            if (item.ItemHealMPAmount > 0)
            {
                int beforeMP = c.ManaPoint;
                c.ManaPoint = Math.Min(beforeMP + item.ItemHealMPAmount, c.MaxManaPoint);
                EventManager.Announce(45, $"MP를 {c.ManaPoint - beforeMP} 회복했습니다. (현재 MP: {c.ManaPoint}/{c.MaxManaPoint})");
            }

            if (item.ItemEffectDesc.Contains("영약"))
            {
                ApplyElixirEffect(item);
                EventManager.Announce(45, $"{item.ItemName}을(를) 사용하여 능력치가 영구 상승했습니다!");
            }
        }

        private static void ApplyElixirEffect(ItemData item)
        {
            var c = Character.Instance;

            if (item.ItemEffectDesc.Contains("최대 체력"))
                c.BonusMaxHP += 5;

            else if (item.ItemEffectDesc.Contains("최대 마나"))
                c.BonusMaxMP += 5;

            else if (item.ItemEffectDesc.Contains("방어력"))
                c.BonusDefense += 1;

            else if (item.ItemEffectDesc.Contains("속도"))
                c.BonusSpeed += 1;

            else if (item.ItemEffectDesc.Contains("데미지"))
                c.BaseDamageMultiplier += 0.05f;

            else if (item.ItemEffectDesc.Contains("추가데미지"))
                c.BaseDamageBonus += 1;
        }
    }
}
