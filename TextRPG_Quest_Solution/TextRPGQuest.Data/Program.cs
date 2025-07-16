
using System.Text.Json;
using TextRPGQuest.Data.TextRPG.Data;

using System;
using TextRPG_Quest_Solution;
using TextRPG_Quest_Solution.QuestSystem;
using TextRPG_Quest_Solution.SaveSystem;


//실행 스크립트 

class Program
{
    static void Main()
    {
        var player = SaveManager.LoadPlayer();

        QuestDatabase.Register();
        QuestDatabase.Load();

        QuestBoard.Show(player); // 퀘스트 수락

        Console.WriteLine("\n슬라임을 5마리 잡았습니다!");
        QuestBoard.ReportProgress(player, monsterKill: 5);

        Console.WriteLine("\n던전을 1번 클리어했습니다!");
        QuestBoard.ReportProgress(player, dungeonClear: 1);

        SaveManager.SaveAll(player);
    }
}
