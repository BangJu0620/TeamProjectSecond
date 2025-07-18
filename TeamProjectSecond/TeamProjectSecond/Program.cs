using TextRPGQuest.QuestSystem;

namespace TeamProjectSecond
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            ClassTypeChange classTypeChange = new ClassTypeChange();
            Intro.DisplayIntro(classTypeChange);
            EventManager.DisplayMainUI();
        }
    }
}
