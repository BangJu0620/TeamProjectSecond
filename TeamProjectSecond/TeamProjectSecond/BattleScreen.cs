using System;
using System.Collections;
using System.Collections.Generic;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public static class BattleScreen
    {
        private static Character player => Character.Instance;

        static BattleScreen()
        {
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
        public static void Wrong()  // "잘못된 입력입니다."를 출력하는 함수
        {
            DrawCommandOptionsClear();
            DrawCommandOptions("잘못된 입력입니다.");
            Console.ReadKey();
        }
        public static void CenteredText(int x, int y, int width, string text, ConsoleColor color, ConsoleColor backcolor)
        {
            int textWidth = GetConsoleTextWidth(text);
            int startX = x + Math.Max(0, (width - textWidth) / 2);

            Console.SetCursorPosition(startX, y);
            Console.ForegroundColor = color;
            Console.BackgroundColor = backcolor;
            Console.Write(text);
            Console.ResetColor();
        }
        private static int GetConsoleTextWidth(string text) // 문자열의 콘솔 너비를 계산 (한글은 2칸, 영어 등은 1칸)
        {
            int width = 0;
            foreach (char c in text)
            {
                if (IsWideChar(c))
                    width += 2;
                else
                    width += 1;
            }
            return width;
        }

        private static bool IsWideChar(char c)
        {
            return c >= 0x1100 && (
                   c <= 0x115F || // Hangul Jamo
                   c == 0x2329 || c == 0x232A ||
                   (c >= 0x2E80 && c <= 0xA4CF && c != 0x303F) || // CJK Radicals
                   (c >= 0xAC00 && c <= 0xD7A3) || // Hangul Syllables
                   (c >= 0xF900 && c <= 0xFAFF) || // CJK Compatibility Ideographs
                   (c >= 0xFE10 && c <= 0xFE19) || // Vertical forms
                   (c >= 0xFE30 && c <= 0xFE6F) || // CJK Compatibility Forms
                   (c >= 0xFF00 && c <= 0xFF60) || // Fullwidth Forms
                   (c >= 0xFFE0 && c <= 0xFFE6));
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
                for (int i = 19; i < 28; i++)  // UI벽3
                {
                    Console.SetCursorPosition(88, i + 1);
                    Console.Write("|");
                }
                BattleDiceUI();
                To(92, 20, ConsoleColor.Yellow, "번호를 눌러 행동을 선택 !");
                UpdateHPMP();
            }
            Console.SetCursorPosition(0, 2);
        }
        public static void BattleDiceUI()
        {
            To(4, 20, ConsoleColor.Cyan, "                                                                    ");
            To(4, 22, ConsoleColor.Cyan, "                                                                    ");
            To(4, 24, ConsoleColor.Cyan, "                                                                    ");
            To(4, 26, ConsoleColor.Cyan, "                                                                    ");
            To(4, 28, ConsoleColor.Cyan, "                                                                    ");
            for (int i = 19; i < 28; i++)  // UI벽1
            {
                Console.SetCursorPosition(19, i + 1);
                Console.Write("|");
            }
            for (int i = 19; i < 28; i++)  // UI벽2
            {
                Console.SetCursorPosition(76, i + 1);
                Console.Write("|");
            }
            To(4, 20, ConsoleColor.Cyan, "Strike Dice");
            To(43, 20, ConsoleColor.Cyan, "Damage Dice");
            To(80, 20, ConsoleColor.Cyan, "합 계");
        }
        public static void BattleSkillUI(List<Skill> skills)
        {
            To(43, 20, ConsoleColor.Cyan, "            ");
            To(43, 20, ConsoleColor.Yellow, " 스킬 목록 ");
            if (skills != null)
            {
                for (int i = 0; i < skills.Count; i++)
                {
                    string skillname = skills[i].Name;
                    int manacost = skills[i].ManaCost;
                    To(20, 22 + 2*i, ConsoleColor.Green, $"<{i + 1}> {skillname}  마나: {manacost}");
                }
            }
            else
            {
                To(48, 25, ConsoleColor.Green, $"사용 가능한 스킬이 없습니다."); 
            }
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
            To(76, 17, ConsoleColor.White, $"   ");
            To(77, 17, ConsoleColor.White, $"{player.HealthPoint}");
            To(81, 17, ConsoleColor.White, "/");
            To(83, 17, ConsoleColor.White, $"{player.MaxHealthPoint}");
            filled = (int)((player.ManaPoint / (float)player.MaxManaPoint) * barWidth);
            To(52, 18, ConsoleColor.White, "MP");
            To(54, 18, ConsoleColor.DarkGray, "|");
            Bar(55, 18, filled, barWidth, ConsoleColor.Blue);
            To(75, 18, ConsoleColor.DarkGray, "|");
            To(76, 18, ConsoleColor.White, $"   ");
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






        public static void UpdateMonsterUI(List<Monster> monsters)
        {
            int cardWidth = 22;                  // 몬스터 1명당 UI 가로 너비
            int totalWidth = monsters.Count * cardWidth;
            int startX = 44 - totalWidth / 2;    // 콘솔 너비가 120 기준 중앙 정렬

            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                int x = startX + i * cardWidth;
                int y = 6;

                if (m.IsDead)
                {
                    CenteredText(x, y - 2, cardWidth, $"Lv.{m.Rank} [{m.Name}]", ConsoleColor.DarkGray, ConsoleColor.Black);
                    Fill(x + 8, y, 6, 3, ConsoleColor.DarkGray);
                    CenteredText(x, y + 1, cardWidth, "X  X", ConsoleColor.Black, ConsoleColor.DarkGray);
                    CenteredText(x, y + 6, cardWidth, "   사망   ", ConsoleColor.DarkGray, ConsoleColor.Black);
                    CenteredText(x, y + 8, cardWidth, $"        ", ConsoleColor.Black, ConsoleColor.Black);
                }
                else
                {
                    CenteredText(x, y - 2, cardWidth, $"Lv.{m.Rank} [{m.Name}]", ConsoleColor.White, ConsoleColor.Black);
                    Fill(x + 8, y, 6, 3, ConsoleColor.Cyan);
                    CenteredText(x, y + 1, cardWidth, "^  ^", ConsoleColor.Black, ConsoleColor.Cyan);
                    CenteredText(x, y + 6, cardWidth, $"  {m.CurrentHP} / {m.MaxHP}  ", ConsoleColor.White, ConsoleColor.Black);
                    if (Battle.IsTargetPhase == true)
                    {
                        CenteredText(x, y + 8, cardWidth, $"< {i+1} >", ConsoleColor.Yellow, ConsoleColor.Black);
                    }
                    else
                    {
                        CenteredText(x, y + 8, cardWidth, $"       ", ConsoleColor.Black, ConsoleColor.Black);
                    }
                }

                // HP 바
                int barWidth = 16;
                int filled = (int)((m.CurrentHP / (float)m.MaxHP) * barWidth);
                Bar(x + 3, y + 5, filled, barWidth, ConsoleColor.Red);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////// 주사위 UI
        public static void DrawDie(int value, int x, int y, ConsoleColor dicecolor, ConsoleColor dotcolor)
        {
            string[] lines = value switch //                                                                    << 여기 윈도우 버전 조심
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
            if (values == null || values.Count == 0)
            {
                Console.WriteLine(" 엥? 전투관련 버그발생");
                return;
            }
            int startX = 3;
            int y = 23;
            for (int i = 0; i < values.Count; i++)
            {
                DrawDie(values[i], startX + i * 8, y, ConsoleColor.DarkGreen, ConsoleColor.White);
                if (Battle.IsRerollPhase == true)
                {
                    To(4 + i * 8, 22, ConsoleColor.Yellow, $"{i + 1}번?");
                }
                else
                {
                    To(4 + i * 8, 22, ConsoleColor.Yellow, $"    ");
                }
            }
        }
        public static void DrawDD(List<int> values)
        {
            if (values == null || values.Count == 0)
            {
                Console.WriteLine(" 엥? 전투관련 버그발생");
                return;
            }
            int dieWidth = 8;
            int centerX = 49;
            int y = 23;

            int totalWidth = values.Count * dieWidth;
            int startX = centerX - totalWidth / 2;

            for (int i = 0; i < values.Count; i++)
            {
                int x = startX + i * dieWidth;
                DrawDie(values[i], x, y, ConsoleColor.Yellow, ConsoleColor.Black);
                if (Battle.IsRerollPhase == true)
                {
                    To(x + 1, 22, ConsoleColor.Yellow, $"{i + 3}번?");
                }
                else
                {
                    To(x + 1, 22, ConsoleColor.Yellow, $"    ");
                }
            }
            Console.SetCursorPosition(114, 28);
        }
        public static void DrawDDTotal(int value, int x, int y)
        {
            string[] lines = value switch
            {
                0 => new[] {   " ██ ",
                               "█  █",
                               "█  █",
                               "█  █",
                               " ██ " },

                1 => new[] { "  █ ",
                             " ██ ",
                             "  █ ",
                             "  █ ",
                             " ███", },

                2 => new[] { "███ ",
                             "   █",
                             " ██ ",
                             "█   ",
                             "████" },

                3 => new[] { "███ ",
                             "   █",
                             " ██ ",
                             "   █",
                             "███ " },

                4 => new[] { "█  █",
                             "█  █",
                             "████",
                             "   █",
                             "   █" },

                5 => new[] { "████",
                             "█   ",
                             "███ ",
                             "   █",
                             "███ " },

                6 => new[] { " ██ ",
                             "█   ",
                             "███ ",
                             "█  █",
                             " ██ " },

                7 => new[] { "████",
                             "   █",
                             "  █ ",
                             " █  ",
                             " █  " },

                8 => new[] { " ██ ",
                             "█  █",
                             " ██ ",
                             "█  █",
                             " ██ " },

                9 => new[] { " ██ ",
                             "█  █",
                             " ███",
                             "   █",
                             " ██ " }
            };

            Console.ForegroundColor = ConsoleColor.Cyan;

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(x, y + i);
                string line = lines[i];
                foreach (char c in line)
                {
                    Console.Write(c);
                }
            }
            Console.ResetColor();
        }

        public static void UpdateDDTotal(List<int> values)
        {
            int DDTotal = values.Sum();
            int DDTotalten = DDTotal / 10; // 10의 자리 숫자
            int DDTotalone = DDTotal % 10; // 1의 자리 숫자
            DrawDDTotal(DDTotalten, 78, 23);
            DrawDDTotal(DDTotalone, 83, 23);
            Console.SetCursorPosition(114, 28);
        }

        public static void DrawCommandOptions(string firstOption, string secondOption, string thirdOption)
        {
            DrawCommandOptionsClear();
            int x = 92;
            int y = 23;
            To(x, y, ConsoleColor.White, $"(1) {firstOption}");
            To(x, y + 2, ConsoleColor.White, $"(2) {secondOption}");
            To(x, y + 4, ConsoleColor.White, $"(3) {thirdOption}");
            To(x + 15, y + 5, ConsoleColor.Gray, "선택 :      ");
            Console.SetCursorPosition(114, 28); //////////////////////           커서 위치 초기화 <<<<<<<
        }

        public static void DrawCommandOptions(string firstOption)
        {
            DrawCommandOptionsClear();
            int x = 92;
            int y = 23;
            To(x, y + 2, ConsoleColor.White, $"{firstOption}");
            To(x + 15, y + 5, ConsoleColor.Gray, "선택 :      ");
            Console.SetCursorPosition(114, 28); //////////////////////           커서 위치 초기화 <<<<<<<
        }
        public static void DrawCommandOptions(string firstOption, string secondOption)
        {
            DrawCommandOptionsClear();
            int x = 92;
            int y = 23;
            To(x, y + 2, ConsoleColor.White, $"{firstOption}");
            To(x, y + 3, ConsoleColor.White, $"{firstOption}");
            To(x + 15, y + 5, ConsoleColor.Gray, "선택 :      ");
            Console.SetCursorPosition(114, 28); //////////////////////           커서 위치 초기화 <<<<<<<
        }
        public static void DrawCommandOptionsClear()
        {
            int x = 92;
            int y = 23;
            for (y = 23; y < 28; y++)
            { To(x, y, ConsoleColor.White, "                         "); }
            To(x + 15, 28, ConsoleColor.Gray, "선택 :      ");
            Console.SetCursorPosition(114, 28); //////////////////////           커서 위치 초기화 <<<<<<<
        }

        public static void DrawYOUDIED()
        {
            EventManager.Clear();
            int x = 27;
            int y = 10;
            ConsoleColor c = ConsoleColor.DarkRed;
            To(x,   y,   c, " ██╗   ██╗  █████╗  ██╗  ██╗     █████╗   ██████╗ ██████╗ █████╗  "); Thread.Sleep(100);
            To(x, y + 1, c, "  ██╗ ██╔╝ ██╔══██║ ██║  ██║     ██╔══██╗ ╚═██╔═╝ ██╔═══╝ ██╔══██╗"); Thread.Sleep(100);
            To(x, y + 2, c, "   ████╔╝  ██║  ██║ ██║  ██║     ██║  ██║   ██║   ██████╗ ██║  ██║"); Thread.Sleep(100);
            To(x, y + 3, c, "    ██╔╝   ██║  ██║ ██║  ██║     ██║  ██║   ██║   ██╔═══╝ ██║  ██║"); Thread.Sleep(100);
            To(x, y + 4, c, "    ██║     █████╔╝  █████╔╝     █████╔═╝ ██████╗ ██████╗ █████╔═╝"); Thread.Sleep(100);
            To(x, y + 5, c, "    ╚═╝     ╚════╝   ╚════╝      ╚════╝   ╚═════╝ ╚═════╝ ╚════╝  ");
            Console.ResetColor();
            To(51,25, ConsoleColor.DarkGray, "- Press Any Key -");
            Console.ReadKey();
        }


    }
}