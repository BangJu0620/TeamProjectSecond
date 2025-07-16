

//Player 클래스 정의
public class Player
{
    public string Name { get; set; } // 플레이어 이름
    public int Level { get; set; } // 플레이어 레벨
    public int Gold { get; set; } // 플레이어 골드
    public int Exp { get; set; } // 플레이어 경험치
    public List<Quest> ActiveQuest { get; set; } = new(); // 플레이어가 수락한 퀘스트 목록
    public Player(string name)
    {
        Name = name;
        Level = 1;
        Gold = 0;
        Exp = 0;
    }
    public void DisplayStatus()
    {
        Console.WriteLine($"플레이어: {Name}, 레벨: {Level}, 골드: {Gold}, 경험치: {Exp}");
    }
}



//퀘스트 enum 및 클래스 정의

public enum QuestStatus
{
    NotAccepted,
    InProgress,
    Completed,
    Rewarded
}

public enum QuestType
{
    killQuest, // 몬스터 처치 퀘스트
    collectQuest, // 아이템 수집 퀘스트
    clearDungeonQuest, // 던전 클리어 퀘스트
}


public class Quest
{
    public int Id { get; set; } //퀘스트 ID
    public string Title { get; set; } //퀘스트 이름
    public string Description { get; set; } //퀘스트 설명
    public QuestStatus Status { get; set; } = QuestStatus.NotAccepted; //퀘스트 상태
    public QuestType Type { get; set; } //퀘스트 종류
    public int TargetAmount { get; set; } //퀘스트 목표 수량
    public int CurrentAmount { get; set; } //현재 진행 중인 수량
    public int RewardGold { get; set; } //퀘스트 완료 시 보상 골드
    public int RewardExp { get;  set; } //퀘스트 완료 시 보상 경험치

    public bool IsCompleted => CurrentAmount >= TargetAmount; //퀘스트 완료 여부 


    public void Progress()
    { 
        if (Status == QuestStatus.InProgress && !IsCompleted)
        {
            CurrentAmount++;
            if (IsCompleted)
            {
                Status = QuestStatus.Completed;
                Console.WriteLine($"퀘스트 '{Title}'이(가) 완료되었습니다!");
            }
        }
    }

    public void GiveReward(Player player)
    {
        if (Status == QuestStatus.Completed)
        {

            player.Gold += RewardGold; // 플레이어에게 골드 지급
            player.Exp += RewardExp; // 플레이어에게 경험치 지급
            Status = QuestStatus.Rewarded;
            Console.WriteLine($"퀘스트 '{Title}'의 보상을 받았습니다! 골드: {RewardGold}, 경험치: {RewardExp}");
        }
        else
        {
            Console.WriteLine($"퀘스트 '{Title}'이(가) 완료되지 않았습니다. 보상을 받을 수 없습니다.");
        }
    }
}

public static class QuestDatabase // 퀘스트 목록
{
    private static List<Quest> quests = new List<Quest>();
    public static void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }
    public static Quest GetQuestById(int id)
    {
        return quests.FirstOrDefault(q => q.Id == id);
    }
    public static List<Quest> GetAllQuests()
    {
        return quests;
    }
}

public static class QuestBoard
{
    public static void DisplayQuests()
    {
        Console.WriteLine("퀘스트 목록:");
        foreach (var quest in QuestDatabase.GetAllQuests())
        {
            Console.WriteLine($"ID: {quest.Id}, 제목: {quest.Title}, 상태: {quest.Status}, 목표: {quest.TargetAmount}, 진행: {quest.CurrentAmount}");
        }
    }
    public static void AcceptQuest(Player player, int questId)
    {
        var quest = QuestDatabase.GetQuestById(questId);
        if (quest != null && quest.Status == QuestStatus.NotAccepted)
        {
            quest.Status = QuestStatus.InProgress;
            player.ActiveQuest.Add(quest);
            Console.WriteLine($"퀘스트 '{quest.Title}'을(를) 수락했습니다.");
        }
        else
        {
            Console.WriteLine("퀘스트를 수락할 수 없습니다.");
        }
    }


    public static void KillMonster(Player player, Quest quest)
    {
        if (quest.Type == QuestType.killQuest && quest.Status == QuestStatus.InProgress)
        {
            quest.Progress();
            Console.WriteLine($"몬스터를 처치했습니다. 현재 진행 상황: {quest.CurrentAmount}/{quest.TargetAmount}");
        }
        else
        {
            Console.WriteLine("퀘스트가 진행 중이 아니거나 잘못된 퀘스트 유형입니다.");
        }
    }

    public static void CollectItem(Player player, Quest quest)
    {
        if (quest.Type == QuestType.collectQuest && quest.Status == QuestStatus.InProgress)
        {
            quest.Progress();
            Console.WriteLine($"아이템을 수집했습니다. 현재 진행 상황: {quest.CurrentAmount}/{quest.TargetAmount}");
        }
        else
        {
            Console.WriteLine("퀘스트가 진행 중이 아니거나 잘못된 퀘스트 유형입니다.");
        }
    }

    public static void ClearDungeon(Player player, Quest quest)
    {
        if (quest.Type == QuestType.clearDungeonQuest && quest.Status == QuestStatus.InProgress)
        {
            quest.Progress();
            Console.WriteLine($"던전을 클리어했습니다. 현재 진행 상황: {quest.CurrentAmount}/{quest.TargetAmount}");
        }
        else
        {
            Console.WriteLine("퀘스트가 진행 중이 아니거나 잘못된 퀘스트 유형입니다.");
        }
    }




}