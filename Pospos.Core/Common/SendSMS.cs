using System;
using System.Collections.Generic;

namespace Pospos.Core.Common
{
    public class SendSMSRequest
    {
        public SendSMSRequest()
        {
            MessageList = new MessageList();

            MessageList.GSMList = new List<GSMListSingle>();
            MessageList.ContentList = new List<ContentListSingle>();
        }
        public string SessionID { get; set; } = ""; //(Bu kısım boş olmalı)
        public string Operator { get; set; } = "2"; //(Turkcell(1),Superonline(4),Avea(2),Vodefone(3) hangi operatör ile sms gönderiliyor ise operatör isimlerinin yanındaki değer yazılmalı)
        public string GroupID { get; set; } = "0";
        public string Orginator { get; set; } = "PETROLOFISI"; //Mesaj başlığı
        public string ShortNumber { get; set; } = "9990";
        public string Isunicode { get; set; } = "1"; //Türkçe karakter 1 / İngilizce karakter 0
        public string SendDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string DeleteDate { get; set; } = "";
        public string iysBrandCode { get; set; } = "123456"; //(IYS den alınan BrandCode)
        public int RecipientTypes { get; set; } = 1; //(BIREYSEL ise “0”, TACİR ise “1” Set edilecek)
        public int MessageTypeIys { get; set; } = 1; //Reklam ise “0”, Bilgilendirme ise   “1” değerleri set edilecek
        public MessageList MessageList { get; set; }
    }

    public class MessageList
    {
        public List<GSMListSingle> GSMList { get; set; }
        public List<ContentListSingle> ContentList { get; set; }
    }

    public class GSMListSingle
    {
        public string value { get; set; }
    }

    public class ContentListSingle
    {
        public string value { get; set; }
    }
}
