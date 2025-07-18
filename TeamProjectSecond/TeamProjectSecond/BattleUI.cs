using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamProjectSecond
{
    public static class BattleUI
    {
        public static void ShowWarningPhase(List<Monster> enemies)
        {
            EventManager.Clear();
            EventManager.To(52, "몬스터들이 나타났다!\n\n");
            foreach (var m in enemies)
                EventManager.To(44, $"{m.Name}: {m.Cry}\n");
            Console.ReadKey();
        }

        public static void ShowPlayerActionMenu()
        {
            EventManager.Clear();
            EventManager.To(52, "[플레이어 턴]\n\n");
            EventManager.To(44, "1. 다이스 롤\n");
            EventManager.To(44, "2. 스킬 사용\n");
            EventManager.To(44, "3. 아이템 사용\n");
            EventManager.Select();
        }

        public static void ShowDiceResults(List<Dice> sdList, List<Dice> ddList)
        {
            EventManager.Clear();
            EventManager.To(50, "주사위 결과\n\n");

            foreach (var d in sdList.Concat(ddList))
                EventManager.To(44, $"{d}: {d.Roll()}\n");

            EventManager.To(44, "\n");
        }

        public static void ShowRerollMenu(List<Dice> diceList, int remainingReroll)
        {
            EventManager.To(44, "1. Go !\n");
            EventManager.To(44, $"2. Reroll ({remainingReroll} left)\n");
            EventManager.Select();
        }

        public static void ShowRerollChoices(List<Dice> diceList)
        {
            EventManager.Clear();
            EventManager.To(50, "리롤할 주사위를 선택하세요 (0: 종료)\n\n");
            for (int i = 0; i < diceList.Count; i++)
                EventManager.To(44, $"{i}. {diceList[i]}\n");
            EventManager.Select();
        }

        public static void ShowTargetSelection(List<Monster> enemies)
        {
            EventManager.To(44, "\n공격할 몬스터를 선택하세요:\n");
            for (int i = 0; i < enemies.Count; i++)
            {
                var m = enemies[i];
                if (!m.IsDead)
                    EventManager.To(44, $"{i + 1}. {m.Name} (HP: {m.CurrentHP}/{m.MaxHP})\n");
            }
            EventManager.Select();
        }

        public static void AnnounceMiss()
        {
            EventManager.Announce(52, "공격이 빗나갔습니다!");
        }

        public static void AnnounceHit(string monsterName, int dmg)
        {
            EventManager.Announce(50, $"{monsterName}에게 {dmg} 데미지를 입혔습니다!");
        }

        public static void AnnounceKill(string monsterName)
        {
            EventManager.Announce(50, $"{monsterName}을(를) 처치했습니다!");
        }

        public static void AnnouncePlayerDown()
        {
            EventManager.Announce(52, "당신은 쓰러졌습니다...");
        }

        public static void AnnounceVictory()
        {
            EventManager.Announce(50, "전투에서 승리했습니다!");
        }

        public static void ShowMonsterAttack(string monsterName, int damage, int playerHP, int maxHP)
        {
            EventManager.Announce(46, $"{monsterName}의 공격! {damage} 데미지를 입었습니다. (HP: {playerHP}/{maxHP})");
        }

        public static void ShowSkillList(List<SkillData> skills)
        {
            EventManager.Clear();
            EventManager.To(50, "사용할 스킬을 선택하세요:\n\n");
            for (int i = 0; i < skills.Count; i++)
            {
                var s = skills[i];
                EventManager.To(44, $"{i + 1}. {s.Name} (MP {s.ManaCost}) - {s.Description}\n");
            }
            EventManager.Select();
        }
    }
}