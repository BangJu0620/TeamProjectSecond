using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

//퀘스트 보상 지급 스크립트

namespace TextRPG_Quest_Solution
{
    public class Player
    {
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }

        public Player(string name)
        {
            Name = name;
            Gold = 0;
            Exp = 0;
        }

        public void ReceiveReward(int gold, int exp)
        {
            Gold += gold;
            Exp += exp;
            Console.WriteLine($"[보상] 골드 {gold} / 경험치 {exp} 추가! (현재 Gold: {Gold}, Exp: {Exp})");
        }
    }
}

