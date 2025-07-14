using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectSecond
{
    public class EventManager
    {
        // 다른 UI를 아래 케이스에 메서드를 넣어서 불러오고
        // 그 메서드 끝에 break 넣어주시면 다시 이 화면으로 돌아올 겁니다.
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

        // UI를 표현하기만 하는 거라면 static으로 하는 게 깔끔할 거 같기도 하고?
        // 아니면 싱글톤을 하거나, 메인 메서드에 객체를 생성하거나인데
        // 아니면 인터페이스? 델리게이트랑 이벤트?
        // 델리게이트랑 이벤트가 UI 쪽에서 쓴다는데 이걸 써볼까?
    }
}
