using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamProjectSecond
{
    public enum DiceType { ID, SD, DD } // 주사위 타입 ID: InitiativeDice, SD: StrikeDice, DD: DamageDice

    public class Dice
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int? FixedEyes { get; set; } = null;
        public List<int> Excluded { get; set; } = new List<int>();
        public Func<int, int> Transform { get; set; } = null;

        public DiceType Type { get; set; }
        public int Index { get; set; }       // 1,2 는 SD, 3번부터 DD

        public Dice(int min, int max, DiceType type, int index)
        {
            if (min > max)
                throw new ArgumentException("뭔가 지금 최솟값이 최댓값보다 커서 오류납니다. 김정민에게 문의해주세요. 제탓입니다.");

            Min = min;
            Max = max;
            Type = type;
            Index = index;
        }

        public int Roll()
        {
            if (FixedEyes.HasValue)
                return ApplyTransform(FixedEyes.Value);

            List<int> candidates = Enumerable.Range(Min, Max - Min + 1)
                                             .Where(n => !Excluded.Contains(n))
                                             .ToList();

            if (candidates.Count == 0)
                throw new InvalidOperationException("굴릴 수 있는 값의 경우가 없어져 버렸습니다. 김정민에게 문의해주세요. 제탓입니다.");

            int rolled = candidates[Random.Shared.Next(candidates.Count)];
            return ApplyTransform(rolled);
        }

        private int ApplyTransform(int value)
        {
            return Transform != null ? Transform(value) : value;
        }

        public override string ToString()   // 이게 굳이 필요할까요?
        {
            return $"{Type}{Index}";        // 저는 아직 모르겠습니당.
        }
    }
}