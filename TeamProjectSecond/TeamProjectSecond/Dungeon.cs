using System;
using System.Collections.Generic;

namespace TeamProjectSecond
{
    public enum Difficulty
    {
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    public class DungeonStage
    {
        public Difficulty Difficulty { get; private set; }
        public int StageNumber { get; private set; }
        public List<int> StageRank { get; private set; }
        public bool IsBoss { get; private set; }

        public DungeonStage(Difficulty difficulty, int stageNumber, List<int> possibleRanks, bool isBoss = false)
        {
            Difficulty = difficulty;
            StageNumber = stageNumber;
            StageRank = possibleRanks;
            IsBoss = isBoss;
        }

        public int GetRandomStageRank()
        {
            Random rand = new Random();
            return StageRank[rand.Next(StageRank.Count)];
        }
    }

    public static class DungeonEntry
    {
        public static void ShowDungeonDifficultySelection()
        {
            while (true)
            {
                EventManager.Clear();
                EventManager.To(56, "던 전 입 장\n\n");
                EventManager.To(44, "입장할 던전의 난이도를 선택하세요.\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                EventManager.To(43, "1. 쉬움 (Easy)\n\n");
                EventManager.To(43, "2. 보통 (Normal)\n\n");
                EventManager.To(43, "3. 어려움 (Hard)\n\n");

                Console.SetCursorPosition(0, 24);
                EventManager.ToS(44, "Enter. 돌아가기\n");
                EventManager.Select();

                int? input = EventManager.CheckInput();

                switch (input)
                {
                    case 1:
                        new Dungeon(Difficulty.Easy).Enter();
                        return;
                    case 2:
                        new Dungeon(Difficulty.Normal).Enter();
                        return;
                    case 3:
                        new Dungeon(Difficulty.Hard).Enter();
                        return;
                    case null:
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                }
            }
        }
    }
    public class Dungeon
    {
        public Difficulty CurrentDifficulty { get; private set; }
        public int CurrentStageIndex { get; private set; } = 0;

        private List<DungeonStage> stages;
        public void Enter() // *************** 던전 입장 메서드 ********** Dungeon dungeon = new Dungeon(Difficulty.Easy혹은Normal혹은Hard); 그리고 dungeon.Enter(); 누르면 던전 들갑니당.
        {
            CurrentStageIndex = 0;
            StartCurrentStage();
        }

        public Dungeon(Difficulty difficulty)
        {
            CurrentDifficulty = difficulty;
            stages = GetStagesForDifficulty(difficulty);
        }

        public bool HasNextStage()
        {
            return CurrentStageIndex < stages.Count;
        }

        public DungeonStage GetCurrentStage()
        {
            return stages[CurrentStageIndex];
        }

        public void ProceedToNextStage()
        {
            if (HasNextStage())
                CurrentStageIndex++;
        }

        public List<Monster> GenerateMonstersForCurrentStage()
        {
            DungeonStage stage = GetCurrentStage();
            int rankSum = stage.GetRandomStageRank();

            return Monster.Gen(rankSum, rankSum, GetMaxRankAllowed());
        }

        private int GetMaxRankAllowed()
        {
            return CurrentDifficulty switch
            {
                Difficulty.Easy => 5,
                Difficulty.Normal => 7,
                Difficulty.Hard => 10,
                _ => 5
            };
        }

        private List<DungeonStage> GetStagesForDifficulty(Difficulty difficulty)  // 던전 스테이지의 랭크값
        {
            List<DungeonStage> result = new();

            switch (difficulty)
            {
                case Difficulty.Easy:
                    result.Add(new DungeonStage(difficulty, 1, new() { 2 }));
                    result.Add(new DungeonStage(difficulty, 2, new() { 2, 3 }));
                    result.Add(new DungeonStage(difficulty, 3, new() { 3 }));
                    result.Add(new DungeonStage(difficulty, 4, new() { 3, 4 }));
                    result.Add(new DungeonStage(difficulty, 5, new() { 4 }));
                    break;

                case Difficulty.Normal:
                    result.Add(new DungeonStage(difficulty, 1, new() { 4 }));
                    result.Add(new DungeonStage(difficulty, 2, new() { 4, 5 }));
                    result.Add(new DungeonStage(difficulty, 3, new() { 5 }));
                    result.Add(new DungeonStage(difficulty, 4, new() { 5, 6 }));
                    result.Add(new DungeonStage(difficulty, 5, new() { 6 }));
                    result.Add(new DungeonStage(difficulty, 6, new() { 7 }, true)); // 보스
                    break;

                case Difficulty.Hard:
                    result.Add(new DungeonStage(difficulty, 1, new() { 6 }));
                    result.Add(new DungeonStage(difficulty, 2, new() { 6, 7 }));
                    result.Add(new DungeonStage(difficulty, 3, new() { 7 }));
                    result.Add(new DungeonStage(difficulty, 4, new() { 7, 8 }));
                    result.Add(new DungeonStage(difficulty, 5, new() { 8 }));
                    result.Add(new DungeonStage(difficulty, 6, new() { 8 }));
                    result.Add(new DungeonStage(difficulty, 7, new() { 10 }, true)); // 보스
                    break;
            }

            return result;
        }

        public void StartCurrentStage()
        {
            DungeonStage stage = GetCurrentStage();
            List<Monster> enemies = GenerateMonstersForCurrentStage();

            // 전투 시작
            Battle.StartBattle(enemies);
        }
    }
}