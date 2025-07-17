using TextRPG_Quest_Solution.QuestSystem;

namespace TeamProjectSecond
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            ClassTypeChange classTypeChange = new ClassTypeChange();
            QuestDatabase.Register();
            Intro.DisplayIntro(classTypeChange);
            EventManager.DisplayMainUI();
        }
    }
}
