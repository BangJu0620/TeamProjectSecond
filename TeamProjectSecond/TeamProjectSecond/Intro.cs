using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextRPGQuest.QuestSystem;

namespace TeamProjectSecond
{
    public static class Intro
    {
        public static void DisplayTitle(ClassTypeChange classTypeChange)
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.To(58, "타  이  틀\n\n");

                EventManager.To(42, "1. 새로 시작\n\n\n\n\n");
                EventManager.To(42, "2. 이어하기\n\n\n\n\n");
                EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case 1:
                        QuestDatabase.RegisterDefaultQuests();  // 퀘스트 생성
                        SetName();                  // 이름 받기
                        SetClass(classTypeChange);  // 클래스 설정
                        return;
                    case 2:
                        if (SaveLoadManager.CheckExistSaveData())   // 세이브 파일 존재하는지 확인
                        {
                            EventManager.Announce(50, "세이브 파일이 없습니다.");
                            break;
                        }
                        if (SaveLoadManager.CheckEmptySaveData())   // 세이브 파일 비어있는지 확인
                        {
                            EventManager.Announce(48, "세이브 파일이 비어있습니다.");
                            break;
                        }
                        SaveLoadData.LoadAllData("save.json");
                        EventManager.Announce(50, "다시 오신 걸 환영합니다.");
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void SetName()    // 이름 입력하기
        {
            while (true)
            {
                string name = WriteName();          // 이름 입력받기
                int userSelect = CheckName(name);   // 입력한 이름 맞는지 확인
                if (userSelect == 1) break;
            }
        }

        public static string WriteName()    // 이름 입력받기
        {
            while (true)
            {
                EventManager.Clear();
                Console.SetCursorPosition(0, 13);
                EventManager.To(53, "이름을 입력해주세요.");
                Console.SetCursorPosition(0, 20);
                EventManager.To(43, "▶▶ ");
                string? name = Console.ReadLine();
                if (name == "") { }
                else return name;
            }
        }

        public static int CheckName(string name)    // 입력한 이름 맞는지 확인
        {
            while (true)
            {
                EventManager.Clear();
                Console.SetCursorPosition(0, 12);
                EventManager.To(58, $"{name}\n\n\n");
                EventManager.To(50, "이대로 진행하시겠습니까?\n\n\n");
                EventManager.To(46, "1. 진행하기     Enter. 다시 입력");
                Console.SetCursorPosition(0, 20);
                EventManager.To(43, "▶▶ ");

                int? input = EventManager.CheckInput();

                switch (input)
                {
                    case 1:
                        Character.Instance.Name = name;
                        return 1;
                    case null:
                        return -1;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }

        public static void SetClass(ClassTypeChange classTypeChange)    // 클래스 선택하기
        {
            while (true)
            {
                SelectClass(classTypeChange);   // 클래스 입력받기
                int userSelect = CheckClass();  // 클래스 맞는지 확인하기
                if (userSelect == 1) break;     // 일치할 경우 break
            }
        }

        public static void SelectClass(ClassTypeChange classTypeChange) // 클래스 입력하기
        {
            while (true)
            {
                EventManager.Clear();
                Console.SetCursorPosition(0, 7);
                EventManager.To(52, "직업을 선택해주세요.\n\n\n");
                EventManager.To(46, "1. Warrior\n\n\n");
                EventManager.To(46, "2. Mage\n\n\n");
                EventManager.To(46, "3. Rogue\n\n\n");
                Console.SetCursorPosition(0, 20);
                EventManager.To(43, "▶▶ ");

                string input = Console.ReadLine();
                bool isInt = int.TryParse(input, out int userSelect);

                if (isInt)
                {
                    if (userSelect == 1)
                    { classTypeChange.PromoteToWarrior(); break; }
                    else if (userSelect == 2)
                    { classTypeChange.PromoteToMage();    break; }
                    else if (userSelect == 3)
                    { classTypeChange.PromoteToRogue();   break; }
                    else EventManager.Wrong();
                }
                else EventManager.Wrong();
            }
        }

        public static int CheckClass()  // 입력한 클래스 확인하기
        {
            while (true)
            {
                EventManager.Clear();
                Console.SetCursorPosition(0, 12);
                EventManager.To(58, $"{Character.Instance.ClassType}\n\n\n");
                EventManager.To(50, "이대로 진행하시겠습니까?\n\n\n");
                EventManager.To(46, "1. 진행하기     Enter. 다시 입력");
                Console.SetCursorPosition(0, 20);
                EventManager.To(43, "▶▶ ");

                int? input = EventManager.CheckInput();

                switch (input)
                {
                    case 1:
                        return 1;
                    case null:
                        return -1;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }
    }
}
