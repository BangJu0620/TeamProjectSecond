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
                //UI 만들어지면 불러오기
                Console.WriteLine("인벤토리 - 보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                //
                bool hasItem = false; //아이템 존재 확인

                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    var item = Item.Instance[i];
                    if (item.IsOwned)
                    {
                        hasItem = true;
                        string equipped = item.IsEquipped ? "[E]" : "";
                        Console.WriteLine($"- {i + 1} {equipped}{item.ItemName} | {item.ItemDescription}");
                    }
                }

                //UI 만들어지면 불러오기
                if (!hasItem)
                {
                    Console.WriteLine("- 보유 중인 아이템이 없습니다.");
                }
                //
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    ManageEquippedItems();
                }
                else if (input == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
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
                    Console.WriteLine("장착 가능한 아이템이 없습니다.");
                    Console.WriteLine("\n0. 나가기");
                    Console.ReadLine();
                    return;
                }

                for (int i = 0; i < ownedItems.Count; i++) //보유템 나열
                {
                    var item = ownedItems[i];
                    string equipped = item.IsEquipped ? "[E]" : "";
                    Console.WriteLine($"- {i + 1} {equipped}{item.ItemName} | {item.ItemDescription}");
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write("\n장착/해제할 아이템 번호를 선택해주세요.\n>> ");

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
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }
    }
}
