using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;
using TextRPGQuest.QuestSystem;
using TextRPGQuest.SaveSystem;



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

            while (true)
            {
                Console.Clear();
                Clear();
                To(56," 주사위 마을\n\n");
                To(46,"이곳에서 행동을 선택할 수 있습니다.\n\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                To(43,"1. 상태창\n\n");
                To(43,"2. 소지품 확인\n\n");
                To(43,"3. 상점\n\n");
                To(43,"4. 던전\n\n");
                To(43,"5. 휴식하기\n\n");
                To(43,"6. 퀘스트\n\n");
                To(43,"7. 세이브\n\n");
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
                        DungeonEntry.ShowDungeonDifficultySelection();
                        break;
                    case 5:
                        // 휴식하기
                        rest.RestInVillage();
                        break;
                    case 6:
                        // 퀘스트
                        
                        break;
                    case 7:
                        // 저장/불러오기
                        SaveLoadManager.DisplaySaveUI();
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
                To(57," 상 태 창\n\n");
                To(46,"캐릭터의 정보를 확인할 수 있습니다.\n\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                To(43,$"Lv. {character.Level}  {character.ClassType}  {character.Name}\n\n");
                To(43,$"경험치 {character.Exp}\n\n");  // {character.RequiredExp}
                To(43,$"주사위 : {character.DiceCount}개\n\n");
                To(43,$"속  도 : {character.Speed}\n\n");
                To(43,$"방어력 : {character.DefensePoint}\n\n");
                To(43,$"생명력 : {character.HealthPoint} / {character.MaxHealthPoint}\n\n");
                To(43,$"마  력 : {character.ManaPoint} / {character.MaxManaPoint}\n\n");

                Console.SetCursorPosition(0, 24);
                ToS(44,"1. 스킬 확인         Enter. 돌아가기");
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

        public static int? CheckInput()  // 선택을 입력받는 함수
        {
            int number;
            while (true)
            {
                string? input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out number);

                if (input == "")                        return null;
                if (isNumber && number > 0)             return number;
                else if (input == "a" || input == "A")  return -1;
                else if (input == "d" || input == "D")  return -2;
                else                                    return -3;
            }
        }

        public static void To(int i)  // 입력된 숫자만큼 띄어쓰기를 해주는 함수
        {
            Console.Write(new string(' ', i));
        }

        public static void To(int i, string text)  //입력된 숫자만큼 띄어쓰기 + 문자열을 출력하는 함수
        {
            Console.Write(new string(' ', i)); Console.Write(text);
        }

        public static void ToS(int i, string text)  //띄어쓰기 + 문자열 + 컬러 다크옐로우
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string(' ', i)); Console.Write(text);
        }

        public static void Select() // 선택지 입력창을 호출하는 함수
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 26);
            To(44,"원하시는 행동의 번호를 입력해주세요.\n");
            To(44,"▶▶ ");
            Console.ResetColor();
        }

        public static void Wrong()  // "잘못된 입력입니다."를 출력하는 함수
        {
            Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 14);
            To(54,"잘못된 입력입니다.");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static void Announce(int i, string input)    // Wrong의 변형, 출력 위치와 출력 문구를 매개변수로 받아 출력
        {
            Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 14);
            To(i,input);
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static void Clear() // 화면을 청소하는 함수
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            string[] repeated = { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" };
            for (int j = 0; j < 6; j++)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 60; i++)
                {
                    Console.Write(repeated[i % 6]);
                    Console.Write(" ");
                }

                for (int i = 1; i < 28; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(new string(' ', 120));
                }

                Console.SetCursorPosition(0, 29);
                for (int i = 59; i > 0; i--)
                {
                    Console.Write(repeated[(i + 5) % 6]);
                    Console.Write(" ");
                }
                Console.Write(repeated[0]);
            }
            Console.SetCursorPosition(0, 2);
        }
    }
}
