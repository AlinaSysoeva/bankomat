using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;


namespace wcf_bank
{
  
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceBank : IServiceBank
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        int totalMoney = 0;
        int oneHundredBanknote = 0;
        int fiveHundredBanknote = 0;
        int oneThousandBanknote = 0;
        int fiveThousandBanknote = 0;




        public int Connect(string name)
        {

            ServerUser user = new ServerUser() {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            users.Add(user);
            return user.ID;

           
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user!=null)
            {
                users.Remove(user);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name+" ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerBankCallback>().MsgCallback(answer);
            }
        }

        public void AddMoney(int summ)
        {
            totalMoney += summ;
            SendMsg(" Добавлено "+summ+" рублей.", 0);
            switch (summ)
            {
                case 100:
                    { oneHundredBanknote += 1; }
                    break;
                case 500:
                    { fiveHundredBanknote += 1; }
                    break;
                case 1000:
                    { oneThousandBanknote += 1; }
                    break;
                case 5000:
                    { fiveThousandBanknote += 1; }
                    break;
            }
         }

        public void ShowBalance()
        {
                 SendMsg(" Всего "+ totalMoney + " на счете.", 0);
        }

        public void TakeMoney(int summ)
        {
            bool correct=true;
            if (summ <= totalMoney)
            {
                if (summ == 5000 && fiveThousandBanknote >= 1 || oneHundredBanknote >= 50 || fiveHundredBanknote>=10 ||oneThousandBanknote>=5)
                {
                    totalMoney -= 5000;
                    fiveThousandBanknote -= 1;
                }
                else if (summ == 1000 && oneThousandBanknote >= 1 || oneHundredBanknote >= 10 || fiveHundredBanknote>=2)
                {
                    totalMoney -= 1000;
                    oneThousandBanknote -= 1;
                }
                else if (summ == 500 && fiveHundredBanknote >= 1||oneHundredBanknote>=5)
                {
                    
                    totalMoney -= 500;
                    fiveHundredBanknote -= 1;
                    
                }
                else if (summ == 100 && oneHundredBanknote >= 1)
                {
                    totalMoney -= 100;
                    oneHundredBanknote -= 1;
                }
                else
                {
                    correct = false;
                    SendMsg("Некорректная сумма", 0);
                }
                if (correct == true)
                {
                    SendMsg(" Выдано " + summ + " рублей.", 0);
                }
           }
            else
                SendMsg("Недостаточно средств на счете", 0);
        }

        public void LogToFile(string text)
        {
            using (FileStream fstream = new FileStream(@"A:\log.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }
        }

        public void TakeMoneyMonualy(int sum)
        {
            int summa = sum;
            bool correct = true;
            if (sum <= totalMoney)
            {
              while (sum!=0 && correct == true)
                {
                    if (sum>=5000 && fiveThousandBanknote>=1)
                    {
                        totalMoney -= 5000;
                        sum -= 5000;
                        fiveThousandBanknote -= 1;
                    }
                    else if (sum >= 1000 && oneThousandBanknote>=1)
                    {
                        totalMoney -= 1000;
                        sum -= 1000;
                        oneThousandBanknote -= 1;
                    }
                    else if (sum >= 500 && fiveHundredBanknote>=1)
                    {
                        totalMoney -= 500;
                        sum -= 500;
                        fiveHundredBanknote -= 1;
                    }
                    else if (sum >= 100 && oneHundredBanknote>=1)
                    {
                        totalMoney -= 100;
                        sum -= 100;
                        oneHundredBanknote -= 1;
                    }
                    else
                    {
                        correct = false;
                        SendMsg("Некорректная сумма",0);
                    }
                }
              if (correct == true)
                SendMsg(" Выдано " + summa + " рублей.", 0);
            }
            else
                SendMsg("Недостаточно средств.", 0);
        }
    }
}
