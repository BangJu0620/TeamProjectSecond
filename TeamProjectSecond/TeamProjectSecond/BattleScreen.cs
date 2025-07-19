using System;
using System.Collections.Generic;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public static class BattleScreen
    {
        private static Queue<string> logQueue = new Queue<string>();

        static BattleScreen()
        {
            Console.Clear();
            Console.SetWindowSize(120, 30); // 안정적인 레이아웃 확보
            Console.CursorVisible = false;
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void DrawMonsterArea(List<Monster> monsters)
        {
            int totalWidth = monsters.Count * 24;
            int startX = (60 - totalWidth) / 2;

            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                int x = startX + i * 24;
                int y = 2;

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Cyan;
                for (int j = 0; j < 3; j++)
                {
                    Console.SetCursorPosition(x, y + j);
                    Console.Write(new string(' ', 12));
                }
                Console.ResetColor();

                Console.SetCursorPosition(x, y + 4);
                Console.Write($"[{m.Name}]  HP: {m.CurrentHP} / {m.MaxHP}");

                Console.SetCursorPosition(x, y + 5);
                int barWidth = 12;
                int filled = (int)((m.CurrentHP / (float)m.MaxHP) * barWidth);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(new string(' ', filled));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(new string(' ', barWidth - filled));
                Console.ResetColor();
            }
        }

        public static void Log(string message)
        {
            if (logQueue.Count >= 6) logQueue.Dequeue();
            logQueue.Enqueue(message);
            DrawLog();
        }

        public static void DrawLog()
        {
            int yStart = 7;
            Console.ForegroundColor = ConsoleColor.Gray;
            int i = 0;
            foreach (var line in logQueue)
            {
                Console.SetCursorPosition(4, yStart + i);
                Console.Write(new string(' ', 90));
                Console.SetCursorPosition(4, yStart + i);
                Console.Write($"• {line}");
                i++;
            }
            Console.ResetColor();
        }

        public static void DrawDie(int value, int x, int y, ConsoleColor dotColor)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = dotColor;

            for (int i = 0; i < 7; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(new string(' ', 7));
            }

            (int dx, int dy)[] dots = GetDieDots(value);
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var (dx, dy) in dots)
            {
                Console.SetCursorPosition(x + dx, y + dy);
                Console.Write(" ");
            }

            Console.ResetColor();
        }

        private static (int, int)[] GetDieDots(int value)
        {
            return value switch
            {
                1 => new[] { (3, 3) },
                2 => new[] { (1, 1), (5, 5) },
                3 => new[] { (1, 1), (3, 3), (5, 5) },
                4 => new[] { (1, 1), (1, 5), (5, 1), (5, 5) },
                5 => new[] { (1, 1), (1, 5), (5, 1), (5, 5), (3, 3) },
                6 => new[] { (1, 1), (1, 3), (1, 5), (5, 1), (5, 3), (5, 5) },
                _ => new (int, int)[0],
            };
        }

        public static void DrawDiceRow(List<int> values, int startX, int y, string label)
        {
            Console.SetCursorPosition(startX, y - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(label);
            Console.ResetColor();

            for (int i = 0; i < values.Count; i++)
            {
                DrawDie(values[i], startX + i * 10, y, ConsoleColor.Black);
            }
        }

        public static void DrawDDTotal(int total, int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(x, y);
            Console.Write("< DD 총합 >");

            Console.SetCursorPosition(x, y + 1);
            Console.Write($"     {total}");
            Console.ResetColor();
        }

        public static void DrawCommandOptions(int rerollLeft)
        {
            int y = 30;
            Console.SetCursorPosition(4, y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1. Go!    2. Reroll ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"({rerollLeft} left)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("    3. 아이템");
            Console.ResetColor();
        }
    }
}