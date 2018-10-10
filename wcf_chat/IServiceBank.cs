using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_bank
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceBank" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServerBankCallback))]
    public interface IServiceBank
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);

        [OperationContract(IsOneWay = true)]
        void AddMoney(int sum);

        [OperationContract(IsOneWay = true)]
        void ShowBalance();

        [OperationContract(IsOneWay = true)]
        void TakeMoney(int sum);

        [OperationContract(IsOneWay = true)]
        void LogToFile(string text);

        [OperationContract(IsOneWay = true)]
        void TakeMoneyMonualy(int sum);

        

    }

    public interface IServerBankCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}
