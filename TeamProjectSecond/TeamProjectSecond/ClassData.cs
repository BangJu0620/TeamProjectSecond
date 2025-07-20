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
        public float BaseCritMultiplier { get; private set; }

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
                    BaseCritMultiplier = 1.6f;

                    PassiveSkills.Add(new SkillData("야금야금", "레벨당 뻥딜 보너스", 0, 1, false));
                    PassiveSkills.Add(new SkillData("능숙한 베팅", "뻥딜 +0.5f", 0, 8, false));
                    PassiveSkills.Add(new SkillData("평정심", "뻥딜+1f / SD_1이 5로 고정", 0, 16, false));

                    ActiveSkills.Add(new SkillData("최소 2", "[A] MinDiceThreshold = 2", 10, 4, true));
                    ActiveSkills.Add(new SkillData("최소 3", "[A] MinDiceThreshold = 3", 20, 12, true));
                    ActiveSkills.Add(new SkillData("확신의 5", "[A] 모든 DD를 5로 고정", 30, 20, true));
                    break;

                case ClassType.Mage:
                    MaxHPByLevel = lv => 38 + 2 * lv;
                    MaxMPByLevel = lv => 190 + 10 * lv;
                    DefenseByLevel = lv => lv / 2;
                    DiceCountByLevel = new() { { 1, 2 }, { 6, 3 }, { 11, 4 }, { 16, 5 }, { 20, 6 } };
                    RerollCountByLevel = new() { { 1, 1 }, { 6, 2 }, { 11, 3 }, { 16, 4 } };
                    BaseSpeed = 10;
                    BaseCritMultiplier = 1.6f;

                    PassiveSkills.Add(new SkillData("미신쟁이의 습관", "크리 없음 / 피해의 80%를 MP로 감당", 0, 1, false));
                    PassiveSkills.Add(new SkillData("메이드", "짝수 DD마다 뻥딜 +1", 0, 16, false));

                    ActiveSkills.Add(new SkillData("땡값", "[A] 전체 공격", 40, 4, true));
                    ActiveSkills.Add(new SkillData("받고, 더", "[A] 뻥딜 +(Lv/10)", 60, 8, true));
                    ActiveSkills.Add(new SkillData("나이스 폴드", "[A] 방어력+ / 마나 회복", 10, 12, true));
                    ActiveSkills.Add(new SkillData("로디드 다이스", "[A] 1,3 제외 전체공격", 200, 20, true));
                    break;

                case ClassType.Rogue:
                    MaxHPByLevel = lv => 40 + 5 * lv;
                    MaxMPByLevel = lv => 50;
                    DefenseByLevel = lv => lv + 4;
                    DiceCountByLevel = new() { { 1, 3 }, { 6, 4 }, { 11, 5 }, { 16, 6 }, { 20, 7 } };
                    RerollCountByLevel = new() { { 1, 1 } };
                    BaseSpeed = 20;
                    BaseCritMultiplier = 1.6f;

                    PassiveSkills.Add(new SkillData("스몰 블라인드", "SD가 9이상이 떠도 크리티컬/데미지계수 +0.5", 0, 1, false));
                    PassiveSkills.Add(new SkillData("아웃사이드 베팅", "6눈 이상의 주사위마다 데미지계수 +1", 0, 4, false));
                    PassiveSkills.Add(new SkillData("럭키 세븐", "7눈의 주사위", 0, 8, false));
                    PassiveSkills.Add(new SkillData("빅 블라인드", "크리티컬 데미지 40%p증가", 0, 12, false));
                    PassiveSkills.Add(new SkillData("칩 리더", "SD가 8이상이 떠도 크리티컬", 0, 20, false));

                    ActiveSkills.Add(new SkillData("Ready to 🎲", "단 한개의 DD. ", 20, 16, true));
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