using System;
using System.Collections.Generic;

public class Monster
{
    public int Rank { get; private set; }
    public int Index { get; private set; }
    public string Name { get; private set; }
    public int BaseAttack { get; private set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; set; }
    public string Cry { get; private set; }
    public string CryToT { get; private set; }

    public Monster(int rank, int index, string name, int baseAttack, int defense, int speed, int hp, string cry, string cryToT)
    {
        Rank = rank;
        Index = index;
        Name = name;
        BaseAttack = baseAttack;
        Defense = defense;
        Speed = speed;
        MaxHP = hp;
        CurrentHP = hp;
        Cry = cry;
        CryToT = cryToT;
    }

    public static Dictionary<int, List<Monster>> MonsterDictionary = new Dictionary<int, List<Monster>>()
    {
        {
            1, new List<Monster>()
            {
                new Monster(1, 0, "주사위 개구리", 0, 0, 8, 8, "\" 개골 \"", "\" 개골... \""),
                new Monster(1, 1, "주사위 쥐", 0, 1, 10, 10, "\" 찍, 찍 \"", "\" 찍... \""),
                new Monster(1, 2, "주사위 슬라임", 0, 1, 5, 5, "\" 꿈~틀 \"", "\" ... \"")         //   "\"  \""
            }
        },
        {
            2, new List<Monster>()
            {
                new Monster(2, 0, "주사위 여우", 3, 1, 10, 18, "\" 캥, 캥 \"", "\" 캐앵!... \""),
                new Monster(2, 1, "주사위 야옹이", 3, 1, 10, 20, "\" 안녕하세요? \"", "\" 안녕히계세요? \""),
                new Monster(2, 2, "주사위 뱀", 2, 0, 20, 13, "\" 샤샤샥.. \"", "\" 샤악... \""),
                new Monster(2, 3, "주사위 거북이", 0, 6, 5, 15, " '단단해 보이는군...' ", "\" 흐에에.. \"")
            }
        },
        {
            3, new List<Monster>()
            {
                new Monster(3, 0, "주사위 코볼트", 8, 4, 10, 26, "\" 왈 왈! \"", "\" 깨갱, 깽 \""),
                new Monster(3, 1, "주사위 원숭이", 8, 5, 10, 33, "\" 우호, 우호 \"", "\" 우끼... \""),
                new Monster(3, 2, "주사위 독수리", 3, 1, 20, 20, "\" 끼~ 악 ! \"", "\" 看鐵蹄錚錚 \"")
            }

        },
        {
            4, new List<Monster>()
            {
                new Monster(4, 0, "주사위 곰", 12, 6, 8, 42, "\" 우루룽~ \"", "\" 고옴... \""),
                new Monster(4, 1, "주사위 멧돼지", 11, 5, 12, 39, "\" 꾸에엑!! \"", "\" 꾸옥... \""),
                new Monster(4, 2, "주사위 매니저님", 10, 4, 14, 32, "\" 아,아,잘들리시나요? \"", "\" 크아아아악! \"")
            }
        },
        {
            5, new List<Monster>()
            {
                new Monster(5, 0, "주사위 늑대맨", 16, 8, 16, 60, "\" 아우우우! \"", "\" 아이구 아파라... \""),
                new Monster(5, 1, "주사위 박쥐떼", 14, 4, 22, 45, "\" 푸드드득! \"", "\" 푸우우... \""),
                new Monster(5, 2, "주사위 사자", 18, 7, 14, 55, "\" 크르릉!! \"", "\" 으르릉... \""),
            }
        }
    };
}