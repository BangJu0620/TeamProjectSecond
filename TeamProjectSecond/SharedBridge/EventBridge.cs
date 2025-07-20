using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedBridge
{
    public static class EventBridge
    {
        public static Action OnClear;
        public static Action<int> OnTo;
        public static Action<int, string> OnToWithText;
        public static Action<int, string> OnToS;
        public static Action OnSelect;
        public static Action OnWrong;
        public static Action<int, string> OnAnnounce;
        public static Func<int?> OnCheckInput;
        public static Func<int> OnGetGold;
        public static Func<int> OnGetExp;
        public static Action<int> OnAddGold;
        public static Action<int> OnAddExp;
        public static Func<int> OnGetMpPotionCount;
    }
}
