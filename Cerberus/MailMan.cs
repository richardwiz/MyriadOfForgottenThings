using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Diagnostics;

namespace Cerberus
{
	[DebuggerDisplay("{ToString()}")]
	public class MailMan
	{
		#region Auto Properties

		public String MailHost { get; set; }
		public String DefaultSubject { get; set; }
		public String Sender { get; set; }
		public List<String> Recipients { get; set; }
        public Boolean SendEmailAsHTML  { get; set; }
		#endregion

        #region Private Variables

        SmtpClient _client;
        Boolean _sendEmailAsHTML;

		#endregion

		#region Constructors

        public MailMan() : this(String.Empty, String.Empty, String.Empty, new List<String>(), false) { }
		public MailMan(String host, String subject, String sender, List<String> recipients, Boolean sendAsHtml)
		{
			Recipients = recipients;
			MailHost = host;
			DefaultSubject = subject;
			Sender = sender;
            _sendEmailAsHTML = sendAsHtml;
			_client = new SmtpClient(MailHost);        
		}

		#endregion

		#region Send Mail Methods

        public void Send(String body)
        {
            try
            {
                MailMessage message = PrepareEmail(DefaultSubject, body);

                _client.Send(message);
            }
            catch (SmtpException sex)
            {
                throw sex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public void Send( String subject, String body )
		{
			try
			{
				MailMessage message = PrepareEmail(subject, body);

				_client.Send(message);
				
			}
			catch (SmtpException sex)
			{
				throw sex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public void Send(String subject, List<String> messages, Boolean isErrorCatalogue)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                if(isErrorCatalogue)
                    builder.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!ERRORS OCCURED DURING EXECUTION OF THE TASK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");

                foreach (String msg in messages)
                    builder.AppendLine(msg);

                MailMessage message = PrepareEmail(subject, builder.ToString());

                _client.Send(message);

            }
            catch (SmtpException sex)
            {
                throw sex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public void Send(String subject, String body, List<String> attachments)
		{			
			try
			{
				MailMessage message = PrepareEmail(subject, body);

                foreach(String attachment in attachments)
				    message.Attachments.Add(new Attachment(attachment));

				_client.Send(message);			
			}
			catch (SmtpException sex)
			{
				throw sex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

        #region Mail Prep Metthods

		private MailMessage PrepareEmail(String subject, String body)
		{
			MailMessage msg = new MailMessage();

			foreach (String recipient in Recipients)
			{
				msg.To.Add(new MailAddress(recipient));			
			}

            if (String.IsNullOrEmpty(DefaultSubject) && String.IsNullOrEmpty(subject))
                subject = "No Subject Assigned...";

			msg.From = new MailAddress(Sender);
			msg.Sender = msg.From;
			msg.Subject = (String.IsNullOrEmpty(subject) ? DefaultSubject : subject);
			msg.Body = body;

            if (SendEmailAsHTML)
                msg.IsBodyHtml = true;

			return msg;
		}

        #endregion

        #region Object Overrides

        public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach(String recipient in Recipients)
			{
				builder.AppendFormat(" {0} ", recipient);
			}
			return String.Format("{0}->{1}", DefaultSubject, builder.ToString());
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

    }
}
