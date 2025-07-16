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
        public static void DisplayMainUI(SaveLoadUI saveLoadUI)
        {
            var character = Character.Instance;
            var rest = Rest.Instance;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            
            while (true)
            {
                Clear();
                Background();
                Console.SetCursorPosition(0, 2);
                To(55); Console.Write (" 주사위 마을");
                Console.WriteLine();
                To(45); Console.Write ("이곳에서 행동을 선택할 수 있습니다.\n\n\n\n");
                To(41); Console.Write("1. 상태창\n\n");
                To(41); Console.Write("2. 소지품 확인\n\n");
                To(41); Console.Write("3. 상점\n\n");
                To(41); Console.Write("4. 던전\n\n");
                To(41); Console.Write("5. 휴식하기\n\n");
                To(41); Console.Write("6. 퀘스트\n\n");
                To(41); Console.Write("7. 저장 / 불러오기\n\n");
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
                        rest.RestInVillage();
                        break;
                    case 6:
                        // 퀘스트
                        Console.Clear();
                        Console.WriteLine("퀘스트");
                        break;
                    case 7:
                        // 저장/불러오기
                        Console.Clear();
                        Console.WriteLine("저장/불러오기");
                        break;
                    case null:
                        break;
                    default:
                        Wrong();
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
            while (true)
            {
                Clear();
                Background();
                Console.SetCursorPosition(0, 2);
                To(56); Console.Write(" 상 태 창\n");
                To(45); Console.Write("캐릭터의 정보를 확인할 수 있습니다.");
                Console.WriteLine("\n\n\n");
                To(41); Console.Write($"Lv. {character.Level} {character.Job}\n\n");
                To(41); Console.Write($"경험치 {character.Exp} / {character.RequiredExp}\n\n");
                To(41); Console.Write($"{character.Name}\n\n");
                To(41); Console.Write($"공격력 : {character.AttackPoint}\n\n");
                To(41); Console.Write($"방어력 : {character.DefensePoint}\n\n");
                To(41); Console.Write($"생명력 : {character.HealthPoint} / {character.MaxHealthPoint}\n\n");
                Console.SetCursorPosition(0, 24);
                To(43); Console.Write("1. 스킬 확인         Enter. 돌아가기");
                Select();
             
                switch (CheckInput())
                {
                    case 1:
                        Clear();
                        Background();
                        Console.WriteLine("미구현 상태");
                        Console.ReadKey();
                        break;
                    case null:
                        return;
                    default:
                        Wrong();
                        break;
                }
            }
        }


        public static void DisplayInventory()    // 인벤토리 메뉴
        {
            var character = Character.Instance;

            while (true)
            {
                Clear();
                Console.SetCursorPosition(0, 3);
                To(55); Console.Write("인 벤 토 리");
                Console.WriteLine("\n\n");

                for (int i = 0; ;)
                {
                    To(35); Console.Write($"");
                    Console.WriteLine($"");
                    Console.WriteLine($"");
                }
            }
        }


        public static int? CheckInput()  // 선택을 입력받는 함수
        {
            int number;
            while (true)
            {
                string? input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out number);

                if (input == "")                        // 엔터만 눌렀을 때
                {
                    return null;                        // null값을 반환합니다.
                } 

                if (isNumber)                           // 숫자를 입력했을 때, 
                {
                    return number;                      // 그 값을 반환합니다.
                }

                else if (!isNumber && (input != ""))    // 숫자가 아닌 값을 입력했을 떄
                {
                    return -1;                          // -1을 반환합니다.
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
        static double GetDisplayWidth(string text)  // 한글은 1.5칸 , 나머지는 1칸으로 계산하는 함수
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
        }
        static bool IsKorean(char c)  // 입력된 글자가 한글인지 체크하는 함수
        {
            return (c >= 0xAC00 && c <= 0xD7A3); // 가 ~ 힣
        }

        public static void To(int i)
        {
            Console.Write(new string(' ', i));
        }


        public static void Clear()  // 화면을 청소하는 함수
        {
            for (int i = 1; i < 29; i++)
            {
                Console.SetCursorPosition(0,i);
                Console.Write(new string(' ', 120));
            }
        }


        public static void Select() // 선택지 입력창을 호출하는 함수
        {
            Console.SetCursorPosition(0, 26);
            To(43); Console.Write ("원하시는 행동의 번호를 입력해주세요.\n");
            To(43); Console.Write("▶▶ ");
        }

        public static void Wrong()  // "잘못된 입력입니다."를 출력하는 함수
        {
            Clear();
            Console.SetCursorPosition(0, 14);
            To(53); Console.Write("잘못된 입력입니다.");
            Console.ReadKey();
        }
        public static void Background() // 화면 위 아래의 주사위를 그리는 함수
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
            Console.SetCursorPosition(0, 0);
        }
    }
}
