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
    public bool IsDead { get; set; } = false;

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
                new Monster(3, 2, "주사위 독수리", 3, 1, 20, 20, "\" 끼~ 악 ! \"", "\" 看鐵蹄錚錚 \""),
                new Monster(3, 3, "주사위 괴인", 7, 7, 7, 7, "\" 으흐히히주사위좋다흐흐 \"", "\" 으흐흐주사위... \"")
            }

        },
        {
            4, new List<Monster>()
            {
                new Monster(4, 0, "주사위 곰", 12, 6, 8, 42, "\" 우루룽~ \"", "\" 고옴... \""),
                new Monster(4, 1, "주사위 경찰", 11, 5, 12, 39, "\" 녀석을 주사위로 구속해라! \"", "\" 윽, 지원을 요청... \""),
                new Monster(4, 2, "주사위 매니저님", 10, 4, 14, 32, "\" 아,아,잘들리시나요? \"", "\" 크아아아악! \"")
            }
        },
        {
            5, new List<Monster>()
            {
                new Monster(5, 0, "주사위 늑대맨", 16, 8, 16, 60, "\" 아우우우! \"", "\" 아이구 아파라... \""),
                new Monster(5, 1, "주사위 박쥐떼", 14, 4, 22, 45, "\" 푸드드득! \"", "\" 푸우우... \""),
                new Monster(5, 2, "주사위 사자", 18, 7, 14, 55, "\" 크르릉!! \"", "\" 으르릉... \"")
            }
        },
        {
            7, new List<Monster>()
            {
                new Monster(7, 0, "약한보스몬스터", 15, 12, 15, 180, "\" 나는보스몹이당 \"", "\" 치욕스럽다~ \"")
            }
        },
        {
            10, new List<Monster>()
            {
                new Monster(10, 0, "진짜보스몬스터", 20, 15, 15, 300, "\" 내가최종보스당 \"", "\" 으허엉 \"")
            }
        }
    };
    public static List<Monster> Gen(int a, int b, int maxRank)
    {
        Random rand = new Random();

        // 가능한 랭크 조합 생성 (1~maxRank 사이의 수 1~3개로 구성, 합이 a 이상 b 이하)
        List<List<int>> validRankCombinations = new List<List<int>>();

        void Search(List<int> current, int sum)
        {
            if (current.Count > 3 || sum > b) return;
            if (current.Count > 0 && sum >= a && sum <= b)
            {
                validRankCombinations.Add(new List<int>(current));
            }

            for (int r = 1; r <= maxRank; r++)
            {
                current.Add(r);
                Search(current, sum + r);
                current.RemoveAt(current.Count - 1);
            }
        }

        Search(new List<int>(), 0);

        if (validRankCombinations.Count == 0)
            return new List<Monster>();  // 조건 만족하는 조합이 없음

        // 랜덤 조합 선택
        List<int> chosenRanks = validRankCombinations[rand.Next(validRankCombinations.Count)];

        List<Monster> result = new List<Monster>();

        foreach (int rank in chosenRanks)
        {
            if (!MonsterDictionary.ContainsKey(rank) || MonsterDictionary[rank].Count == 0)
                continue;

            // 해당 랭크에서 랜덤 몬스터 선택 후 복제해서 넣기
            Monster template = MonsterDictionary[rank][rand.Next(MonsterDictionary[rank].Count)];
            result.Add(new Monster(
                template.Rank,
                template.Index,
                template.Name,
                template.BaseAttack,
                template.Defense,
                template.Speed,
                template.MaxHP,
                template.Cry,
                template.CryToT
            ));
        }

        return result;
    }
}