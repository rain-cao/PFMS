using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace PFMS_S
{
    class MailClass
    {
        public MailClass()
        { 
        }

        public class EmailParameterSet
        {
            /// <summary>
            /// 发送邮件服务器的地址
            /// </summary>
            public string mailServer { get; set; }

            /// <summary>
            /// 发送邮件端口号
            /// </summary>
            public int mailPort { get; set; }

            /// <summary>
            /// 发送邮件的账号
            /// </summary>
            public string mailFrom { get; set; }

            /// <summary>
            /// 发送邮件账号的密码
            /// </summary>
            public string mailFromPassword { get; set; }

            /// <summary>
            /// 发送邮件的主题
            /// </summary>
            public string mailSubject { get; set; }

            /// <summary>
            /// 发送邮件的内容
            /// </summary>
            public string mailBody { get; set; }

            /// <summary>
            /// 收件人清单
            /// </summary>
            public List<string> mailTo { get; set; }

            /// <summary>
            /// 抄送人清单
            /// </summary>
            public List<string> mailCopy { get; set; }
        }

        public void SendMail(EmailParameterSet mailPar)
        {
            SmtpClient mailClient = null;
            MailMessage message = null;
            try
            {
                //确定smtp服务器端的地址，实列化一个客户端smtp 
                mailClient = new SmtpClient();
                //设定Mail服务器地址及端口号
                mailClient.Host = mailPar.mailServer;
                mailClient.Port = mailPar.mailPort;
                //是否随着请求一起发
                mailClient.UseDefaultCredentials = true;
                //指定如何发邮件 是以网络来发
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //服务器支持安全接连，安全则为true
                mailClient.EnableSsl = false;
                //用户登录信息
                mailClient.Credentials = new System.Net.NetworkCredential(mailPar.mailFrom, mailPar.mailFromPassword);
                //构造一个Email对象
                message = new MailMessage();
                //构造一个发件人对象
                message.From = new MailAddress(mailPar.mailFrom);
                //构造一个收件人集合
                foreach (string toStr in mailPar.mailTo)
                {
                    message.To.Add(toStr);
                }
                //构造一个抄送人员集合
                foreach (string ccStr in mailPar.mailCopy)
                {
                    message.CC.Add(ccStr);
                }
                //设置邮件表头
                /*message.Headers.Add("X-Priority", "3");
                message.Headers.Add("X-MSMail-Priority", "Normal");
                message.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
                message.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                message.Headers.Add("ReturnReceipt", "1");*/
                //设置邮件主题
                message.Subject = mailPar.mailSubject;
                //设置邮件主体
                message.Body = mailPar.mailBody;
                //主题编码
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                //主体编码
                message.BodyEncoding = System.Text.Encoding.UTF8;
                //设置此邮件优先级
                message.Priority = System.Net.Mail.MailPriority.High;
                //邮件主体可以为html
                message.IsBodyHtml = false;
                //mailClient.Timeout = 10;
                mailClient.Send(message);
                message.Dispose();
                message = null;
                mailClient.Dispose();
                mailClient = null;
            }
            catch (Exception msg)
            {
                if (message != null)
                {
                    message.Dispose();
                    message = null;
                }
                if (mailClient != null)
                {
                    mailClient.Dispose();
                    mailClient = null;
                }
                throw new Exception("邮件发送失败：" + msg.Message);
            }
        }
    }
}
