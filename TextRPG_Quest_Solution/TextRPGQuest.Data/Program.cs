using System;
using TextRPGQuest.QuestSystem;
using TextRPGQuest.SaveSystem;

namespace TextRPGQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            //QuestDatabase.Initialize();

            //bool running = true;

            //while (running)
            //{
            //    Console.Clear();
            //    Console.WriteLine("===== Text RPG Quest =====");
            //    Console.WriteLine("1. 퀘스트 목록 보기");
            //    Console.WriteLine("2. 퀘스트 수락하기");
            //    Console.WriteLine("3. 몬스터 처치 (진행도)");
            //    Console.WriteLine("4. 던전 클리어 (진행도)");
            //    Console.WriteLine("5. 보상 받기");
            //    Console.WriteLine("6. 저장 및 종료");

            //    Console.Write("\n메뉴를 선택하세요: ");
            //    string input = Console.ReadLine();

            //    switch (input)
            //    {
            //        case "1":
            //            QuestBoard.ShowQuests();
            //            Pause();
            //            break;

            //        case "2":
            //            QuestBoard.ShowQuests();
            //            Console.Write("\n수락할 퀘스트 ID를 입력하세요: ");
            //            if (int.TryParse(Console.ReadLine(), out int id))
            //            {
            //                QuestBoard.AcceptQuest(id);
            //            }
            //            Pause();
            //            break;

            //        case "3":
            //            QuestBoard.UpdateQuestProgress(QuestCategory.Hunt, 1);
            //            Console.WriteLine("몬스터를 처치했습니다!");
            //            Pause();
            //            break;

            //        case "4":
            //            QuestBoard.UpdateQuestProgress(QuestCategory.Explore, 1);
            //            Console.WriteLine("던전을 클리어했습니다!");
            //            Pause();
            //            break;

            //        case "5":
            //            QuestBoard.ShowQuests();
            //            Console.Write("\n보상 받을 퀘스트 ID를 입력하세요: ");
            //            if (int.TryParse(Console.ReadLine(), out int rewardId))
            //            {
            //                QuestBoard.ReceiveReward(rewardId);
            //            }
            //            Pause();
            //            break;

            //        case "6":
            //            QuestDatabase.SavePlayerQuests();
            //            running = false;
            //            break;

            //        default:
            //            Console.WriteLine("잘못된 입력입니다.");
            //            Pause();
            //            break;
            //    }
            //}

            //Console.WriteLine("게임을 종료합니다.");
        }

        //static void Pause()
        //{
        //    Console.WriteLine("\n엔터를 눌러 계속...");
        //    Console.ReadLine();
        //}
    }
}

