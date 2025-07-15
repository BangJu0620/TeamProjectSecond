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
            CritRate = 0.15f;
            EvasionRate = 0.1f;
            ManaPoint = 50;
            MaxManaPoint = 50;
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
        public int MaxManaPoint { get; set; }
    }

    public class JobChange 
    {
        // 전직 기능, 인트로에 해당 직업에 해당하는 메서드 설정하면 될겁니다. 아마
        // 직업별 스탯 바꾸시려면 아래에 있는 수치 바꾸시면 됩니다.
        public void PromoteToWarrior()
        {
            Character.Instance.Job = "워리어";
            Character.Instance.AttackPoint = 10;
            Character.Instance.DefensePoint = 5;
            Character.Instance.HealthPoint = 100;
            Character.Instance.MaxHealthPoint = 100;
            Character.Instance.CritRate = 0.15f;
            Character.Instance.EvasionRate = 0.1f;
            Character.Instance.ManaPoint = 50;
            Character.Instance.MaxManaPoint = 50;
        }

        public void PromoteToRogue()
        {
            Character.Instance.Job = "로그";
            Character.Instance.AttackPoint = 12;
            Character.Instance.DefensePoint = 3;
            Character.Instance.HealthPoint = 70;
            Character.Instance.MaxHealthPoint = 70;
            Character.Instance.CritRate = 0.2f;
            Character.Instance.EvasionRate = 0.2f;
            Character.Instance.ManaPoint = 50;
            Character.Instance.MaxManaPoint = 50;
        }

        public void PromoteToMage()
        {
            Character.Instance.Job = "메이지";
            Character.Instance.AttackPoint = 14;
            Character.Instance.DefensePoint = 2;
            Character.Instance.HealthPoint = 50;
            Character.Instance.MaxHealthPoint = 50;
            Character.Instance.CritRate = 0.15f;
            Character.Instance.EvasionRate = 0.05f;
            Character.Instance.ManaPoint = 100;
            Character.Instance.MaxManaPoint = 100;
        }
    }
}
