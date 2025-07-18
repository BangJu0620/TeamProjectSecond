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
            int listIndex = 1;
            while (true)
            {
                EventManager.Clear();
                EventManager.To(57,"인 벤 토 리\n\n");
                EventManager.To(44,"보유 중인 아이템을 관리할 수 있습니다.\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                var sortedItems = Item.Instance
                .Where(i => i.IsOwned)
                .OrderByDescending(i => i.IsEquipped)
                .ThenBy(i => i.ID)
                .ToList();
                
                bool hasItem = false; //아이템 존재 확인

                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, sortedItems.Count - listIndex * 9 % 9); i++)
                {

                    var item = sortedItems[i];

                    string type;    //해당 아이템의 타입에 따라 출력되는 문구가 변경됩니다.
                    if (item.ItemType == ItemType.Weapon) type = "무  기";
                    else if (item.ItemType == ItemType.Armor) type = "방어구";
                    else if (item.ItemType == ItemType.Accessory) type = "장신구";
                    else type = "소모품";

                    bool isConsumable = item.ItemType == ItemType.Consumable;

                    if (item.IsOwned && (isConsumable ? item.Quantity > 0 : true))
                    {
                        hasItem = true;
                        string equipped = item.IsEquipped ? "[E] " : "";

                        Console.ForegroundColor = ConsoleColor.White;       //앞 작대기 - 표현
                        EventManager.To(10, "- ");

                        Console.ForegroundColor = ConsoleColor.Yellow;      // 장비중이라면 [E]를 표시합니다.
                        Console.Write($"{equipped}");

                        Console.ForegroundColor = ConsoleColor.White;       // 아이템 이름 표시
                        Console.Write($"{item.ItemName}");

                        Console.ForegroundColor = ConsoleColor.Yellow;      // 보유중인 수 표시
                        Console.Write($" (보유: {item.Quantity})");

                        Console.ForegroundColor = ConsoleColor.White;       // 아이템 타입 표시
                        Console.SetCursorPosition(43, Console.CursorTop);
                        Console.Write($"|  {type}");

                        Console.SetCursorPosition(54, Console.CursorTop);   // 아이템 효과
                        Console.Write($"| {item.ItemEffectDesc}");

                        Console.ForegroundColor = ConsoleColor.Gray;        // 아이템 설명
                        Console.SetCursorPosition(80, Console.CursorTop);
                        Console.Write($"||  {item.ItemLoreDesc}\n\n");
                    }
                }

                if (!hasItem)
                {
                    Console.SetCursorPosition(0, 8);
                    EventManager.To(46,"- 보유 중인 아이템이 없습니다.");
                }
                
                Console.SetCursorPosition(0, 24);
                EventManager.ToS(27, $"◁◁ A         1. 장착 관리   2. 포션 사용  Enter. 돌아가기         D ▷▷\n\n");
                EventManager.Select();

                bool goNextPage = (sortedItems.Count - listIndex * 9 > 0 && (sortedItems.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인
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
                    case -1:
                        listIndex = Math.Max(listIndex - 1, 1);
                        break;
                    case -2:
                        if (goNextPage)
                            listIndex++;
                        break;

                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void ManageEquippedItems()  //아이템 장착 관리
        {
            int listIndex = 1;
            while (true)
            {
                EventManager.Clear();
                EventManager.To(57,"인 벤 토 리\n\n");
                EventManager.To(45,"장비를 장착하거나 해제할 수 있습니다.\n\n");

                var ownedItems = Item.Instance
                .Where(i => i.IsOwned && i.ItemType != ItemType.Consumable)
                .OrderByDescending(i => i.IsEquipped)
                .ThenBy(i => i.ID)
                .ToList();

                if (ownedItems.Count == 0) //보유 중인 아이템이 없으면
                {
                    Console.SetCursorPosition(0, 8);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(46,"- 장착 가능한 아이템이 없습니다.");
                }

                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, ownedItems.Count - listIndex * 9 % 9); i++) //보유템 나열
                {
                    var item = ownedItems[i];

                    string type;    //해당 아이템의 타입에 따라 출력되는 문구가 변경됩니다.
                    if (item.ItemType == ItemType.Weapon) type = "무  기";
                    else if (item.ItemType == ItemType.Armor) type = "방어구";
                    else if (item.ItemType == ItemType.Accessory) type = "장신구";
                    else type = "소모품";

                    bool isConsumable = item.ItemType == ItemType.Consumable;
                    string equipped = item.IsEquipped ? "[E] " : "";

                    Console.ForegroundColor = ConsoleColor.White;       //앞 작대기 - 표현
                    EventManager.To(10, $"{i + 1}. ");

                    Console.ForegroundColor = ConsoleColor.Yellow;      // 장비중이라면 [E]를 표시합니다.
                    Console.Write($"{equipped}");

                    Console.ForegroundColor = ConsoleColor.White;       // 아이템 이름 표시
                    Console.Write($"{item.ItemName}");

                    Console.ForegroundColor = ConsoleColor.Yellow;      // 보유중인 수 표시
                    Console.Write($" (보유: {item.Quantity})");

                    Console.ForegroundColor = ConsoleColor.White;       // 아이템 타입 표시
                    Console.SetCursorPosition(43, Console.CursorTop);
                    Console.Write($"|  {type}");

                    Console.SetCursorPosition(54, Console.CursorTop);   // 아이템 효과
                    Console.Write($"| {item.ItemEffectDesc}");

                    Console.ForegroundColor = ConsoleColor.Gray;        // 아이템 설명
                    Console.SetCursorPosition(80, Console.CursorTop);
                    Console.Write($"||  {item.ItemLoreDesc}\n\n");
                }

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(20, $"◁◁ A         장착 / 해제할 아이템 번호를 선택해주세요.   Enter. 돌아가기         D ▷▷\n\n");
                EventManager.Select();

                bool goNextPage = (ownedItems.Count - listIndex * 9 > 0 && (ownedItems.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인
                int? input = EventManager.CheckInput();
                if (input == null) return;                                      // 엔터 누를 시 돌아가기
                else if (input == -1) listIndex = Math.Max(listIndex - 1, 1);   // 페이지 <- 이동 
                else if (input == -2 && goNextPage) listIndex++;                // 페이지 -> 이동

                else if (input >= 1 && input <= ownedItems.Count)
                {
                    var selectedItem = ownedItems[(int)input - 1]; //선택 아이템 저장

                    if (selectedItem.IsEquipped) //기존 장착 해제
                    {
                        selectedItem.IsEquipped = false;

                        EventManager.Announce(47, $"{selectedItem.ItemName}을(를) 장착 해제했습니다.");
                    }
                    else
                    {
                        if (selectedItem.ItemType == ItemType.Accessory)
                        {
                            // 액세서리는 최대 5개까지 중복 착용 가능
                            int equippedCount = ownedItems.Count(i => i.ItemType == ItemType.Accessory && i.IsEquipped);
                            if (equippedCount >= 5)
                            {
                                EventManager.Announce(45, "액세서리는 최대 5개까지 착용할 수 있습니다.");
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
                        EventManager.Announce(47, $"{selectedItem.ItemName}을(를) 장착했습니다.");
                    }
                }
                else
                    EventManager.Wrong();
            }
        }


        private static void UsePotionFlow()
        {
            int listIndex = 1;
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

                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, potions.Count - listIndex * 9 % 9); i++) //보유템 나열

                {
                    var potion = potions[i];

                    Console.ForegroundColor = ConsoleColor.White;       //앞 작대기 - 표현
                    EventManager.To(10, $"{i + 1}. ");

                    Console.ForegroundColor = ConsoleColor.White;       // 아이템 이름 표시
                    Console.Write($"{potion.ItemName}");

                    Console.ForegroundColor = ConsoleColor.Yellow;      // 보유중인 수 표시
                    Console.Write($" (보유: {potion.Quantity})");

                    Console.SetCursorPosition(54, Console.CursorTop);   // 아이템 효과
                    Console.Write($"| {potion.ItemEffectDesc}");

                    Console.ForegroundColor = ConsoleColor.Gray;        // 아이템 설명
                    Console.SetCursorPosition(80, Console.CursorTop);
                    Console.Write($"||  {potion.ItemLoreDesc}\n\n");

                }

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(23, $"◁◁ A         사용할 아이템 번호를 선택해주세요.   Enter. 돌아가기         D ▷▷\n\n");
                EventManager.Select();

                bool goNextPage = (potions.Count - listIndex * 9 > 0 && (potions.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인
                int? input = EventManager.CheckInput();
                if (input == null) break;                                       // 엔터 누를시 돌아가기
                else if (input == -1) listIndex = Math.Max(listIndex - 1, 1);   // 페이지 <- 이동 
                else if (input == -2 && goNextPage) listIndex++;                // 페이지 -> 이동

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

            if (item.ItemEffectDesc.Contains("MaxHP"))
                c.BonusMaxHP += 5;

            else if (item.ItemEffectDesc.Contains("MaxMP"))
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

        public static int GetTotalRerollFromItems()
        {
            return Item.Instance
                .Where(item => item.IsEquipped && IsEquipment(item))
                .Sum(item => item.RerollBonus);
        }

        private static bool IsEquipment(ItemData item)
        {
            return item.ItemType == ItemType.Weapon
                || item.ItemType == ItemType.Armor
                || item.ItemType == ItemType.Accessory;
        }
    }
}
