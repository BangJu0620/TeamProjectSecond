using System;

namespace TeamProjectSecond
{
    public enum SkillType
    {
        Passive,
        Active
    }

    public class Skill
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ManaCost { get; private set; }
        public int RequiredLevel { get; private set; }
        public bool IsActive { get; private set; }

        // 스킬 효과를 실행하는 델리게이트
        public Action<Character> ApplyEffect { get; private set; }
        public Action<Character>? OnRoundStart { get; set; }
        public Action<Character>? OnBeforePrepareRoll { get; set; }
        public Action<Character>? OnBeforeRoll { get; set; }
        public Action<Character>? OnAfterRoll { get; set; }
        public Action<Character>? OnMonsterAttack { get; set; }
        public Action<Character>? OnRoundEnd { get; set; }

        public Skill(string name, string description, int manaCost, int requiredLevel, bool isActive, Action<Character> applyEffect)
        {
            Name = name;
            Description = description;
            ManaCost = manaCost;
            RequiredLevel = requiredLevel;
            IsActive = isActive;
            ApplyEffect = applyEffect;
        }

        public bool TryUse(Character character)
        {
            if (!IsActive) return false;
            if (character.Level < RequiredLevel) return false;
            if (character.ManaPoint < ManaCost) return false;

            character.ManaPoint -= ManaCost;
            ApplyEffect?.Invoke(character);
            return true;
        }

        public void ApplyPassive(Character character)
        {
            if (!IsActive && character.Level >= RequiredLevel)
                ApplyEffect?.Invoke(character);
        }

        // ✅ SkillData → Skill 변환

        public static Skill From(SkillData data)
        {
            var skill = new Skill(
                data.Name,
                data.Description,
                data.ManaCost,
                data.RequiredLevel,
                data.IsActive,
                character => { }  // 더 이상 여기에서 직접 적용 안 함
            );

            if (data.IsActive)
            {
                switch (data.Name)
                {
                    case "확신의 5":
                        skill.OnBeforeRoll = c =>
                        {
                            foreach (var d in Battle.ddList)
                                d.FixedEyes = 5;
                        };
                        break;

                    case "최소 2":
                        skill.OnBeforePrepareRoll = c => c.TempMinDice += 1;
                        break;

                    case "최소 3":
                        skill.OnBeforePrepareRoll = c => c.TempMinDice += 2;
                        break;

                    case "받고, 더":
                        skill.OnAfterRoll = c => c.TempDamageMultiplier += (c.Level / 10.0f);
                        break;

                    case "로디드 다이스":
                        skill.OnBeforeRoll = c =>
                        {
                            foreach (var d in Battle.ddList)
                            {
                                d.Excluded.Add(1);
                                d.Excluded.Add(3);
                            }
                        };
                        break;

                    case "나이스 폴드":
                        skill.OnAfterRoll = c => c.BonusDefense += 4 * Battle.ddValues.Sum();
                        skill.OnRoundEnd = c => c.ManaPoint += 8 * Battle.ddValues.Sum();
                        break;

                    case "Ready to 🎲":
                        skill.OnBeforePrepareRoll = c => c.TempDiceCountOverride = 1;
                        skill.OnAfterRoll = c => c.TempDamageBonus += (float)Math.Pow(Battle.ddValues.Sum() - 1, 3);
                        break;

                }
            }
            else
            {
                switch (data.Name)
                {
                    case "야금야금":
                        skill.OnRoundStart = c => c.TempDamageBonus += c.Level;
                        break;

                    case "능숙한 베팅":
                        skill.OnRoundStart = c => c.BaseDamageMultiplier += 0.5f;
                        break;

                    case "평정심":
                        skill.OnRoundStart = c => c.BaseDamageMultiplier += 1.0f;
                        skill.OnBeforeRoll = c =>
                        {
                            var sd1 = Battle.sdList.FirstOrDefault(d => d.Type == DiceType.SD && d.Index == 1);
                            if (sd1 != null)
                                sd1.FixedEyes = 5;
                        };
                        break;

                    case "스몰 블라인드":
                        skill.OnRoundStart = c =>
                        {
                            c.TempDamageMultiplier += 0.5f;
                            c.BonusCritThreshold += 2;
                        };
                        break;

                    case "빅 블라인드":
                        skill.OnRoundStart = c => c.BonusCritMultiplier += 0.4f;
                        break;

                    case "칩 리더":
                        skill.OnRoundStart = c => c.BonusCritThreshold += 3;
                        break;
                    case "안좋은 습관":
                        skill.OnBeforeRoll = c =>
                        {
                            c.BonusCritThreshold -= 12;
                        };
                        skill.OnMonsterAttack = c =>
                        {
                            if (Battle.IncomingMonsterDamage > 0)
                            {
                                int reduced = (int)(Battle.IncomingMonsterDamage * 0.8f);
                                int mpToAbsorb = Math.Min(reduced, c.ManaPoint);
                                c.ManaPoint -= mpToAbsorb;
                                Battle.IncomingMonsterDamage -= mpToAbsorb;
                            }
                        };
                        break;
                    case "메이드":
                        skill.OnAfterRoll = c =>
                        {
                            int evenCount = Battle.ddValues.Count(v => v % 2 == 0);
                            c.TempDamageMultiplier += evenCount * 1.0f;
                        };
                        break;
                    case "아웃사이드 베팅":
                        skill.OnAfterRoll = c =>
                        {
                            int highCount = Battle.ddValues.Count(v => v >= 6);
                            c.TempDamageMultiplier += highCount * 1.0f;
                        };
                        break;
                    case "럭키 세븐":
                        skill.OnBeforePrepareRoll = c =>
                        {
                            c.TempMaxDice = 1;
                        };
                        break;
                }
            }

            return skill;
        }

        public static void ShowSkills()
        {
            while(true)
            {
                var character = Character.Instance;
                var classData = character.ClassData;
                EventManager.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;
                EventManager.To(45, $"───── {classData.Type} 클래스 스킬 목록 ─────\n\n\n");

                Console.ForegroundColor = ConsoleColor.White;
                EventManager.To(28, "📌 [액티브 스킬]\n\n");
                foreach (var skill in classData.ActiveSkills)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;   // 스킬 이름 표시
                    EventManager.To(28, $"- ");
                    Console.Write($"{skill.Name}");                      

                    Console.ForegroundColor = ConsoleColor.Gray;        // 스킬 코스트
                    Console.SetCursorPosition(48, Console.CursorTop);
                    Console.Write($"| MP {skill.ManaCost}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(61, Console.CursorTop);    // 스킬 설명
                    Console.Write($"| {skill.Description}\n\n");
                }
                Console.WriteLine();

                EventManager.To(28, "📌 [패시브 스킬]\n\n");
                foreach (var skill in classData.PassiveSkills)

                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;   // 스킬 번호 표현
                    EventManager.To(28, $"- ");
                    Console.Write($"{skill.Name}");                      // 스킬 이름 표시

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(48, Console.CursorTop);    // 스킬 요구 레벨
                    Console.Write($"| 요구LV {skill.RequiredLevel}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(61, Console.CursorTop);    // 스킬 설명
                    Console.Write($"| {skill.Description}\n\n");
                }
                    Console.SetCursorPosition(0, 24);
                    EventManager.ToS(54, "Enter. 돌아가기\n");
                    EventManager.Select();

                switch (EventManager.CheckInput())
                {
                    case null:
                        return;
                    default:
                        EventManager.Wrong();
                        break;
                    }
            }
        }
    }
}