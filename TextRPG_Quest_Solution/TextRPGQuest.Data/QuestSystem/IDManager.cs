using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextRPG_Quest_Solution.QuestSystem
{
    public static class IDManager
    {
        private static string filePath = "quest_id.txt";
        private static int lastId;

        static IDManager()
        {
            if (File.Exists(filePath))
                lastId = int.Parse(File.ReadAllText(filePath));
            else
                lastId = 0;
        }

        public static int Generate()
        {
            lastId++;
            File.WriteAllText(filePath, lastId.ToString());
            return lastId;
        }
    }
}

