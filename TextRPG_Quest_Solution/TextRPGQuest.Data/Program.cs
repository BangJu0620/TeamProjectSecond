
using TextRPGQuest.PlayerSystem;
using TextRPGQuest.QuestSystem;
using TextRPGQuest.SaveSystem;

class Program
{
    static void Main()
    {
        Player player = new Player();
        SaveManager.Load(player);

        while (true)
        {
            Console.WriteLine("\n1. 퀘스트 확인\n2. 퀘스트 수락\n3. 진행(몬스터 처치/아이템 수집/던전 클리어)\n4. 저장\n5. 종료");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    QuestBoard.ShowQuests();
                    break;
                case "2":
                    Console.Write("퀘스트 ID 입력: ");
                    int id = int.Parse(Console.ReadLine());
                    QuestBoard.AcceptQuest(id);
                    break;
                case "3":
                    SimulateProgress(player);
                    break;
                case "4":
                    SaveManager.Save(player);
                    break;
                case "5":
                    return;
            }
        }
    }

    static void SimulateProgress(Player player)
    {
        foreach (var quest in QuestDatabase.Quests)
        {
            if (quest.Status == QuestStatus.InProgress)
            {
                quest.CurrentCount++;
                quest.CheckComplete();

                if (quest.Status == QuestStatus.Completed)
                {
                    quest.Status = QuestStatus.Rewarded;
                    player.AddGold(quest.RewardGold);
                    player.AddExp(quest.RewardExp);
                    Console.WriteLine($"{quest.Title} 완료! 골드 {quest.RewardGold}, 경험치 {quest.RewardExp} 획득.");
                }
            }
        }
    }
}
