using System;
using System.Collections.Generic;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public static class BattleScreen
    {
        private static Character player => Character.Instance;

        static BattleScreen()
        {
            Clear();
            Console.ReadKey();
            Console.SetWindowSize(120, 30);
            Console.CursorVisible = false;
        }

        // ─────────────── UI UTILITIES ───────────────

        public static void To(int x, int y, ConsoleColor color, string text) // 입력된 숫자만큼 띄어쓰기를 해주는 함수
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        public static void To(int x, int y, ConsoleColor color, ConsoleColor backcolor, string text) // 입력된 숫자만큼 띄어쓰기를 해주는 함수
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.BackgroundColor = backcolor;
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
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(new string(' ', total - filled));
            Console.ResetColor();
        }

        // ─────────────── 전투 화면 영역 ───────────────
        public static void Clear() // 전투 화면 프레임 으로 초기화
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
                for (int i = 19; i < 28; i++)  // UI벽1
                {
                    Console.SetCursorPosition(24, i + 1);
                    Console.Write("|");
                }
                for (int i = 19; i < 28; i++)  // UI벽2
                {
                    Console.SetCursorPosition(76, i + 1);
                    Console.Write("|");
                }
                for (int i = 19; i < 28; i++)  // UI벽3
                {
                    Console.SetCursorPosition(88, i + 1);
                    Console.Write("|");
                }
                
                
                To(6, 20, ConsoleColor.Cyan, "Strike Dice");
                To(45, 20, ConsoleColor.Cyan, "Damage Dice");
                To(80, 20, ConsoleColor.Cyan, "합 계");
                To(92, 20, ConsoleColor.Yellow, "번호를 눌러 행동을 선택 !");
                UpdateHPMP();
            }
            Console.SetCursorPosition(0, 2);
        }
        //HP MP Bar그리기
        public static void UpdateHPMP()
        {
            int filled = 1;
            int barWidth = 20;

            filled = (int)((player.HealthPoint / (float)player.MaxHealthPoint) * barWidth);
            To(52, 17, ConsoleColor.White, "HP");
            To(54, 17, ConsoleColor.DarkGray, "|");
            Bar(55, 17, filled, barWidth, ConsoleColor.Red);
            To(75, 17, ConsoleColor.DarkGray, "|");
            To(77, 17, ConsoleColor.White, $"{player.HealthPoint}");
            To(81, 17, ConsoleColor.White, "/");
            To(83, 17, ConsoleColor.White, $"{player.MaxHealthPoint}");
            filled = (int)((player.ManaPoint / (float)player.MaxManaPoint) * barWidth);
            To(52, 18, ConsoleColor.White, "MP");
            To(54, 18, ConsoleColor.DarkGray, "|");
            Bar(55, 18, filled, barWidth, ConsoleColor.Blue);
            To(75, 18, ConsoleColor.DarkGray, "|");
            To(77, 18, ConsoleColor.White, $"{player.ManaPoint}");
            To(81, 18, ConsoleColor.White, "/");
            To(83, 18, ConsoleColor.White, $"{player.MaxManaPoint}");
        }
        public static void UpdateCurrentStage(Dungeon dungeon)
        {
            int stage = dungeon.GetCurrentStage().StageNumber;
            var d = dungeon.GetCurrentStage();
            To(76, 2, ConsoleColor.Gray, $"<지하 {stage}층>");
        }


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

        /// ///////////////////////////////////////////////////////////////////////몬스터 UI

        public static void DrawMonsterArea(List<Monster> monsters)
        {
            int totalWidth = monsters.Count * 20;
            int startX = (60 - totalWidth) / 2;

            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                int x = startX + i * 20;
                int y = 2;

                Fill(x + 3, y, 6, 3, ConsoleColor.Cyan);
                To(x, y + 4, ConsoleColor.White, $"[{m.Name}]  HP: {m.CurrentHP} / {m.MaxHP}");

                int barWidth = 12;
                int filled = (int)((m.CurrentHP / (float)m.MaxHP) * barWidth);
                Bar(x, y + 5, filled, barWidth, ConsoleColor.Red);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////// 주사위 UI
        public static void DrawDie(int value, int x, int y, ConsoleColor dicecolor, ConsoleColor dotcolor)
        {
            string[] lines = value switch
            {
                1 => new[] { "      ", "  ●  ", "      ", "      ", " < 1> " },
                2 => new[] { "●    ", "      ", "    ●", "      ", " < 2> " },
                3 => new[] { "    ●", "  ●  ", "●    ", "      ", " < 3> " },
                4 => new[] { "●  ●", "      ", "●  ●", "      ", " < 4> " },
                5 => new[] { "●  ●", "  ●  ", "●  ●", "      ", " < 5> " },
                6 => new[] { "●  ●", "●  ●", "●  ●", "      ", " < 6> " },
                7 => new[] { "●  ●", "●●●", "●  ●", "      ", " < 7> " }, // Rogue 전용
                _ => new[] { "??????", "??????", "??????", "      ", " <??> " }
            };

            Console.BackgroundColor = dicecolor;
            Console.ForegroundColor = dotcolor;

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(x, y + i);
                string line = lines[i];
                foreach (char c in line)
                {
                    Console.Write(c);
                }
            }
            Console.ResetColor();
            for (int i = 3; i < 5; i++)
            {
                Console.SetCursorPosition(x, y + i);
                string line = lines[i];
                foreach (char c in line)
                {
                    Console.Write(c);
                }
            }
        }

        public static void DrawSD(List<int> values)
        {
            int startX = 5;
            int y = 23;
            for (int i = 0; i < values.Count; i++)
                DrawDie(values[i], startX + i * 8, y, ConsoleColor.DarkGreen, ConsoleColor.White);  // 가로 간격 16칸으로 여유 있게
        }
        public static void DrawDD(List<int> values)
        {
            int startX = 47;
            int ddcount = values.Count;
            //if (ddcount == 2)
            //    startX -= 5;
            //if (ddcount == 3)
            //    startX -= 11;

            int y = 23;
            for (int i = 0; i < ddcount; i++)
                DrawDie(values[i], startX + i * 8, y, ConsoleColor.Yellow, ConsoleColor.Black);  // 가로 간격 16칸으로 여유 있게
        }
        //public static void DrawDDTotal(int total, int x, int y)
        //{

        //}

        public static void DrawCommandOptions(string firstOption, string secondOption, string thirdOption)
        {
            int x = 92;
            int y = 23;
            To(x, y, ConsoleColor.White, $"(1) {firstOption}");
            To(x, y + 2, ConsoleColor.White, $"(2) {secondOption}");
            To(x, y + 4, ConsoleColor.White, $"(3) {thirdOption}");
            To(x + 15, y + 5, ConsoleColor.Gray, "선택 : ");
        }




    }
}