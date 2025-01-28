using Platform.AbstractionClass;
using Platform.EmailSenderConsole.Util;
using Platform.Infra;
using Platform.Messages;
using Platform.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Platform.EmailSenderConsole
{
    internal class Program
    {
        /// <summary> 待發郵件管理者 </summary>
        private static MailPoolManager _mgr = new MailPoolManager();
        
        /// <summary> 模擬現在登入者 </summary>
        private const string _cUserID = "EmailSenderConsole";
        
        /// <summary> 預設的顏色 </summary>
        private readonly static ConsoleColor _defaultColor = Console.ForegroundColor;

        static void Main(string[] args)
        {
            WriteMessage("Mail Sender Starting.");

            // 初始化設定
            WriteMessage("Config Loader Starting.");
            ConfigLoader.Init();


            // 讀取待發信件
            var list = _mgr.GetWaitingList();
            var list_WithCC = _mgr.GetWaitingListWithCC();
            WriteMessage($"Mail pool readed. Total rows: {list.Count + list_WithCC.Count}. ");


            // 逐一寄信，並回寫執行結果
            foreach (var item in list)
            {
                try
                {
                    Console.ForegroundColor = _defaultColor;
                    WriteMessage($"Sending mail[{item.ID.ToString("D6")}] to {item.RecipientEmail}, title is {item.Subject}. ");
                    EMailSender.Send(item.RecipientEmail, new EMailContent() { Title = item.Subject, Body = item.Body });

                    Console.ForegroundColor = ConsoleColor.Green;
                    WriteMessage($"Mail[{item.ID.ToString("D6")}] send success. ");
                    _mgr.WriteSuccessMessage(item, _cUserID, DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteMessage($"Mail[{item.ID.ToString("D6")}] send fail. Please check log. ");
                    _mgr.WriteFailResult(item, ex, _cUserID, DateTime.Now);
                }
            }


            // 逐一寄信，並回寫執行結果
            foreach (var item in list_WithCC)
            {
                try
                {
                    Console.ForegroundColor = _defaultColor;
                    WriteMessage($"Sending MailWithCC[{item.ID.ToString("D6")}], title is {item.Subject}. ");
                    EMailSender.SendWithCC(item.Receivers, item.CCs, new EMailContent() { Title = item.Subject, Body = item.Body });

                    Console.ForegroundColor = ConsoleColor.Green;
                    WriteMessage($"MailWithCC[{item.ID.ToString("D6")}] send success. ");
                    _mgr.WriteSuccessMessage(item, _cUserID, DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteMessage($"MailWithCC[{item.ID.ToString("D6")}] send fail. Please check log. ");
                    _mgr.WriteFailResult(item, ex, _cUserID, DateTime.Now);
                }
            }

            var deletedMailCount = _mgr.DeleteExpiredMail(_cUserID, DateTime.Now);
            WriteMessage($"Expired mail Deleted. Total rows: {deletedMailCount}");


            Console.ForegroundColor = _defaultColor;
            WriteMessage("Mail Sender completed.");
        }

        /// <summary> 輸出文字 </summary>
        /// <param name="msg"></param>
        private static void WriteMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
