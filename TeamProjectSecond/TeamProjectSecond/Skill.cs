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
            return new Skill(
                data.Name,
                data.Description,
                data.ManaCost,
                data.RequiredLevel,
                data.IsActive,
                character =>
                {
                    // Active Skill 처리
                    if (data.IsActive)
                    {
                        switch (data.Name)
                        {
                            case "FixAllDD=5":
                  //              character.TempDDOverride = 5;
                                break;

                            case "MinDice2":
                   //             character.DDTransform = d => Math.Max(2, d);
                                break;

                            case "MinDice3":
                       //         character.DDTransform = d => Math.Max(3, d);
                                break;

                            case "Dmg+Lv/10":
                     //           character.TempDamageBonus += character.Level / 10f;
                                break;

                            case "AOE":
                     //           character.MageMPRecoveryInsteadOfAttack = true;
                                break;

                            case "Exclude1/3+AOE":
                        //        character.DDTransform = d => (d == 1 || d == 3) ? 0 : d;
                        //        character.MageMPRecoveryInsteadOfAttack = true;
                                break;

                            case "Def+4DD / MP+8DD":
                                character.BonusDefense += 4;
                                character.ManaPoint += 8 * Battle.ddValues.Sum();
                                break;

                            case "CubeDmg":
                                character.TempDamageBonus += (float)Math.Pow(Battle.ddValues.Sum() - 1, 3);
                                break;
                        }
                    }
                    // Passive Skill 처리
                    else
                    {
                        switch (data.Name)
                        {
                            case "BonusDmgPerLevel":
                                character.BaseDamageBonus += character.Level;
                                break;

                            case "FlatDmg+0.5":
                                character.BaseDamageMultiplier += 0.5f;
                                break;

                            case "FlatDmg+1 / SD1=5":
                                character.BaseDamageMultiplier += 1.0f;
                         //       character.TempSDOverride = 5;
                                break;

                            case "NoCrit / MPShield":
                           //     character.CritThreshold = 99;
                                // MP로 데미지 흡수 → 별도 전투 로직에서 분기 필요
                                break;

                            case "EvenDD+1.0":
                        //        character.DDTransform = d => (d % 2 == 0) ? d + 1 : d;
                                break;

                            case "Crit+0.5 / Crit9+":
                                character.BonusCritMultiplier += 0.5f;
                                character.BonusCritThreshold += 2;
                                break;

                            case "6+Bonus+1f":
                //                character.DDTransform = d => (d >= 6) ? d + 1 : d;
                                break;

                            case "MaxDice=7":
                   //             character.DDTransform = d => Math.Min(7, d);
                      //          character.SDTransform = d => Math.Min(7, d);
                                break;

                            case "Crit2.0x":
                                character.BonusCritMultiplier += 0.4f;
                                break;

                            case "Crit8+":
                                character.BonusCritThreshold += 3;
                                break;
                        }
                    }
                });
        }
    }
}