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
        private static Character instance;

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
            Gold = 1500;
            ManaPoint = MaxManaPoint;
            HealthPoint = MaxHealthPoint;
            Speed = ClassData.BaseSpeed+ BonusSpeed;
            Item.AddItem("HP 포션", 3, showMessage: false);//시작시 포션3개 지급
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public ClassType ClassType { get; set; }  // 변수들 몇개 좀 ClassData쪽으로 옮겨서 관리하겄슴다 
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int HealthPoint { get; set; }
        public int ManaPoint { get; set; }
        public int Speed { get; set; }
        public float BaseDamageMultiplier { get; set; } = 1f; // 영구적인 데미지 관여 값 (영약으로 오르는 것도 여기다 넣으면 될 듯용)
        public float BaseDamageBonus { get; set; } = 0f;
        public float TempDamageMultiplier { get; set; } = 1f;  // 일시적인 데미지 관여 값 (스킬같은 걸로 전투 중에만 적용)
        public float TempDamageBonus { get; set; } = 0f;
        public float TotalDamageMultiplier => BaseDamageMultiplier * TempDamageMultiplier;  // 최종 적용값 (계산용 프로퍼티)
        public float TotalDamageBonus => BaseDamageBonus + TempDamageBonus;

        // 이 밑으로 영약 추가했습니다, 영약 관련 주석들 확인 후 필요없어지면 삭제하셔도 무방합니다 /// 확인했습니다요
        public int BonusMaxHP { get; set; } = 0;
        public int BonusMaxMP { get; set; } = 0;
        public int BonusDefense { get; set; } = 0;
        public int BonusSpeed { get; set; } = 0;

        public ClassData ClassData => new ClassData(ClassType);

        public int MaxHealthPoint => ClassData.MaxHPByLevel(Level) + BonusMaxHP; //+ Bonus 붙은 부분이 영약 계산식입니다
        public int MaxManaPoint => ClassData.MaxMPByLevel(Level) + BonusMaxMP;
        public int DefensePoint => ClassData.DefenseByLevel(Level) + BonusDefense;
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
                ManaPoint = ManaPoint,
                //데미지계산식
                BaseDamageMultiplier = BaseDamageMultiplier,
                BaseDamageBonus = BaseDamageBonus,
                TempDamageMultiplier = TempDamageMultiplier,
                TempDamageBonus = TempDamageBonus,
                //영약
                BonusMaxHP = BonusMaxHP,
                BonusMaxMP = BonusMaxMP,
                BonusDefense = BonusDefense,
                BonusSpeed = BonusSpeed,
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
            //데미지계산식로드
            BaseDamageMultiplier = data.BaseDamageMultiplier;
            BaseDamageBonus = data.BaseDamageBonus;
            TempDamageMultiplier = data.TempDamageMultiplier;
            TempDamageBonus = data.TempDamageBonus;
            //영약
            BonusMaxHP = data.BonusMaxHP;
            BonusMaxMP = data.BonusMaxMP;
            BonusDefense = data.BonusDefense;
            BonusSpeed = data.BonusSpeed;
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
        public string? Name { get; set; }
        public int Level { get; set; }
        public ClassType ClassType { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int HealthPoint { get; set; }
        public int ManaPoint { get; set; }
        public float BaseDamageMultiplier { get; set; } = 1f; // 영구적인 데미지 관여 값 (영약으로 오르는 것도 여기다 넣으면 될 듯용)
        public float BaseDamageBonus { get; set; } = 0f;
        public float TempDamageMultiplier { get; set; } = 1f;  // 일시적인 데미지 관여 값 (스킬같은 걸로 전투 중에만 적용)
        public float TempDamageBonus { get; set; } = 0f;
        public float TotalDamageMultiplier => BaseDamageMultiplier * TempDamageMultiplier;
        public float TotalDamageBonus => BaseDamageBonus + TempDamageBonus;
        //영약
        public int BonusMaxHP { get; set; }
        public int BonusMaxMP { get; set; }
        public int BonusDefense { get; set; }
        public int BonusSpeed { get; set; }
    }
}
