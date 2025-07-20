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
        public Action<Character>? OnBeforeRoll { get; set; }
        public Action<Character>? OnAfterRoll { get; set; }
        public Action<Character>? OnRoundEnd { get; set; }
        public Action<Character>? OnMonsterAttack { get; set; }

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
                        if (data.IsActive)
                        {
                            switch (data.Name)
                            {
                                case "확신의 5":
                                    // 하드코딩 롤 전에 다이스.FixedEyes = 5; 
                                    break;
                                case "최소 2":
                                    character.TempMinDice += 1;
                                    break;
                                case "최소 3":
                                    character.TempMinDice += 2;
                                    break;
                                case "받고, 더":
                                    character.TempDamageMultiplier += (character.Level / 10.0f);
                                    break;
                                case "패 돌리기":
                                    // 하드코딩
                                    break;
                                case "로디드 다이스":
                                    // 하드코딩 롤 전에 다이스.FixedEyes = 5; 
                                    break;
                                case "나이스 폴드":
                                    character.BonusDefense += 4;
                                    character.ManaPoint += 8 * Battle.ddValues.Sum();
                                    break;
                                case "Ready to 🎲":
                                    character.TempDamageBonus += (float)Math.Pow(Battle.ddValues.Sum() - 1, 3);
                                    break;
                            }
                        }
                        else
                        {
                            switch (data.Name)
                            {
                                case "야금야금":
                                    character.TempDamageBonus += character.Level;
                                    break;
                                case "능숙한 베팅":
                                    character.BaseDamageMultiplier += 0.5f;
                                    break;
                                case "평정심":
                                    character.BaseDamageMultiplier += 1.0f;
                                    break;
                                case "안좋은 습관":
                                    break;
                                case "메이드":
                                    break;
                                case "스몰 블라인드":
                                    character.BonusCritMultiplier += 0.5f;
                                    character.BonusCritThreshold += 2;
                                    break;
                                case "아웃사이드 베팅":
                                    break;
                                case "럭키 세븐":
                                    break;
                                case "빅 블라인드":
                                    character.BonusCritMultiplier += 0.4f;
                                    break;
                                case "칩 리더":
                                    character.BonusCritThreshold += 3;
                                    break;
                            }
                        }
                    }       );
        }
    }
}