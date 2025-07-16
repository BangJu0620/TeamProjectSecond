using System;

namespace TeamProjectSecond
{
    public class Skill
    {
        public string Name { get; private set; }           // 스킬명
        public string Description { get; private set; }    // 스킬툴팁?
        public int ManaCost { get; private set; }          // 마나 소모량
        public int RequiredLevel { get; private set; }     // 습득 레벨
        public bool IsActive { get; private set; }         // Active / Passive 여부

        // 스킬 효과를 실제로 실행하는 델리게이트
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

        
        public bool TryUse(Character character) // 스킬을 사용 메서드 (임시로 만들었씀다 전투쪽 코드구조 잡으면 바꿀듯)
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
    }
}
