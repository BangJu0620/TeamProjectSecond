
using TextRPGQuest.PlayerSystem;
using TextRPGQuest.QuestSystem;
using TextRPGQuest.SaveSystem;
using System;

namespace TextRPGQuest
{
    /// <summary>
    /// 프로그램의 시작 지점입니다.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 초기 퀘스트 데이터 로드 (또는 초기화)
            QuestDatabase.Load(); // json 불러오기 (처음 실행 시 파일 없으면 자동 생성됨)


            // 게임 루프 시작
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== Text RPG Quest =====");
                Console.WriteLine("1. 퀘스트 목록 보기");
                Console.WriteLine("2. 퀘스트 수락하기");
                Console.WriteLine("3. 게임 종료");

        //QuestDatabase.Register();
        QuestDatabase.Load("quest.json");


                Console.Write("\n메뉴를 선택하세요: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        QuestBoard.ShowQuests(); // 퀘스트 목록 출력
                        Pause(); // 멈춤 대기
                        break;

                    case "2":
                        QuestBoard.ShowQuests(); // 먼저 퀘스트 보여주기
                        Console.Write("\n수락할 퀘스트 ID를 입력하세요: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            QuestBoard.AcceptQuest(id); // 퀘스트 수락
                        }
                        else
                        {
                            Console.WriteLine("숫자를 입력하세요!");
                        }
                        Pause();
                        break;

                    case "3":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Pause();
                        break;
                }
            }

            // 종료 시 퀘스트 저장
            QuestDatabase.Save();

            Console.WriteLine("게임을 종료합니다.");
        }

        /// <summary>
        /// 잠깐 멈추는 함수 (엔터 입력 대기)
        /// </summary>
        static void Pause()
        {
            Console.WriteLine("\n엔터를 눌러 계속...");
            Console.ReadLine();
        }
    }
}

