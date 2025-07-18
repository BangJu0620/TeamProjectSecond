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
            int listIndex = 1;
            while (true)
            {
                EventManager.Clear();
                EventManager.To(60, "상 점\n\n");
                EventManager.To(46,"아이템을 거래할 수 있는 상점입니다.");
                EventManager.ToS(18, $"보유 골드 : {Character.Instance.Gold} G\n\n");

                var shopItems = Item.Instance
                    .Where(item => item.IsShopItem)
                    .ToList();

                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, shopItems.Count - listIndex * 9 % 9); i++)
                {

                    var item = shopItems[i];

                    string priceDisplay = $"{item.ItemPrice,8} G";
                    string quantityInfo = item.IsOwned
                        ? $"(보유: {item.Quantity})"
                        : "";

                    string type;    //해당 아이템의 타입에 따라 출력되는 문구가 변경됩니다.
                    if (item.ItemType == ItemType.Weapon) type = "무  기";
                    else if (item.ItemType == ItemType.Armor) type = "방어구";
                    else if (item.ItemType == ItemType.Accessory) type = "장신구";
                    else type = "소모품";

                    EventManager.To(5, $"- {item.ItemName}");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {quantityInfo}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(38, Console.CursorTop);
                    Console.Write($"| {type} ");

                    Console.SetCursorPosition(47, Console.CursorTop);
                    Console.Write($"| {item.ItemEffectDesc}");

                    Console.SetCursorPosition(98, Console.CursorTop);
                    Console.Write($"| 가격: {priceDisplay}\n\n");
                }

                bool goNextPage = (shopItems.Count - listIndex * 9 > 0 && (shopItems.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인

                Console.SetCursorPosition(0, 24);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                EventManager.ToS(25, $"◁◁ A         1. 아이템 구매  2. 아이템 판매  Enter. 돌아가기         D ▷▷\n\n");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case 1:
                        BuyItem(shopItems, ref listIndex);
                        break;
                    case 2:
                        SellItem();
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
        /////////////////////////////////////////////////////////구매 탭
        private static void BuyItem(List<ItemData> shopItems , ref int listIndex)
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.To(60, "상 점\n\n");
                EventManager.To(46, "아이템을 거래할 수 있는 상점입니다.");
                EventManager.ToS(18, $"보유 골드 : {Character.Instance.Gold} G\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, shopItems.Count - listIndex * 9 % 9); i++)
                {
                    var item = shopItems[i];

                    string type;    //해당 아이템의 타입에 따라 출력되는 문구가 변경됩니다.
                    if (item.ItemType == ItemType.Weapon) type = "무  기";
                    else if (item.ItemType == ItemType.Armor) type = "방어구";
                    else if (item.ItemType == ItemType.Accessory) type = "장신구";
                    else type = "소모품";

                    string priceDisplay = $"{item.ItemPrice,8} G";
                    string quantityInfo = item.IsOwned
                        ? $"(보유: {item.Quantity})"
                        : "";

                    EventManager.To(5, $"{i + 1}. {item.ItemName}");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {quantityInfo}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(38, Console.CursorTop);
                    Console.Write($"| {type} ");

                    Console.SetCursorPosition(47, Console.CursorTop);
                    Console.Write($"| {item.ItemEffectDesc}");

                    Console.SetCursorPosition(98, Console.CursorTop);
                    Console.Write($"| 가격: {priceDisplay}\n\n");
                }

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(25, $"◁◁ A        구매할 아이템 번호를 입력하세요.  Enter. 돌아가기        D ▷▷\n\n");
                EventManager.Select();

                bool goNextPage = (shopItems.Count - listIndex * 9 > 0 && (shopItems.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인

                int? input = EventManager.CheckInput();
                if (input == null)
                    return;
                else if (input == -1)
                    listIndex = Math.Max(listIndex - 1, 1);
                else if (input == -2 && goNextPage)
                    listIndex++;
                else if (input == -2 && !goNextPage)
                    continue;
                else if (input >= 1 && input <= shopItems.Count)
                {
                    var item = shopItems[(int)input - 1];

                    if (item.ItemType == ItemType.Consumable)
                    {
                        while (true)
                        {
                            EventManager.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(0, 12);
                            EventManager.To(50, $"구매하려는 물품 : {item.ItemName}\n\n");
                            EventManager.To(50, "몇 개를 구매하시겠습니까?");

                            Console.SetCursorPosition(0, 24);
                            EventManager.ToS(40, $"구매할 아이템 수량을 입력하세요.  Enter. 돌아가기\n\n");
                            EventManager.Select();

                            int? amount = EventManager.CheckInput();
                            if (amount == null) return;
                            else if (amount > 0)
                            {
                                int totalCost = item.ItemPrice * (int)amount;

                                if (Character.Instance.Gold < totalCost)
                                    EventManager.Announce(55, "Gold가 부족합니다.");
                                else
                                {
                                    bool added = Item.AddItem(item.ItemName, (int)amount, showMessage: false);

                                    if (added)
                                    {
                                        Character.Instance.Gold -= totalCost;
                                        EventManager.Announce(45, $"{item.ItemName}을(를) {amount}개 구매했습니다.");
                                        return;
                                    }
                                }
                            }
                            else EventManager.Wrong();
                        }
                    }
                    else // 장비
                    {
                        int totalCost = item.ItemPrice;

                        if (item.IsOwned)
                            EventManager.Announce(48, "이미 보유하고 있는 아이템입니다.");
                        else if (Character.Instance.Gold < totalCost)
                            EventManager.Announce(55, "Gold가 부족합니다.");
                        else
                        {
                            Character.Instance.Gold -= totalCost;
                            bool added = Item.AddItem(item.ItemName, 1, showMessage: false);

                            if (added)
                                EventManager.Announce(50, $"{item.ItemName}을(를) 구매했습니다.");
                        }
                    }
                }
                else { EventManager.Wrong(); }
            }
        }

        //////////////////////////////////////판매탭
        private static void SellItem()
        {
            int listIndex = 1;
            while (true)
            {
                EventManager.Clear();
                EventManager.To(60, "상 점\n\n");
                EventManager.To(46, "아이템을 거래할 수 있는 상점입니다.");
                EventManager.ToS(18, $"보유 골드 : {Character.Instance.Gold} G\n\n");
                
                var ownedItems = Item.Instance.Where(i => i.IsOwned).ToList();
                if (ownedItems.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.Announce(47,"판매 가능한 아이템이 없습니다.");
                    return;
                }

                for (int i = (listIndex * 9) - 9; i < Math.Min(listIndex * 9, ownedItems.Count - listIndex * 9 % 9); i++)
                {

                    var item = ownedItems[i];
                    string priceDisplay = $"{item.ItemPrice} G";
                    string quantityInfo = item.IsOwned
                        ? $"(보유: {item.Quantity})"
                        : "";
                    string type;    //해당 아이템의 타입에 따라 출력되는 문구가 변경됩니다.
                    if (item.ItemType == ItemType.Weapon) type = "무  기";
                    else if (item.ItemType == ItemType.Armor) type = "방어구";
                    else if (item.ItemType == ItemType.Accessory) type = "장신구";
                    else type = "소모품";

                    EventManager.To(5, $"{i + 1}. {item.ItemName}");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {quantityInfo}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(38, Console.CursorTop);
                    Console.Write($"| {type} ");

                    Console.SetCursorPosition(47, Console.CursorTop);
                    Console.Write($"| {item.ItemEffectDesc}");

                    Console.SetCursorPosition(98, Console.CursorTop);
                    Console.Write($"| 판매가: {item.GetSellPrice(),6} G\n\n");

                    //EventManager.To(15,$"{i + 1}. {item.ItemName}{quantityInfo} - 판매가: {item.GetSellPrice()} G\n\n");
                }

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(25, $"◁◁ A        판매할 아이템의 번호를 입력하세요.  Enter. 돌아가기        D ▷▷\n\n");
                EventManager.Select();

                bool goNextPage = (ownedItems.Count - listIndex * 9 > 0 && (ownedItems.Count - (listIndex * 9)) % 9 != 0);  // 리스트 다음장에 아이템이 남아있는지 확인
                
                int? input = EventManager.CheckInput();
                if (input == null) return;
                else if (input == -1) listIndex = Math.Max(listIndex - 1, 1);   //페이지 <- 이동 
                else if (input == -2 && goNextPage) listIndex++;                //페이지 -> 이동

                else if (input >= 1 && input <= ownedItems.Count)

                    if (input >= 1 && input <= ownedItems.Count)
                {
                    var item = ownedItems[(int)input - 1];
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
                    EventManager.Announce(48,$"{item.ItemName}을(를) 판매했습니다. {sellPrice} G 획득!");
                }
                else
                {
                    EventManager.Wrong();
                }
            }
        }
    }
}
