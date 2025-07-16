using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    // 다른 클래스에서 사용하시려면
    // Console.WriteLine(Character.Instance.Name); > 출력: 이름없음
    // 저런 식으로 접근하면 됩니다.
    // 캐릭터를 수정하시려면 private Character(){} 안에 있는 요소들을 수정하시면 됩니다.
    public class Character
    {
        private static Character? instance;

        public static Character Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Character();
                }
                return instance;
            }
        }

        private Character()
        {
            Level = 1;
            Name = "이름없음";
            ClassType = ClassType.Warrior;
            Exp = 0;
            Gold = 0;
            ManaPoint = MaxManaPoint;
            HealthPoint = MaxHealthPoint;
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public ClassType ClassType { get; set; }  // 변수들 몇개 좀 ClassData쪽으로 옮겨서 관리하겄슴다 
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int HealthPoint { get; set; }
        public int ManaPoint { get; set; }

        public ClassData ClassData => new ClassData(ClassType);

        public int MaxHealthPoint => ClassData.MaxHPByLevel(Level);
        public int MaxManaPoint => ClassData.MaxMPByLevel(Level);
        public int DefensePoint => ClassData.DefenseByLevel(Level);
        public int DiceCount => ClassData.DiceCountByLevel
                                  .Where(kv => kv.Key <= Level)
                                  .Select(kv => kv.Value)
                                  .Last();
        public int RerollCount => ClassData.RerollCountByLevel
                                  .Where(kv => kv.Key <= Level)
                                  .Select(kv => kv.Value)
                                  .Last();

        public List<SkillData> ActiveSkills => ClassData.ActiveSkills
            .Where(skill => skill.RequiredLevel <= Level)
            .ToList();

        public List<SkillData> PassiveSkills => ClassData.PassiveSkills
            .Where(skill => skill.RequiredLevel <= Level)
            .ToList();


        public CharacterData ToData()
        {
            return new CharacterData
            {
                Name = Name,
                Level = Level,
                ClassType = ClassType,
                Exp = Exp,
                Gold = Gold,
                HealthPoint = HealthPoint,
                ManaPoint = ManaPoint
            };
        }

        public void LoadFromData(CharacterData data)
        {
            Name = data.Name;
            Level = data.Level;
            ClassType = data.ClassType;
            Exp = data.Exp;
            Gold = data.Gold;
            HealthPoint = data.HealthPoint;
            ManaPoint = data.ManaPoint;
        }
    }

    public class ClassTypeChange
    {
        // 전직 기능, 인트로에 해당 직업에 해당하는 메서드 설정하면 될겁니다. 아마
        // 직업별 스탯 바꾸시려면 아래에 있는 수치 바꾸시면 됩니다.
        public void PromoteToWarrior()
        {
            var character = Character.Instance;
            character.ClassType = ClassType.Warrior;
            character.Level = 1;
            character.Exp = 0;
            character.HealthPoint = character.MaxHealthPoint;
            character.ManaPoint = character.MaxManaPoint;
        }

        public void PromoteToRogue()
        {
            var character = Character.Instance;
            character.ClassType = ClassType.Rogue;
            character.Level = 1;
            character.Exp = 0;
            character.HealthPoint = character.MaxHealthPoint;
            character.ManaPoint = character.MaxManaPoint;
        }

        public void PromoteToMage()
        {
            var character = Character.Instance;
            character.ClassType = ClassType.Mage;
            character.Level = 1;
            character.Exp = 0;
            character.HealthPoint = character.MaxHealthPoint;
            character.ManaPoint = character.MaxManaPoint;
        }
    }

    public class CharacterData
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public ClassType ClassType { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int HealthPoint { get; set; }
        public int ManaPoint { get; set; }
    }
}
