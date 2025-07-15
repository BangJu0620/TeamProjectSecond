using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeamProjectSecond;

namespace TeamProjectSecond
{
    public class Rest
    {
        private int? restCost;          // restCost라는 null값도 받을 수 있는 변수를 선언
        public int RestCost             // 그 변수에 값을 입력하기 위해 호출되는 변수를 선언
        {
            get                         // 외부에서 RestCost의 값을 호출하는 순간 get이라는 함수가 작동
            {
                if (restCost == null)   // restCost는 선언만 됐을 뿐 아무런 값도 입력되지 않았기 때문에
                {
                    restCost = 500;     // if문이 동작하고, restCost에 500이라는 값이 입력됩니다
                }
                return (int)restCost;   // 조건식의 바깥. 즉 언제나 작동되는 부위입니다
            }                           // 호출된 RestCost에 (int)restCost라는 값을 반환합니다.
                                        // int?는 int를 암시적 형변환 하는 것이 불가능하기 때문에 
                                        // (int)를 통해 형변환합니다.
            set
            {
                restCost = value;       // 외부에서 RestCost에 값을 입력하려고 시도하면 set이라는 함수가 작동합니다
            }                           // 그 결과 restCost에는 입력한 값, value가 입력됩니다.
        }
        public void RestInVillage()
        {
            var character = Character.Instance;         // character라는 변수를 선언 + 이 변수는 Character클래스에 있는 Instance라는 녀석이다.

            while (true)
            {
                if (character.HealthPoint == character.MaxHealthPoint)      // 체력이 최대일 경우 휴식 시도는 실패합니다.
                {
                    Console.WriteLine("이미 체력이 최대치입니다.");
                    Console.ReadKey();                                      // 키를 입력하면 다시 메뉴가 반복됩니다.
                }
                else if (character.HealthPoint < character.MaxHealthPoint)
                {
                    if (character.Gold >= RestCost)
                    {

                        if (character.HealthPoint + 100 > character.MaxHealthPoint)     // 체력을 회복했을 때 그 값이 최대체력보다 많다면
                        {                                                               // 최대체력까지만 회복합니다.
                            character.HealthPoint = character.MaxHealthPoint;
                            character.Gold -= RestCost;
                            Console.WriteLine($"체력이 {character.MaxHealthPoint}까지 회복되었습니다.");
                            Console.ReadKey();   // 키를 입력해야만 다음 장면이 진행
                            break;               // 반복문을 깨고 나갑니다.
                        }

                        else if (character.HealthPoint + 100 <= character.MaxHealthPoint)  //최대체력보다 적거나 같다면 회복량만큼 회복합니다
                        {
                            character.HealthPoint += 100;
                            character.Gold -= RestCost;
                            Console.WriteLine($"체력이 {character.HealthPoint}까지 회복되었습니다.");
                            Console.ReadKey();   //키를 입력해야만 다음 장면이 진행
                            break;               // 반복문을 깨고 나갑니다.
                        }
                    }
                    else if (character.Gold < RestCost)  // 휴식을 시도했으나 돈이 부족할 때, 휴식 시도는 실패합니다.
                    {
                        Console.WriteLine("골드가 부족합니다. 이 가난뱅이!");
                        Console.ReadKey();               // 키를 입력하면 다시 메뉴가 반복됩니다.
                    }
                }
            }
        }
    }
}