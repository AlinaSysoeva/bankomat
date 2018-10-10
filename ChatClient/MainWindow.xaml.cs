using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankClient.BankHost;
using System.IO;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceBankCallback
    {
        bool isConnected = false;
        ServiceBankClient client;
        int ID;
        int fiveh = 0;
        int sto = 0;
        int fivet = 0;
        int onet = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceBankClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnDicon.Content = "Disconnect";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.IsEnabled = true;
                bConnDicon.Content = "Connect";
                isConnected = false;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }

        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);            
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
           
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client!=null)
                {
                   
                        client.TakeMoneyMonualy(Convert.ToInt32(tbMessage.Text));
                    client.LogToFile("Клиент снял "+ Convert.ToInt32(tbMessage.Text) +" рублей");
                    //client.SendMsg(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                    
                }               
            }
        }

        private void bAddOneH_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                lbChat.Items.Clear();
                client.AddMoney(100);
                client.LogToFile("Клиент внес 100 рублей");
                sto += 1;
                if (sto >= 1)
                    lbSto.IsEnabled = true;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ConnectUser();
            if (sto < 1)
                lbSto.IsEnabled = false;
            if (fiveh < 1)
                lbFiveH.IsEnabled = false;
            if (onet < 1)
                lbOneTh.IsEnabled = false;
            if (fivet < 1)
                lbFiveTh.IsEnabled = false;
        }

        private void bAddFiveH_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.AddMoney(500);
                client.LogToFile("Клиент внес 500 рублей");

                fiveh += 1;
                if (fiveh >= 1)
                    lbFiveH.IsEnabled = true;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bAddOneS_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.AddMoney(1000);
                client.LogToFile("Клиент внес 1000 рублей");
                onet += 1;
                if (onet >= 1)
                    lbOneTh.IsEnabled = true;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bAddFiveS_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.AddMoney(5000);
                client.LogToFile("Клиент внес 5000 рублей");
                fivet += 1;
                if (fivet >= 1)
                    lbFiveTh.IsEnabled = true;
            }
            else lbChat.Items.Add("Нет подключения к серверу");

        }

        private void bBalance_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.ShowBalance();

                client.LogToFile("Клиент узнал баланс");
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bTakeOneH_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.TakeMoney(100);
                client.LogToFile("Клиент снял 100 рублей");
                sto -= 1;
                if (sto == 0)
                    lbSto.IsEnabled = false;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bTakeFiveH_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.TakeMoney(500);
                client.LogToFile("Клиент снял 500 рублей");
                fiveh -= 1;
                if (fiveh == 0)
                    lbFiveH.IsEnabled = false;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bTakeOneS_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.TakeMoney(1000);
                client.LogToFile("Клиент снял 1000 рублей");
                onet -= 1;
                if (onet == 0)
                    lbOneTh.IsEnabled = false;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void bTakeFiveS_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                client.TakeMoney(5000);
                client.LogToFile("Клиент снял 5000 рублей");
                fivet -= 1;
                if (fivet == 0)
                    lbFiveTh.IsEnabled = false;
            }
            else lbChat.Items.Add("Нет подключения к серверу");
        }

        private void Label_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sto >= 1)
                lbSto.IsEnabled = true;
            else lbSto.IsEnabled = false;
        }
    }
}
