
using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue messageQ = new MessageQueue();
        public void sendData2Queue(string token)
        {
            messageQ.Path = @".\private$\Tokens";
            if (!MessageQueue.Exists(messageQ.Path))
            {
                MessageQueue.Create(messageQ.Path);
            }
            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted;
            messageQ.Send(token);
            messageQ.BeginReceive();
            messageQ.Close();
        }

        private void MessageQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messageQ.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string subject = "FundoNote App Reset Link";
            string body = token;




            var SMTP = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,//port num for gmail is 587
                Credentials = new NetworkCredential("bandarisrikanth8096@gmail.com", "etcekhnomeavsdbs"),
                EnableSsl = true,
            };
            SMTP.Send("bandarisrikanth8096@gmail.com", "bandarisrikanth8096@gmail.com", subject, body);
            messageQ.BeginReceive();
        }
    }
}
