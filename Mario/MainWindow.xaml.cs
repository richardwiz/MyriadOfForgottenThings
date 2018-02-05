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
using Amqp;

namespace Mario
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }

      private void btnSend_Click(object sender, RoutedEventArgs e)
      {
         string broker =  @"amqp://localhost:5672";
         string address = @"amq.topic";

         Address brokerAddr = new Address(broker);
         Connection connection = new Connection(brokerAddr);
         Session session = new Session(connection);

         SenderLink senderLink = new SenderLink(session, "helloworld-sender", address);
         ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

         Message helloOut = new Message(this.txtMessage.Text);
         senderLink.Send(helloOut);

         MessageBox.Show(String.Format("Message : {0} => Sent to {1}:{2}", helloOut, broker, address));
      }
   }
}
