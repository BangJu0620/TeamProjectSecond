using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public static class LevelManager
    {
        public const int MaxLevel = 20;
        public const int StartLevel = 1;
        public const int BaseExp = 100;
        public const double GrowthRate = 1.2;  // 지수적으로 증가 비율

        public static readonly List<int> ExpRequirements;

        static LevelManager()
        {
            ExpRequirements = ExpTableGenerator(StartLevel, MaxLevel, BaseExp, GrowthRate);
        }

        public static int GetRequiredExp(int level)
        {
            if (level < 1 || level >= MaxLevel)
                return int.MaxValue; // 최종 레벨 이상은 경험치 무의미
            return ExpRequirements[level];
        }

        public static void GainExpWithEffect(int amount)
        {
            var character = Character.Instance;
            int targetTotalExp = character.Exp + amount;

            while (true)
            {
                int requiredExp = GetRequiredExp(character.Level);
                int expBefore = character.Exp;
                int expToGain = Math.Min(requiredExp - expBefore, targetTotalExp - expBefore);
                int steps = 30;
                double perStep = expToGain / (double)steps;

                for (int i = 1; i <= steps; i++)
                {
                    int currentExp = expBefore + (int)(perStep * i);
                    if (currentExp > targetTotalExp) currentExp = targetTotalExp;
                    if (currentExp > requiredExp) currentExp = requiredExp;

                    // 경험치 바 출력 (한 줄에)
                    int barWidth = 30;
                    double ratio = Math.Clamp(currentExp / (double)requiredExp, 0, 1);
                    int filled = (int)(barWidth * ratio);
                    int empty = barWidth - filled;

                    Console.SetCursorPosition(44, 12);
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write(new string(' ', filled));
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write(new string(' ', empty));
                    Console.ResetColor();
                    Console.WriteLine();
                    EventManager.To(44,$"[ {currentExp,7} / {requiredExp,7} ]");

                    Thread.Sleep(20);
                }

                character.Exp += expToGain;

                if (character.Exp >= requiredExp)
                {
                    character.Exp -= requiredExp;
                    character.Level++;
                    character.HealthPoint = character.MaxHealthPoint;
                    character.ManaPoint = character.MaxManaPoint;

                    EventManager.Clear();
                    Console.SetCursorPosition(0, 12);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.White;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    EventManager.To(55, $"🎉 LEVEL UP!\r");
                    Thread.Sleep(30);

                    EventManager.To(55,$"현재 레벨: {character.Level} 🎉");
                    if (character.Level >= MaxLevel) break;
                }
                else
                {
                    Console.ReadKey(true);
                    break;
                }
                Console.ReadKey(true);
                break;
            }
        }

        public static void PrintExpTable()
        {
            for (int i = 1; i < ExpRequirements.Count; i++)
            {
                Console.WriteLine($"레벨 {i + 1} 필요 경험치: {ExpRequirements[i]}");
            }
        }
        private static List<int> ExpTableGenerator(int startLevel, int maxLevel, int baseExp, double growthRate)
        {
            List<int> expTable = new List<int>();
            int totalExp = 0;
            double currentExp = baseExp;

            expTable.Add(0); // 레벨 1의 누적 경험치는 0

            for (int level = startLevel + 1; level <= maxLevel; level++)
            {
                int roundedExp = (int)Math.Floor(currentExp);
                totalExp += roundedExp;
                expTable.Add(totalExp);

                currentExp *= growthRate;
            }

            return expTable;
        }
    }

}
