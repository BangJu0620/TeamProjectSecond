using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    // 다른 클래스에서 사용하시려면
    // Console.WriteLine(Monster.Instance[0].Name); > 출력: 미니언
    // 저런 식으로 접근하면 됩니다.
    // 몬스터를 수정, 추가하고 싶으시면 static Monster(){} 안에 있는 요소들을 수정, 추가하시면 됩니다.
    public class Monster
    {
        private static List<MonsterData> instance;

        public static List<MonsterData> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new List<MonsterData>();
                }
                return instance;
            }
        }

        static Monster()
        {
            instance = new List<MonsterData>();
            instance.Add(new MonsterData(2, "미니언", 15, 5));
            instance.Add(new MonsterData(3, "공허충", 10, 9));
            instance.Add(new MonsterData(5, "대포미니언", 25, 8));
        }
    }

    public class MonsterData
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int HealthPoint { get; set; }
        public int AttackPoint { get; set; }

        public MonsterData(int mLevel, string mName, int mHP, int mAP)
        {
            Level = mLevel;
            Name = mName;
            HealthPoint = mHP;
            AttackPoint = mAP;
        }
    }
}
