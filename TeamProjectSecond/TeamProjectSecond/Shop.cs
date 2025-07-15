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

                int displayIndex = 1;
                for (int i = 0; i < Item.Instance.Count; i++)
                {
                    var item = Item.Instance[i];
                    if (item.IsShopItem)
                    {
                        shopItems.Add(item);

                        string stats = item.ItemType == ItemType.Weapon ? $"공격력 +{item.ItemAttackPoint}" :
                                       item.ItemType == ItemType.Armor ? $"방어력 +{item.ItemDefensePoint}" : "";

                        string priceDisplay = item.IsOwned && item.ItemType != ItemType.Consumable
                            ? "구매완료"
                            : $"{item.ItemPrice} G";

                        string quantityInfo = item.ItemType == ItemType.Consumable && item.IsOwned
                            ? $" (보유: {item.Quantity})"
                            : "";

                        Console.WriteLine($"- {displayIndex}. {item.ItemName}{quantityInfo} | {stats} | 가격: {priceDisplay}");
                        displayIndex++;
                    }
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    BuyItem(shopItems);
                }
                else if (input == "2")
                {
                    SellItem();
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
            while (true)
            {
                Console.Clear();
                //UI
                Console.WriteLine("구매할 아이템 번호를 입력하세요. (0. 취소)\n");
                //
                for (int i = 0; i < shopItems.Count; i++)
                {
                    var item = shopItems[i];
                    string quantityInfo = item.ItemType == ItemType.Consumable && item.IsOwned
                                          ? $" (보유: {item.Quantity})" : "";
                    string priceDisplay = item.IsOwned && item.ItemType != ItemType.Consumable
                                          ? "구매완료" : $"{item.ItemPrice} G";
                    Console.WriteLine($"{i + 1}. {item.ItemName}{quantityInfo} - {priceDisplay}");
                }
                //UI
                Console.WriteLine("\n구매할 아이템 번호를 입력하세요. (0. 취소)");
                //
                string input = Console.ReadLine();


                if (input == "0") break;

                if (int.TryParse(input, out int index) && index >= 1 && index <= shopItems.Count)
                {
                    var item = shopItems[index - 1];

                    if (item.ItemType == ItemType.Consumable)
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
                                bool added = Item.AddItem(item.ItemName, amount, showMessage: false);

                                if (added)
                                {
                                    Character.Instance.Gold -= totalCost;
                                    Console.WriteLine($"{item.ItemName}을(를) {amount}개 구매했습니다.");
                                }
                                else
                                {
                                    Console.WriteLine("아이템 추가에 실패했습니다.");
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
                            bool added = Item.AddItem(item.ItemName, 1, showMessage: false);

                            if (added)
                            {
                                Console.WriteLine($"{item.ItemName}을(를) 구매했습니다.");
                            }
                            else
                            {
                                Console.WriteLine("아이템 추가에 실패했습니다.");
                            }

                            Console.WriteLine("\n[Enter]를 누르면 계속 구매할 수 있습니다. (종료하려면 0 입력)");
                            if (Console.ReadLine() == "0") break;
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

        private static void SellItem()
        {
            while (true)
            {
                Console.Clear();
                //UI
                Console.WriteLine("판매할 아이템을 선택하세요 (0. 취소)\n");
                //
                var ownedItems = Item.Instance.Where(i => i.IsOwned).ToList();
                if (ownedItems.Count == 0)
                {
                    Console.WriteLine("판매 가능한 아이템이 없습니다.");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < ownedItems.Count; i++)
                {
                    var item = ownedItems[i];
                    string quantityInfo = item.ItemType == ItemType.Consumable ? $" (보유: {item.Quantity})" : "";
                    Console.WriteLine($"{i + 1}. {item.ItemName}{quantityInfo} - 판매가: {item.GetSellPrice()} G");
                }

                Console.Write("\n판매할 아이템 번호 입력: ");
                string input = Console.ReadLine();

                if (input == "0") break;

                if (int.TryParse(input, out int index) && index >= 1 && index <= ownedItems.Count)
                {
                    var item = ownedItems[index - 1];
                    int sellPrice = item.GetSellPrice();

                    if (item.ItemType == ItemType.Consumable)
                    {
                        item.Quantity--;
                        if (item.Quantity <= 0)
                        {
                            item.IsOwned = false;
                        }
                    }
                    else
                    {
                        if (item.IsEquipped)
                        {
                            item.IsEquipped = false;
                        }
                        item.IsOwned = false;
                    }

                    Character.Instance.Gold += sellPrice;
                    Console.WriteLine($"{item.ItemName}을(를) 판매했습니다. {sellPrice} G 획득!");

                    Console.WriteLine("\n[Enter]를 누르면 계속 판매할 수 있습니다. (종료하려면 0 입력)");
                    if (Console.ReadLine() == "0") break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }
    }
}
