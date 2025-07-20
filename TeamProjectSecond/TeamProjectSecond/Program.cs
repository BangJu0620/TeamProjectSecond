using TextRPGQuest.QuestSystem;
using SharedBridge;

namespace TeamProjectSecond
{
    public class Program
    {
        static void Main(string[] args)
        {
            // TextRPGQuest.Data 프로젝트에서 TeamProjectSecond 프로젝트 내의 메서드를 쓰기위한 것들
            EventBridge.OnClear = () => EventManager.Clear();
            EventBridge.OnTo = i => EventManager.To(i);
            EventBridge.OnToS = (i, text) => EventManager.ToS(i, text);
            EventBridge.OnToWithText = (i, text) => EventManager.To(i, text);
            EventBridge.OnSelect = () => EventManager.Select();
            EventBridge.OnWrong = () => EventManager.Wrong();
            EventBridge.OnAnnounce = (i, text) => EventManager.Announce(i, text);
            EventBridge.OnCheckInput = () => EventManager.CheckInput();
            EventBridge.OnGetGold = () => Character.Instance.Gold;
            EventBridge.OnGetExp = () => Character.Instance.Exp;
            EventBridge.OnAddGold = amount => Character.Instance.Gold += amount;
            EventBridge.OnAddExp = amount => Character.Instance.Exp += amount;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            ClassTypeChange classTypeChange = new ClassTypeChange();
            Intro.DisplayTitle(classTypeChange);
            EventManager.DisplayMainUI();
        }
    }
}
