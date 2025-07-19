using System;
using System.Collections.Generic;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public static class BattleScreen
    {


        static BattleScreen()
        {
            Clear();
            Console.SetWindowSize(120,30);
            Console.CursorVisible = false;
        }

        // ─────────────── UI UTILITIES ───────────────

        public static void Clear() // 전투 화면 프레임
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            string[] repeated = { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" };

            for (int j = 0; j < 6; j++)
            {
                Console.SetCursorPosition(0, 0); // 맨 위
                for (int i = 0; i < 60; i++)
                {
                    Console.Write(repeated[i % 6]);
                    Console.Write(" ");
                }

                for (int i = 1; i < 28; i++) // 맨왼쪽벽띄어쓰기
                {
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(new string(' ', 120));
                }

                Console.SetCursorPosition(0, 19); // UI상단경계
                for (int i = 0; i < 60; i++)
                {
                    Console.Write(repeated[i % 6]);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(0, 21); // UI 내부 이름 칸 하단경계
                for (int i = 0; i < 120; i++)
                {
                    Console.Write("─");
                }

                Console.SetCursorPosition(0, 29); // 맨 바닥
                for (int i = 60; i > 0; i--)
                {
                    Console.Write(repeated[(i + 5) % 6]);
                    Console.Write(" ");
                }
                for (int i = 0; i < 18; i++)  // 전투 로그 구분 벽
                {
                    Console.SetCursorPosition(88, i + 1);
                    Console.Write(repeated[(i + 1) % 6]);
                }
                for (int i = 19; i < 28; i++)  // 전투 로그 구분 벽
                {
                    Console.SetCursorPosition(88, i + 1);
                    Console.Write("|");
                }
            }
            Console.SetCursorPosition(0, 2);
        }

        public static void To(int x, int y, ConsoleColor color, string text) // 입력된 숫자만큼 띄어쓰기를 해주는 함수
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Fill(int x, int y, int width, int height, ConsoleColor bg) //입력된 숫자만큼 띄어쓰기 + 문자열을 출력하는 함수
        {
            Console.BackgroundColor = bg;
            for (int j = 0; j < height; j++)
            {
                Console.SetCursorPosition(x, y + j);
                Console.Write(new string(' ', width));
            }
            Console.ResetColor();
        }

        public static void Bar(int x, int y, int filled, int total, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = color;
            Console.Write(new string(' ', filled));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(new string(' ', total - filled));
            Console.ResetColor();
        }

        // ─────────────── 전투 화면 영역 ───────────────

        ////// 전투로그 처리

        private static Queue<string> logQueue = new Queue<string>();
        public static void Log(string message) 
        {
            if (logQueue.Count >= 6) logQueue.Dequeue();
            logQueue.Enqueue(message);
            DrawLog();
        }

        public static void DrawLog()
        {
            int xStart = 90;           // 조금 더 오른쪽으로
            int yStart = 2;            // 위에서부터 출력
            int maxLines = 15;          // 최대 출력 줄 수
            int lineWidth = 16;        // 한 줄 너비 제한

            int lineCounter = 0;

            foreach (var fullLine in logQueue)
            {
                // 긴 줄은 줄바꿈 처리
                List<string> wrappedLines = WrapText(fullLine, lineWidth);

                foreach (var wrapped in wrappedLines)
                {
                    if (lineCounter >= maxLines) return;

                    To(xStart, yStart + lineCounter, ConsoleColor.Gray, $" {wrapped.PadRight(lineWidth)}");
                    lineCounter++;
                }
            }
        }
        private static List<string> WrapText(string text, int maxLength)
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < text.Length; i += maxLength)
            {
                int len = Math.Min(maxLength, text.Length - i);
                lines.Add(text.Substring(i, len));
            }

            return lines;
        }
        // 몬스터 UI
        public static void DrawMonsterArea(List<Monster> monsters)
        {
            int totalWidth = monsters.Count * 28;
            int startX = (60 - totalWidth) / 2;

            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                int x = startX + i * 28;
                int y = 2;

                Fill(x+3, y, 6, 3, ConsoleColor.Cyan);
                To(x, y + 4, ConsoleColor.White, $"[{m.Name}]  HP: {m.CurrentHP} / {m.MaxHP}");

                int barWidth = 12;
                int filled = (int)((m.CurrentHP / (float)m.MaxHP) * barWidth);
                Bar(x, y + 5, filled, barWidth, ConsoleColor.Red);
            }
        }

        // 주사위 UI
        public static void DrawDie(int value, int x, int y, ConsoleColor dotColor)
        {
            Fill(x, y, 14, 7, ConsoleColor.White);

            (int dx, int dy)[] dots = GetDieDots(value);
            foreach (var (dx, dy) in dots)
            {
                Fill(x + dx * 2, y + dy, 2, 1, ConsoleColor.Black);
            }
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
            To(startX, y - 1, ConsoleColor.White, label);
            for (int i = 0; i < values.Count; i++)
                DrawDie(values[i], startX + i * 16, y, ConsoleColor.Black);  // 가로 간격 16칸으로 여유 있게
        }

        public static void DrawDDTotal(int total, int x, int y)
        {
            To(x, y, ConsoleColor.Cyan, "< DD 총합 >");
            To(x, y + 1, ConsoleColor.Cyan, $"     {total}");
        }

        public static void DrawCommandOptions(int rerollLeft)
        {
            int y = 30;
            To(4, y, ConsoleColor.White, "1. Go!    2. Reroll ");
            To(Console.CursorLeft, y, ConsoleColor.Yellow, $"({rerollLeft} left)");
            To(Console.CursorLeft, y, ConsoleColor.White, "    3. 아이템");
        }
    }
}