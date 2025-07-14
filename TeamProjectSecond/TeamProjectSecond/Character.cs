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
            Job = "초보자";
            AttackPoint = 1;
            DefensePoint = 1;
            HealthPoint = 50;
            Gold = 1500;
        }

        public int Level { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int HealthPoint { get; set; }
        public int Gold { get; set; }
    }
}
