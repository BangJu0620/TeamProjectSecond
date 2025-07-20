using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TeamProjectSecond
{
    public static class Battle
    {
        private static int currentRerollCount;
        public static List<Dice>? sdList;
        public static List<Dice>? ddList;
        public static List<int>? sdValues;
        public static List<int>? ddValues;
        public static bool IsRerollPhase = false;

        private static Character player => Character.Instance;


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

        public static void StartBattle(Dungeon dungeon, List<Monster> enemies)
        {
            BattleScreen.Clear();
            BattleScreen.UpdateCurrentStage(dungeon);
            BattleScreen.UpdateMonsterUI(enemies);                 // UI
            BattleScreen.Log("몬스터가 나타났다!");                 // UI
            var turnOrder = DetermineInitiative(enemies);
            bool won = StartRounds(turnOrder, enemies);
            if (won)
            {
                dungeon.ProceedToNextStage();
            }
            else
            {
                Console.Clear();
                EventManager.Clear();
                BattleScreen.CenteredText(50, 12, 10, "YOU DIED", ConsoleColor.DarkRed, ConsoleColor.Black);
                BattleScreen.CenteredText(50, 25, 10, "아무 키나 눌러 계속", ConsoleColor.DarkGray, ConsoleColor.Black);
                Console.ReadKey();
                player.HealthPoint = player.MaxHealthPoint;
                player.ManaPoint = player.MaxManaPoint;
                return;
            }
        }



        private static bool StartRounds(List<object> turnOrder, List<Monster> enemies)
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
                        player.HealthPoint = 0;
                        return false;
                    }

                    if (enemies.All(e => e.IsDead))
                    {
                        // Reward.Grant(); ////////////////////////////////////////////// TODO
                        return true;
                    }
                }
            }
        }

        private static void PlayerTurn(List<Monster> enemies)
        {
            BattleScreen.Log("나의 턴 !");
            BattleScreen.DrawCommandOptions(" Dice  Roll !"," 스  킬"," 아 이 템");                    // UI

            while (true)
            {
                int? input = EventManager.CheckInput();
                if (input == 1) break;
                else if (input == 2) { SelectAndApplySkill(); continue; }
                else if (input == 3) { Inventory.ShowInventory(); return; }
                else { BattleScreen.Wrong(); BattleScreen.DrawCommandOptions(" Dice  Roll !", " 스  킬", " 아 이 템"); }
            }

            RollPhase();
            RerollPhase();
            SelectTarget(enemies);
        }

        private static void SelectAndApplySkill()
        {
            //var skills = player.ActiveSkills;
            //BattleScreen.Log("[스킬 목록 표시됨]");
            //for (int i = 0; i < skills.Count; i++)
            //    Console.WriteLine($"{i + 1}. {skills[i].Name} (MP {skills[i].ManaCost})");

            //int? selected = EventManager.CheckInput();
            //if (!selected.HasValue || selected < 1 || selected > skills.Count)
            //    return;

            //var skillData = skills[selected.Value - 1];
            //var skill = new Skill(skillData.Name, skillData.Description, skillData.ManaCost, skillData.RequiredLevel, skillData.IsActive, c => { });
            //skill.TryUse(player);
        }

        private static void RollPhase()
        {
            sdList = new List<Dice>
            {
                new Dice(1, 6, DiceType.SD, 1),
                new Dice(1, 6, DiceType.SD, 2)
            };
            ddList = new();
            for (int i = 0; i < player.DiceCount+4; i++)
            {
                var dd = new Dice(1, 6, DiceType.DD, 3 + i);
                ddList.Add(dd);
            }
                
            //눈 값 저장
            sdValues = sdList.Select(d => d.Roll()).ToList();
            ddValues = ddList.Select(d => d.Roll()).ToList();
            // 화면 출력
            BattleScreen.DrawSD(sdValues);
            BattleScreen.DrawDD(ddValues);
            BattleScreen.UpdateDDTotal(ddValues);
        }

        private static void RerollPhase()
        {
            
            while (true)
            {
                BattleScreen.DrawCommandOptions(" Go !", $" Reroll ( {currentRerollCount} left)", " 아 이 템");
                int? input = EventManager.CheckInput();
                if (input == 1) break;
                if (input == 2)
                {
                    if (currentRerollCount == 0 )
                    {
                        BattleScreen.DrawCommandOptions("리롤 횟수가 부족합니다.");
                        Console.ReadKey();
                        continue; 
                    }     /// 리롤횟수 0 이면 돌아가.
                    IsRerollPhase = true;
                    List<Dice> all = sdList.Concat(ddList).ToList();
                    BattleScreen.DrawCommandOptions("리롤할 주사위 번호를 선택");
                    BattleScreen.DrawSD(sdValues);
                    BattleScreen.DrawDD(ddValues);
                    int? index = EventManager.CheckInput() - 1;
                    if (index.HasValue && index.Value >= 0 && index.Value < all.Count)
                    {
                        int rolledValue = all[index.Value].Roll();
                        if (index.Value < sdList.Count)
                        {
                            // SD 쪽
                            sdValues[index.Value] = rolledValue;
                        }
                        else
                        {
                            // DD 쪽
                            int ddIndex = index.Value - sdList.Count;
                            ddValues[ddIndex] = rolledValue;
                        }
                        currentRerollCount--;
                        IsRerollPhase = false;
                        BattleScreen.DrawSD(sdValues);
                        BattleScreen.DrawDD(ddValues);
                        BattleScreen.UpdateDDTotal(ddValues);
                    }
                    else
                    {
                        BattleScreen.Wrong();  // 잘못된 인덱스 입력
                    }
                    continue;
                }
                if (input == 3)
                {
                    //           여기에 아이템을 입력.          //
                    break; // 일단 브레이크 ;;; ㅜ
                }
                else
                {
                    BattleScreen.Wrong(); BattleScreen.DrawCommandOptions(" Go !", $" Reroll {currentRerollCount}left", " 아 이 템");
                }
            }
        }

        private static void SelectTarget(List<Monster> enemies)
        {
            IsRerollPhase = false;
            while (true)
            {
                BattleScreen.DrawCommandOptions("공격할 대상을 선택");
                for (int i = 0; i < enemies.Count; i++)
                {
                    var m = enemies[i];
                }

                int? selected = EventManager.CheckInput();
                if (!selected.HasValue || selected < 1 || selected > enemies.Count)
                {
                    continue;
                }

                var target = enemies[selected.Value - 1];
                if (target.IsDead) return;

                int sdTotal = sdValues.Sum();
                bool isMiss = sdTotal <= 3;
                bool isCrit = sdTotal >= Character.Instance.CritThreshold;

                if (isMiss)
                {
                    BattleScreen.Log("공격이 빗나갔다!");
                    break;
                }

                int damage = CalculatePlayerDamage(ddValues, isCrit, target);
                target.CurrentHP -= damage;
                BattleScreen.Log($"{target.Name}에게 {damage} 데미지를 입혔다!");
                if (target.CurrentHP <= 0)
                {
                    target.IsDead = true;
                    BattleScreen.Log($"{target.Name}을(를) 처치했다!");
                    target.CurrentHP = 0;
                }
                BattleScreen.UpdateHPMP();
                BattleScreen.UpdateMonsterUI(enemies);
                break;
            }
        }

        private static int CalculatePlayerDamage(List<int> ddValues, bool isCrit, Monster target)
        {
            int ddTotal = ddValues.Sum();
            float damage = (ddTotal + player.TotalDamageBonus)
                           * player.TotalDamageMultiplier
                           * (isCrit ? player.CritMultiplier : 1f) - target.Defense;
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

            BattleScreen.Log($"{monster.Name}의 공격! {damage} 만큼 아프다.");
            if (player.HealthPoint <= 0)
            {
                player.HealthPoint = 0;
            }
            BattleScreen.UpdateHPMP();
        }
    }
}
