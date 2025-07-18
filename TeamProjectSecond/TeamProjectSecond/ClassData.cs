using System;

namespace TeamProjectSecond
{
    public enum ClassType
    {
        Warrior,
        Mage,
        Rogue
    }

    public class ClassData
    {
        public ClassType Type { get; private set; }

        public Func<int, int> MaxHPByLevel;
        public Func<int, int> MaxMPByLevel;
        public Func<int, int> DefenseByLevel;
        public int BaseSpeed { get; private set; }

        public Dictionary<int, int> DiceCountByLevel = new();
        public Dictionary<int, int> RerollCountByLevel = new();

        public List<SkillData> PassiveSkills = new(); // 다 넣어놓고 렙업하면 그때마다 언락하는 방식
        public List<SkillData> ActiveSkills = new();  // 액티브는 ㄹㅇ 추가되는 방식

        public ClassData(ClassType type)
        {
            Type = type;

            switch (type)
            {
                case ClassType.Warrior:
                    MaxHPByLevel = lv => 40 + 10 * lv;
                    MaxMPByLevel = lv => 50;
                    DefenseByLevel = lv => 2 * lv + 3;
                    DiceCountByLevel = new() { { 1, 1 }, { 6, 2 }, { 11, 3 }, { 16, 4 }, { 20, 5 } };
                    RerollCountByLevel = new() { { 1, 1 }, { 11, 2 } };
                    BaseSpeed = 15;

                    PassiveSkills.Add(new SkillData("BonusDmgPerLevel", "레벨당 뻥딜 보너스", 0, 1, false));
                    PassiveSkills.Add(new SkillData("FlatDmg+0.5", "뻥딜 +0.5f", 0, 8, false));
                    PassiveSkills.Add(new SkillData("FlatDmg+1 / SD1=5", "뻥딜+1f / SD_1이 5로 고정", 0, 16, false));

                    ActiveSkills.Add(new SkillData("MinDice2", "[A] MinDiceThreshold = 2", 10, 4, true));
                    ActiveSkills.Add(new SkillData("MinDice3", "[A] MinDiceThreshold = 3", 20, 12, true));
                    ActiveSkills.Add(new SkillData("FixAllDD=5", "[A] 모든 DD를 5로 고정", 30, 20, true));
                    break;

                case ClassType.Mage:
                    MaxHPByLevel = lv => 38 + 2 * lv;
                    MaxMPByLevel = lv => 190 + 10 * lv;
                    DefenseByLevel = lv => lv / 2;
                    DiceCountByLevel = new() { { 1, 2 }, { 6, 3 }, { 11, 4 }, { 16, 5 }, { 20, 6 } };
                    RerollCountByLevel = new() { { 1, 1 }, { 6, 2 }, { 11, 3 }, { 16, 4 } };
                    BaseSpeed = 10;

                    PassiveSkills.Add(new SkillData("NoCrit / MPShield", "크리 없음 / 피해의 80%를 MP로 감당", 0, 1, false));
                    PassiveSkills.Add(new SkillData("EvenDD+1.0", "짝수 DD마다 뻥딜 +1", 0, 16, false));

                    ActiveSkills.Add(new SkillData("AOE", "[A] 전체 공격", 40, 4, true));
                    ActiveSkills.Add(new SkillData("Dmg+Lv/10", "[A] 뻥딜 +(Lv/10)", 60, 8, true));
                    ActiveSkills.Add(new SkillData("Def+4DD / MP+8DD", "[A] 방어력+ / 마나 회복", 10, 12, true));
                    ActiveSkills.Add(new SkillData("Exclude1/3+AOE", "[A] 1,3 제외 전체공격", 200, 20, true));
                    break;

                case ClassType.Rogue:
                    MaxHPByLevel = lv => 40 + 5 * lv;
                    MaxMPByLevel = lv => 50;
                    DefenseByLevel = lv => lv + 4;
                    DiceCountByLevel = new() { { 1, 3 }, { 6, 4 }, { 11, 5 }, { 16, 6 }, { 20, 7 } };
                    RerollCountByLevel = new() { { 1, 1 } };
                    BaseSpeed = 20;

                    PassiveSkills.Add(new SkillData("Crit+0.5 / Crit9+", "기본뻥딜+0.5 / 크리 9+", 0, 1, false));
                    PassiveSkills.Add(new SkillData("6+Bonus+1f", "6이상 눈마다 +1f", 0, 4, false));
                    PassiveSkills.Add(new SkillData("MaxDice=7", "DD/SD의 Max를 7로", 0, 8, false));
                    PassiveSkills.Add(new SkillData("Crit2.0x", "크리 데미지 2.0배", 0, 12, false));
                    PassiveSkills.Add(new SkillData("Crit8+", "크리 임계값 8+", 0, 20, false));

                    ActiveSkills.Add(new SkillData("CubeDmg", "[A] DD갯수 1 / 데미지=(D-1)^3", 20, 16, true));
                    break;
            }
        }
    }

    public class SkillData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
        public int RequiredLevel { get; set; }
        public bool IsActive { get; set; }

        public SkillData(string name, string desc, int manaCost, int level, bool isActive)
        {
            Name = name;
            Description = desc;
            ManaCost = manaCost;
            RequiredLevel = level;
            IsActive = isActive;
        }
    }
}