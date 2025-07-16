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
                Console.Clear();
                Item.Instance.Sort((x, y) => y.IsEquipped.CompareTo(x.IsEquipped)); // 장착 중인 아이템을 맨 위로 정렬
                //UI 만들어지면 불러오기
                Console.WriteLine("인벤토리 - 보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                //
                bool hasItem = false; //아이템 존재 확인

                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    var item = Item.Instance[i];
                    if (item.IsOwned && (item.ItemType != ItemType.Consumable || item.Quantity > 0))
                    {
                        hasItem = true;
                        string equipped = item.IsEquipped ? "[E]" : "";
                        Console.WriteLine($"- {i + 1} {equipped}{item.ToString()} x{item.Quantity}");
                    }
                }

                //UI 만들어지면 불러오기
                if (!hasItem)
                {
                    Console.WriteLine("- 보유 중인 아이템이 없습니다.");
                }
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("2. 포션 사용");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                //
                string input = Console.ReadLine();

                if (input == "1")
                {
                    ManageEquippedItems();
                }
                else if (input == "2")
                {
                    UsePotionFlow();
                }
                else if (input == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");//
                    Console.ReadKey();
                }
            }
        }

        public static void ManageEquippedItems()
        {
            while (true)
            {
                Console.Clear();
                //UI 만들어지면 불러오기
                Console.WriteLine("인벤토리 - 장착 관리\n");
                //
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
                    //
                    Console.WriteLine("장착 가능한 아이템이 없습니다.");
                    Console.WriteLine("\n0. 나가기");
                    //
                    Console.ReadLine();
                    return;
                }

                for (int i = 0; i < ownedItems.Count; i++) //보유템 나열
                {
                    var item = ownedItems[i];
                    string equipped = item.IsEquipped ? "[E]" : "";
                    Console.WriteLine($"- {i + 1} {equipped}{item.ItemName} | {item.ItemDescription}");
                }
                //
                Console.WriteLine("\n0. 나가기");
                Console.Write("\n장착/해제할 아이템 번호를 선택해주세요.\n>> ");
                //
                string input = Console.ReadLine();

                if (input == "0") break;

                if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= ownedItems.Count) //숫자인지, 유효한 번호인지 검증
                {
                    var selectedItem = ownedItems[selectedIndex - 1]; //선택 아이템 저장

                    if (selectedItem.IsEquipped) //기존 장착 해제
                    {
                        selectedItem.IsEquipped = false;
                        Console.WriteLine($"\n{selectedItem.ItemName}을(를) 장착 해제했습니다.");
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
                        Console.WriteLine($"\n{selectedItem.ItemName}을(를) 장착했습니다.");
                    }
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");//
                    Console.ReadKey();
                }
            }
        }

        // 포션 사용 기능
        // 활용 예시 Inventory.UsePotion("MP포션");
        // UsePotion("HP포션", false); <<회복했습니다 메세지 안뜸
        public static bool UsePotion(string potionName, bool showMessage = true)
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

                    if (showMessage)
                        Console.WriteLine($"HP를 {Character.Instance.HealthPoint - beforeHP} 회복했습니다.");
                }

                if (item.ItemHealMPAmount > 0) // MP회복 로직
                {
                    int beforeMP = Character.Instance.ManaPoint;
                    Character.Instance.ManaPoint = Math.Min(
                        beforeMP + item.ItemHealMPAmount,
                        Character.Instance.MaxManaPoint);

                    if (showMessage)
                        Console.WriteLine($"MP를 {Character.Instance.ManaPoint - beforeMP} 회복했습니다.");
                }

                return true;
            }

            if (showMessage)
                Console.WriteLine("포션이 부족하거나 존재하지 않습니다.");
            return false;
        }

        private static void UsePotionFlow()
        {
            while (true)
            {
                Console.Clear();
                var potions = Item.Instance
                    .Where(i => i.IsOwned && i.ItemType == ItemType.Consumable && i.Quantity > 0)
                    .ToList();

                if (potions.Count == 0)
                {
                    Console.WriteLine("사용할 수 있는 포션이 없습니다.");//
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\n[사용 가능한 포션 목록]");//
                for (int i = 0; i < potions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {potions[i].ItemName} (보유 수량: {potions[i].Quantity})");
                }

                Console.Write("\n사용할 포션 번호를 선택하세요 (0. 취소): ");//
                string input = Console.ReadLine();

                if (input == "0") break;

                if (int.TryParse(input, out int index) && index >= 1 && index <= potions.Count)
                {
                    Inventory.UsePotion(potions[index - 1].ItemName);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");//
                }

                Console.WriteLine("\n계속 사용하려면 아무 키나 누르세요. (0 입력 시 종료)");//
                if (Console.ReadLine() == "0") break;
            }
        }
    }
}
