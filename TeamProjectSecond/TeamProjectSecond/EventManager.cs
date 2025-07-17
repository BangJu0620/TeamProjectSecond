using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;
using TextRPG_Quest_Solution.QuestSystem;
using TextRPG_Quest_Solution.SaveSystem;
using TextRPGQuest.Data.QuestSystem;


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
            var rest = Rest.Instance;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            
            while (true)
            {
                Clear();
                Background();
                Console.SetCursorPosition(0, 2);
                To(55," 주사위 마을");
                Console.WriteLine();
                To(44,"이곳에서 행동을 선택할 수 있습니다.\n\n\n\n");
                To(41,"1. 상태창\n\n");
                To(41,"2. 소지품 확인\n\n");
                To(41,"3. 상점\n\n");
                To(41,"4. 던전\n\n");
                To(41,"5. 휴식하기\n\n");
                To(41,"6. 퀘스트\n\n");
                To(41,"7. 저장 / 불러오기\n\n");
                Select();

                switch (CheckInput())
                {
                    case 1:
                        // 상태보기
                        DisplayStatus();
                        break;
                    case 2:
                        // 소지품 확인
                        Inventory.ShowInventory();
                        break;
                    case 3:
                        // 상점
                        Shop.EnterShop();
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
                        Clear();
                        DisplaySaveLoadUI();
                        break;
                    case null:
                        break;
                    default:
                        Wrong();
                        break;
                }
            }
        }


        public static void DisplayStatus()
        {
            var character = Character.Instance;
            while (true)
            {
                Clear();
                Background();
                Console.SetCursorPosition(0, 2);
                To(56," 상 태 창\n");
                To(45,"캐릭터의 정보를 확인할 수 있습니다.");
                Console.WriteLine("\n\n\n");
                To(41,$"Lv. {character.Level} {character.ClassType}\n\n");
                To(41,$"경험치 {character.Exp}\n\n");  //  / {character.RequiredExp}
                To(41,$"{character.Name}\n\n");
                To(41,$"주사위 수 : {character.DiceCount}\n\n");
                To(41,$"리롤 횟수 : {character.RerollCount}\n\n");
                To(41,$"방어력 : {character.DefensePoint}\n\n");
                To(41,$"생명력 : {character.HealthPoint} / {character.MaxHealthPoint} (+{character.MaxConsumableHealthPoint})\n\n");
                To(41,$"마  력 : {character.ManaPoint} / {character.MaxManaPoint}\n\n");
                Console.SetCursorPosition(0, 24);
                To(43,"1. 스킬 확인         Enter. 돌아가기");
                Select();
             
                switch (CheckInput())
                {
                    case 1:
                        Announce(55,"미구현");
                        break;
                    case null:
                        return;
                    default:
                        Wrong();
                        break;
                }
            }
        }

        public static void DisplaySaveLoadUI()
        {
            while (true)
            {
                Clear();
                Console.SetCursorPosition(0, 3);
                To(53,"세 이 브 / 로 드");
                Console.WriteLine("\n\n");
                To(41,"1. 세이브\n\n");
                To(41,"2. 로드\n\n");
                Console.SetCursorPosition(0, 24);
                To(53,"Enter. 돌아가기");
                Select();
                switch (CheckInput())
                {
                    case null:
                        return;
                    case 1: // 캐릭터, 아이템 정보를 세이브
                        Clear();
                        SaveLoadManager.SaveCharacterData("character.json");
                        SaveLoadManager.SaveItemData("item.json");
                        QuestDatabase.Save("quest.json");
                        Announce(49, "세이브가 완료되었습니다.");
                        break;
                    case 2: // 캐릭터, 아이템 정보를 로드
                        Clear();
                        bool isExistCharacter;   // 캐릭터 저장 파일 있는지 확인하는 불리언  true: 있음, false: 없음
                        bool isExistItem;        // 아이템 저장 파일 있는지 확인하는 불리언  true: 있음, false: 없음
                        bool isExistQuest;
                        isExistCharacter = SaveLoadManager.LoadCharacterData("character.json");
                        isExistItem = SaveLoadManager.LoadItemData("item.json");
                        isExistQuest = QuestDatabase.Load("quest.json");
                        if(!isExistCharacter || !isExistItem || !isExistQuest)   // 셋 중 하나라도 없으면 없다고 출력
                        {
                            Announce(50, "세이브 파일이 없습니다.");
                            break;
                        }
                        Announce(50, "로드가 완료되었습니다.");
                        break;
                    default:
                        Wrong();
                        break;
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

        public static void To(int i)  // 입력된 숫자만큼 띄어쓰기를 해주는 함수
        {
            Console.Write(new string(' ', i));
        }

        public static void To(int i, string text)  //입력된 숫자만큼 띄어쓰기 + 문자열을 출력하는 함수
        {
            Console.Write(new string(' ', i)); Console.Write(text);
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
            To(43,"원하시는 행동의 번호를 입력해주세요.\n");
            To(43,"▶▶ ");
        }

        public static void Wrong()  // "잘못된 입력입니다."를 출력하는 함수
        {
            Clear();
            Background();
            Console.SetCursorPosition(0, 14);
            To(53,"잘못된 입력입니다.");
            Console.ReadKey();
        }

        public static void Announce(int i, string input)    // Wrong의 변형, 출력 위치와 출력 문구를 매개변수로 받아 출력
        {
            Clear();
            Background();
            Console.SetCursorPosition(0, 14);
            To(i,input);
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
