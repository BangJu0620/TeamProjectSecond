using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public static class Shop
    {
        public static void EnterShop()
        {
            while (true)
            {
                Console.Clear();
                //UI 만들어지면 불러오기
                Console.WriteLine("상점\n");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드] {Character.Instance.Gold} G\n");

                Console.WriteLine("[아이템 목록]");
                //
                var shopItems = new List<ItemData>();

                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    var item = Item.Instance[i];
                    if (item.IsShopItem)
                    {
                        shopItems.Add(item);
                        string stats = item.ItemType == ItemType.Weapon ? $"공격력 +{item.ItemAttackPoint}" :
                                       item.ItemType == ItemType.Armor ? $"방어력 +{item.ItemDefensePoint}" : "";

                        string priceDisplay = item.IsOwned && item.ItemType != ItemType.Consumable ? "구매완료" : $"{item.ItemPrice} G";

                        string quantityInfo = item.ItemType == ItemType.Consumable && item.IsOwned ? $" (보유: {item.Quantity})" : ""; //소모품 보유 수량
                        Console.WriteLine($"- {shopItems.Count}. {item.ItemName}{quantityInfo}");
                        Console.WriteLine($"  {item.ToString()}가격: {priceDisplay}");
                    }
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    BuyItem(shopItems);
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

        private static void BuyItem(List<ItemData> shopItems)
        {
            Console.Clear();
            //UI 만들어지면 불러오기
            Console.WriteLine("구매할 아이템 번호를 입력하세요. (0. 취소)");
            //
            string input = Console.ReadLine();

            if (input == "0") return;

            if (int.TryParse(input, out int index) && index >= 1 && index <= shopItems.Count)
            {
                var item = shopItems[index - 1];

                if (item.ItemType == ItemType.Consumable) // 소비템 다중구매
                {
                    Console.Write("몇 개를 구매하시겠습니까? >> ");
                    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                    {
                        int totalCost = item.ItemPrice * amount;

                        if (Character.Instance.Gold < totalCost)
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                        }
                        else
                        {
                            bool allSuccess = true;

                            int addedCount = 0;

                            for (int i = 0; i < amount; i++)
                            {
                                if (Item.AddItem(item.ItemName))
                                {
                                    addedCount++;
                                }
                                else
                                {
                                    Console.WriteLine("아이템 추가에 실패했습니다.");
                                    // 롤백 처리
                                    var rollbackItem = Item.Instance.FirstOrDefault(it => it.ItemName == item.ItemName);
                                    if (rollbackItem != null)
                                    {
                                        rollbackItem.Quantity -= addedCount;
                                    }
                                    allSuccess = false;
                                    break;
                                }
                            }
                            if (allSuccess)
                            {
                                Character.Instance.Gold -= totalCost;
                                Console.WriteLine($"{item.ItemName}을(를) {amount}개 구매했습니다.");
                            }
                            else
                            {
                                Console.WriteLine("일부 아이템 추가에 실패하여 구매가 취소되었습니다.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else // 장비
                {
                    int totalCost = item.ItemPrice;

                    if (item.IsOwned)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    }
                    else if (Character.Instance.Gold < totalCost)
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                    else
                    {
                        Character.Instance.Gold -= totalCost;
                        bool added = Item.AddItem(item.ItemName);

                        if (added)
                        {
                            Console.WriteLine($"{item.ItemName}을(를) 구매했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("아이템 추가에 실패했습니다.");
                        }

                        Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    }
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

            Console.ReadKey();
        }
    }
}
