using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;


namespace TeamProjectSecond
{
    public class EventManager
    {
        // UI 출력하는 메서드를 while, for 반복문을 이용해서 무한루프 상태로 만들어주세요.
        // 그리고 break를 넣으면 무한루프 반복문이 종료되면서 그 메서드가 끝나게 해주시면 됩니다.
        // UI 출력하는 메서드를 아래 주석 위치에 맞게 넣어주시고 실행하시면 될겁니다.
        public static void DisplayMainUI()
        {
            var character = Character.Instance;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            while (true)
            {
                Clear();

                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                Console.WriteLine();
                CenterWrite("주사위 마을");
                Console.WriteLine();
                CenterWrite("\b이곳에서 행동을 선택할 수 있습니다.\n\n\n");
                Console.Write(new string(' ', 35)); Console.Write("1. 상태창\n\n");
                Console.Write(new string(' ', 35)); Console.Write("2. 소지품 확인\n\n");
                Console.Write(new string(' ', 35)); Console.Write("3. 상점\n\n");
                Console.Write(new string(' ', 35)); Console.Write("4. 던전\n\n");
                Console.Write(new string(' ', 35)); Console.Write("5. 휴식하기\n\n");
                Console.Write(new string(' ', 35)); Console.Write("6. 퀘스트\n\n");
                Console.Write(new string(' ', 35)); Console.Write("7. 저장 / 불러오기\n\n");
                Select();

                switch (CheckInput())
                {
                    case 1:
                        // 상태보기
                        DisplayStatus();
                        break;
                    case 2:
                        // 소지품 확인
                        DisplayInventory();
                        break;
                    case 3:
                        // 상점
                        Console.Clear();
                        Console.WriteLine("상점");
                        break;
                    case 4:
                        // 던전
                        Console.Clear();
                        Console.WriteLine("던전");
                        break;
                    case 5:
                        // 휴식하기
                        Console.Clear();
                        Console.WriteLine("휴식하기");
                        break;
                    case 6:
                        // 퀘스트
                        Console.Clear();
                        Console.WriteLine("퀘스트");
                        break;
                    case 7:
                        // 저장/불러오기
                        Clear();
                        DisplaySaveLoadUI();
                        break;
                    case -1:
                        Clear();
                        Console.SetCursorPosition(1, 14);
                        CenterWrite("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                    case null:
                        break;
                    default:
                        Clear();
                        Console.SetCursorPosition(1, 14);
                        CenterWrite("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        // UI를 표현하기만 하는 거라면 static으로 하는 게 깔끔할 거 같기도 하고?
        // 아니면 싱글톤을 하거나, 메인 메서드에 객체를 생성하거나인데
        // 아니면 인터페이스? 델리게이트랑 이벤트?
        // 델리게이트랑 이벤트가 UI 쪽에서 쓴다는데 이걸 써볼까?


        public static void DisplayStatus()
        {
            var character = Character.Instance;

            Clear();
            Console.SetCursorPosition(0, 3);
            CenterWrite(" 상 태 창");
            Console.WriteLine("\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"Lv. {character.Level} {character.Job}\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"경험치 {character.Exp} / {character.RequiredExp}\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"{character.Name}\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"공격력 : {character.AttackPoint}\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"방어력 : {character.DefensePoint}\n\n");
            Console.Write(new string(' ', 35)); Console.Write($"생명력 : {character.HealthPoint} / {character.MaxHealthPoint}\n\n");
            Console.SetCursorPosition(0, 24);
            Console.Write(new string(' ', 43)); Console.Write("1. 스킬 확인         Enter. 돌아가기");
            Select();
            switch (CheckInput())
            {
                case 1:
                    Clear();
                    Console.WriteLine("미구현 상태");
                    Console.ReadKey();
                    break;
                case null:
                    return;
                default:
                    Clear();
                    Console.WriteLine("미구현 상태");
                    Console.ReadKey();
                    break;
            }


        }


        public static void DisplayInventory()    // 인벤토리 메뉴
        {
            var character = Character.Instance;

            Clear();
            Console.WriteLine("소지품");
            Console.WriteLine($"");
            Console.WriteLine($"");
        }

        public static void DisplaySaveLoadUI()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 3);
                CenterWrite(" 세 이 브 / 로 드");
                Console.WriteLine("\n\n");
                Console.Write(new string(' ', 35)); Console.Write("1. 세이브\n\n");
                Console.Write(new string(' ', 35)); Console.Write("2. 로드\n\n");
                Console.SetCursorPosition(0, 24);
                CenterWrite("Enter. 돌아가기");
                Select();
                switch (CheckInput())
                {
                    case null:
                        return;
                    case 1:
                        Clear();
                        SaveLoadManager.SaveCharacterData("character.json");
                        SaveLoadManager.SaveItemData("item.json");
                        Console.SetCursorPosition(0, 20);
                        CenterWrite("세이브가 완료되었습니다.");
                        break;
                    case 2:
                        Clear();
                        bool isNotExistCharacter;
                        bool isNotExistItem;
                        isNotExistCharacter = SaveLoadManager.LoadCharacterData("character.json");
                        isNotExistItem = SaveLoadManager.LoadItemData("item.json");
                        Console.SetCursorPosition(0, 20);
                        if(isNotExistCharacter || isNotExistItem)
                        {
                            CenterWrite("저장된 파일이 없습니다.");
                            break;
                        }
                        CenterWrite("로드가 완료되었습니다.");
                        break;
                    default:
                        CenterWrite("잘못된 입력입니다.");
                        break;
                }
            }
        }

        public static int? CheckInput()  // 선택지
        {
            int number;
            while (true)
            {
                string? input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out number);

                if (input == "")
                {
                    return null;
                } 

                if (isNumber)
                {
                    return number;
                }

                else if (!isNumber && (input != ""))
                {
                    return -1;
                }
            }
        }
        static void CenterWrite(string text)
        {
            double width = Console.WindowWidth;
            double textWidth = GetDisplayWidth(text);
            int leftPadding = (int)Math.Max((width - textWidth) / 2, 0);

            Console.WriteLine(new string(' ', leftPadding) + text);
        }
        static double GetDisplayWidth(string text)
        {
            double width = 0;
            foreach (char c in text)
            {
                // 한글 유니코드 범위면 2칸, 아니면 1칸
                if (IsKorean(c))
                    width += 1.5;
                else
                    width += 1;
            }
            return width;
        } // 한글은 1.5칸 , 나머지는 1칸으로 계산하는 함수
        static bool IsKorean(char c)
        {
            return (c >= 0xAC00 && c <= 0xD7A3); // 가 ~ 힣
        } // 입력된 글자가 한글인지 체크하는 함수

        static void Clear()  //화면을 청소하는 함수
        {
            for (int i = 1; i < 29; i++)
            {
                Console.SetCursorPosition(0,i);
                Console.Write(new string(' ', 120));
            }
        }


        static void Select()
        {
            Console.SetCursorPosition(0, 26);
            CenterWrite("\b\b원하시는 행동의 번호를 입력해주세요.");
            Console.SetCursorPosition(43, 27);
            Console.Write("▶▶ ");
            Background();
            Console.SetCursorPosition(60, 27);
        }


        public static void Background()
        {
            string[] repeated = { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" };

            for (int j = 0; j < 6; j++)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 60; i++)
                {
                    Console.Write(repeated[i % 6]);
                    Console.Write(" ");
                }

                Console.SetCursorPosition(0, 29);
                for (int i = 60; i > 0; i--)
                {
                    Console.Write(repeated[(i + 5) % 6]);
                    Console.Write(" ");
                }
            }
        }
    }
}
