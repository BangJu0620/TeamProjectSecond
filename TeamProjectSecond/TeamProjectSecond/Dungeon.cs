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

    public class Dungeon
    {
        public Difficulty CurrentDifficulty { get; private set; }
        public int CurrentStageIndex { get; private set; } = 0;

        private List<DungeonStage> stages;

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

        private List<DungeonStage> GetStagesForDifficulty(Difficulty difficulty)
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

            Console.WriteLine(stage.IsBoss ? "보스 몬스터가 나타났다!" : "몬스터가 나타났다!");

            List<Monster> enemies = GenerateMonstersForCurrentStage();

            // 전투 시작
            // Battle.StartBattle(enemies);
        }
    }
}