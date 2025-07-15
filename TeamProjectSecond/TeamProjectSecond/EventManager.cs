using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public class EventManager
    {
        // UI 출력하는 메서드를 while, for 반복문을 이용해서 무한루프 상태로 만들어주세요.
        // 그리고 break를 넣으면 무한루프 반복문이 종료되면서 그 메서드가 끝나게 해주시면 됩니다.
        // UI 출력하는 메서드를 아래 주석 위치에 맞게 넣어주시고 실행하시면 될겁니다.
        public static void DisplayMainUI()
        {
            while (true)
            {
                Console.WriteLine("스파르타 마을");
                Console.WriteLine("어서오세요");
                Console.WriteLine("선택지 1, 2, 3, 4, 5, 6, 7");
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                string input = Console.ReadLine();
                int userSelect;
                bool isInt = int.TryParse(input, out userSelect);

                if (isInt)
                {
                    switch (userSelect)
                    {
                        case 1:
                            // 상태보기
                            Console.Clear();
                            Console.WriteLine("상태보기");
                            break;
                        case 2:
                            // 인벤토리
                            Console.Clear();
                            Console.WriteLine("인벤토리");
                            break;
                        case 3:
                            // 상점
                            Console.Clear();
                            Console.WriteLine("상점");
                            break;
                        case 4:
                            // 던전
                            Console.Clear();
                            Console.WriteLine("던전");
                            break;
                        case 5:
                            // 휴식하기
                            Console.Clear();
                            Console.WriteLine("휴식하기");
                            break;
                        case 6:
                            // 퀘스트
                            Console.Clear();
                            Console.WriteLine("퀘스트");
                            break;
                        case 7:
                            // 저장/불러오기
                            Console.Clear();
                            Console.WriteLine("저장/불러오기");
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
