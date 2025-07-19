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
            BattleScreen.DrawMonsterArea(enemies);
            BattleScreen.Log("몬스터가 나타났다!");
            var turnOrder = DetermineInitiative(enemies);
            StartRounds(turnOrder, enemies);
        }

        private static List<object> DetermineInitiative(List<Monster> enemies)
        {
            List<(object entity, int initiative)> all = new();

            var id = new Dice(1, 6, DiceType.ID, 0);
            int playerInitiative = id.Roll() + player.Speed;
            all.Add((player, playerInitiative));

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
                        BattleScreen.Log("당신은 쓰러졌습니다...");
                        return;
                    }

                    if (enemies.All(e => e.IsDead))
                    {
                        BattleScreen.Log("전투에서 승리했습니다!");
                        // Reward.Grant(); // TODO
                        return;
                    }
                }
            }
        }

        private static void PlayerTurn(List<Monster> enemies)
        {
            BattleScreen.Log("당신의 턴입니다. 행동을 선택하세요.");
            BattleScreen.DrawCommandOptions(currentRerollCount);

            while (true)
            {
                int? input = EventManager.CheckInput();
                if (input == 1) break;
                else if (input == 2) { SelectAndApplySkill(); continue; }
                else if (input == 3) { Inventory.ShowInventory(); return; }
                else EventManager.Wrong();
            }

            RollDice();
            RerollPhase();
            SelectTargetAndAttack(enemies);
        }

        private static void SelectAndApplySkill()
        {
            var skills = player.ActiveSkills;
            BattleScreen.Log("[스킬 목록 표시됨]");
            for (int i = 0; i < skills.Count; i++)
                Console.WriteLine($"{i + 1}. {skills[i].Name} (MP {skills[i].ManaCost})");

            int? selected = EventManager.CheckInput();
            if (!selected.HasValue || selected < 1 || selected > skills.Count)
                return;

            var skillData = skills[selected.Value - 1];
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

            var sdValues = sdList.Select(d => d.Roll()).ToList();
            var ddValues = ddList.Select(d => d.Roll()).ToList();

            BattleScreen.DrawDiceRow(sdValues, 10, 15, "Strike Dice");
            BattleScreen.DrawDiceRow(ddValues, 10, 23, "Damage Dice");
            BattleScreen.DrawDDTotal(ddValues.Sum(), 60, 22);
        }

        private static void RerollPhase()
        {
            while (currentRerollCount > 0)
            {
                BattleScreen.DrawCommandOptions(currentRerollCount);
                int? input = EventManager.CheckInput();
                if (input == 1) break;
                if (input == 2)
                {
                    List<Dice> all = sdList.Concat(ddList).ToList();
                    for (int i = 0; i < all.Count; i++)
                        Console.WriteLine($"{i}. {all[i]}");

                    BattleScreen.Log("리롤할 주사위 번호 선택 (0 이상):");
                    int? index = EventManager.CheckInput();
                    if (index.HasValue && index.Value >= 0 && index.Value < all.Count)
                    {
                        BattleScreen.Log($"{all[index.Value]} 리롤 결과: {all[index.Value].Roll()}");
                        currentRerollCount--;
                    }
                }
            }
        }

        private static void SelectTargetAndAttack(List<Monster> enemies)
        {
            BattleScreen.Log("공격할 대상을 선택하세요.");
            for (int i = 0; i < enemies.Count; i++)
            {
                var m = enemies[i];
                if (!m.IsDead)
                    Console.WriteLine($"{i + 1}. {m.Name} (HP: {m.CurrentHP})");
            }

            int? selected = EventManager.CheckInput();
            if (!selected.HasValue || selected < 1 || selected > enemies.Count)
                return;

            var target = enemies[selected.Value - 1];
            if (target.IsDead) return;

            int sdTotal = sdList.Sum(d => d.Roll());
            bool isMiss = sdTotal <= 3;
            bool isCrit = sdTotal >= 11;

            if (isMiss)
            {
                BattleScreen.Log("공격이 빗나갔습니다!");
                return;
            }

            int damage = CalculatePlayerDamage(ddList, isCrit);
            target.CurrentHP -= damage;
            BattleScreen.Log($"{target.Name}에게 {damage} 데미지를 입혔습니다!");
            if (target.CurrentHP <= 0)
            {
                target.IsDead = true;
                BattleScreen.Log($"{target.Name}을(를) 처치했습니다!");
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

            int ddSum = 0;
            for (int i = 0; i < monster.Rank; i++)
                ddSum += new Dice(1, 6, DiceType.DD, 3 + i).Roll();

            int damage = Math.Max(0, ddSum + monster.BaseAttack - player.DefensePoint);
            player.HealthPoint -= damage;

            BattleScreen.Log($"{monster.Name}의 공격! {damage} 데미지를 입었습니다. (HP: {player.HealthPoint}/{player.MaxHealthPoint})");
        }
    }
}
