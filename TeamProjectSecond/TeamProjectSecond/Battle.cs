using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamProjectSecond
{
    public static class Battle
    {
        private static int currentRerollCount;
        private static List<Dice> sdList;
        private static List<Dice> ddList;
        private static Character player => Character.Instance;

        public static void StartBattle(List<Monster> enemies)
        {
            ShowWarningPhase(enemies);
            var turnOrder = DetermineInitiative(enemies);
            StartRounds(turnOrder, enemies);
        }

        private static void ShowWarningPhase(List<Monster> enemies)
        {
            Console.Clear();
            Console.WriteLine("몬스터들이 나타났다!");
            foreach (var monster in enemies)
            {
                Console.WriteLine($"{monster.Name}: {monster.Cry}");
            }
            Console.ReadKey();
        }

        private static List<object> DetermineInitiative(List<Monster> enemies)
        {
            List<(object entity, int initiative)> all = new();

            // Player
            var id = new Dice(1, 6, DiceType.ID, 0);
            int playerInitiative = id.Roll() + player.Speed;
            all.Add((player, playerInitiative));

            // Monsters
            foreach (var m in enemies)
            {
                var mid = new Dice(1, 6, DiceType.ID, 0);
                all.Add((m, mid.Roll() + m.Speed));
            }

            return all.OrderByDescending(t => t.initiative).Select(t => t.entity).ToList();
        }

        private static void StartRounds(List<object> turnOrder, List<Monster> enemies)
        {
            while (true)
            {
                currentRerollCount = player.RerollCount;

                foreach (var actor in turnOrder)
                {
                    if (actor is Character)
                        PlayerTurn(enemies);
                    else if (actor is Monster m && !m.IsDead)
                        MonsterTurn(m);

                    if (player.HealthPoint <= 0)
                    {
                        Console.WriteLine("당신은 쓰러졌다...");
                        Console.ReadKey();
                        return;
                    }

                    if (enemies.All(e => e.IsDead))
                    {
                        Console.WriteLine("전투 승리!");
                        // Reward.Grant(); // TODO: 보상 시스템 연결
                        Console.ReadKey();
                        return;
                    }
                }
            }
        }

        private static void PlayerTurn(List<Monster> enemies)
        {
            Console.Clear();
            Console.WriteLine("[플레이어 턴]");
            Console.WriteLine("1. 다이스 롤\n2. 스킬\n3. 아이템");

            while (true)
            {
                string? input = Console.ReadLine();
                if (input == "1") break;
                else if (input == "2") { SelectAndApplySkill(); continue; }
                else if (input == "3") { Inventory.ShowInventory(); return; }
                else Console.WriteLine("잘못된 입력입니다.");
            }

            RollDice();
            RerollPhase();
            SelectTargetAndAttack(enemies);
        }

        private static void SelectAndApplySkill()
        {
            var skills = player.ActiveSkills;
            Console.WriteLine("사용할 스킬을 선택하세요:");
            for (int i = 0; i < skills.Count; i++)
                Console.WriteLine($"{i + 1}. {skills[i].Name} (MP {skills[i].ManaCost})");

            if (!int.TryParse(Console.ReadLine(), out int selected) || selected < 1 || selected > skills.Count)
                return;

            var skillData = skills[selected - 1];
            var skill = new Skill(skillData.Name, skillData.Description, skillData.ManaCost, skillData.RequiredLevel, skillData.IsActive, c => { });
            skill.TryUse(player);
        }

        private static void RollDice()
        {
            sdList = new List<Dice>
            {
                new Dice(1, 6, DiceType.SD, 1),
                new Dice(1, 6, DiceType.SD, 2)
            };
            ddList = new();
            for (int i = 0; i < player.DiceCount - 2; i++)
                ddList.Add(new Dice(1, 6, DiceType.DD, 3 + i));

            Console.WriteLine("주사위 결과:");
            foreach (var die in sdList.Concat(ddList))
                Console.WriteLine($"{die}: {die.Roll()}");
        }

        private static void RerollPhase()
        {
            while (currentRerollCount > 0)
            {
                Console.WriteLine($"1. Go!\n2. Reroll ({currentRerollCount} left)");
                string? input = Console.ReadLine();
                if (input == "1") break;
                if (input == "2")
                {
                    List<Dice> all = sdList.Concat(ddList).ToList();
                    for (int i = 0; i < all.Count; i++)
                        Console.WriteLine($"{i}. {all[i]}");

                    Console.WriteLine("리롤할 주사위 번호 선택 (0 이상):");
                    if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < all.Count)
                    {
                        Console.WriteLine($"{all[index]} 리롤 결과: {all[index].Roll()}");
                        currentRerollCount--;
                    }
                }
            }
        }

        private static void SelectTargetAndAttack(List<Monster> enemies)
        {
            Console.WriteLine("공격할 몬스터를 선택하세요:");
            for (int i = 0; i < enemies.Count; i++)
            {
                var m = enemies[i];
                if (!m.IsDead)
                    Console.WriteLine($"{i + 1}. {m.Name} (HP: {m.CurrentHP})");
            }

            if (!int.TryParse(Console.ReadLine(), out int selected) || selected < 1 || selected > enemies.Count)
                return;

            var target = enemies[selected - 1];
            if (target.IsDead) return;

            int sdTotal = sdList.Sum(d => d.Roll());
            bool isMiss = sdTotal <= 3;
            bool isCrit = sdTotal >= 11;

            if (isMiss)
            {
                Console.WriteLine("공격이 빗나갔다!");
                return;
            }

            int damage = CalculatePlayerDamage(ddList, isCrit);
            target.CurrentHP -= damage;
            Console.WriteLine($"{target.Name}에게 {damage} 데미지를 입혔다!");
            if (target.CurrentHP <= 0)
            {
                target.IsDead = true;
                Console.WriteLine($"{target.Name}을(를) 처치했다!");
            }
        }

        private static int CalculatePlayerDamage(List<Dice> ddList, bool isCrit)
        {
            int ddSum = ddList.Sum(d => d.Roll());
            float damage = (ddSum + player.TotalDamageBonus)
                           * player.TotalDamageMultiplier
                           * (isCrit ? 1.6f : 1f);
            return (int)Math.Max(1, damage);
        }

        private static void MonsterTurn(Monster monster)
        {
            if (monster.IsDead) return;
            Console.WriteLine($"{monster.Name}이(가) 공격한다!");

            int ddSum = 0;
            for (int i = 0; i < monster.Rank; i++)
                ddSum += new Dice(1, 6, DiceType.DD, 3 + i).Roll();

            int damage = Math.Max(0, ddSum + monster.BaseAttack - player.DefensePoint);
            player.HealthPoint -= damage;
            Console.WriteLine($"{monster.Name}의 공격! {damage} 데미지를 입었다. 현재 HP: {player.HealthPoint}/{player.MaxHealthPoint}");
        }
    }
}