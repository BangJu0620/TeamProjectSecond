using TextRPG_Quest_Solution.QuestSystem;

namespace TeamProjectSecond
{
    public class Program
    {
        static void Main(string[] args)
        {
            QuestDatabase.Register();
            EventManager.DisplayMainUI();
        }
    }
}
