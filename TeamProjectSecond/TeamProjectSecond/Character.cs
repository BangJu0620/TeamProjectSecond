using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Job = "초보자";
            AttackPoint = 1;
            DefensePoint = 1;
            HealthPoint = 50;
            Exp = 0;
            RequiredExp = 0;
            CritRate = 15;
            EvasionRate = 0.1f;
            ManaPoint = 50;
            Gold = 1500;
        }

        public int Level { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int HealthPoint { get; set; }
        public int MaxHealthPoint { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }
        public int RequiredExp { get; set; }
        public float CritRate { get; set; }
        public float EvasionRate { get; set; }
        public int ManaPoint { get; set; }

        public CharacterData ToData()
        {
            return new CharacterData
            {
                Level = Level,
                Name = Name,
                Job = Job,
                AttackPoint = AttackPoint,
                DefensePoint = DefensePoint,
                HealthPoint = HealthPoint,
                MaxHealthPoint = MaxHealthPoint,
                Gold = Gold,
                Exp = Exp,
                RequiredExp = RequiredExp,
                CritRate = CritRate,
                EvasionRate = EvasionRate,
                ManaPoint = ManaPoint
            };
        }

        public void LoadFromData(CharacterData data)
        {
            Level = data.Level;
            Name = data.Name;
            Job = data.Job;
            AttackPoint = data.AttackPoint;
            DefensePoint = data.DefensePoint;
            HealthPoint = data.HealthPoint;
            MaxHealthPoint = data.MaxHealthPoint;
            Gold = data.Gold;
            Exp = data.Exp;
            RequiredExp = data.RequiredExp;
            CritRate = data.CritRate;
            EvasionRate= data.EvasionRate;
            ManaPoint = data.ManaPoint;
        }
    }

    public class CharacterData
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int HealthPoint { get; set; }
        public int MaxHealthPoint { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }
        public int RequiredExp { get; set; }
        public float CritRate { get; set; }
        public float EvasionRate { get; set; }
        public int ManaPoint { get; set; }
    }
}
