using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace RegMAX
{
    internal class createInfo
    {
        public static hotmail getInfo(string email = "")
        {
            hotmail hotmail = new hotmail();
            if (!email.Contains("@"))
            {
                string ho = HoViet[rd.Next(0, HoViet.Length - 1)];
                string ten = nameViet[rd.Next(0, nameViet.Length - 1)];
                string mail = ho + Numbe() + ten + rd.Next(113419, 99343499).ToString();
                hotmail.mail = LocDau(mail.Replace(" ","")).ToLower();
                hotmail.fname = $"{LocDau($"{ho}{ten}".Replace(" ", "")).ToLower()}";
            }
            else
            {
                hotmail.mail = email;
            }
           
            while (true)
            {
                using (HttpRequest http = new HttpRequest())
                {
                    string get = http.Get($"https://tpteams.com/hotmail/getAccount.php").ToString();
                    JObject obj = JObject.Parse(get);

                    if ((bool)obj["success"])
                    {
                      
                        hotmail.mail = obj["data"]["taikhoan"].ToString().Trim();
                        hotmail.pass = obj["data"]["matkhau"].ToString().Trim();
                        break;
                    }
                    else
                        Console.WriteLine("Không tìm thấy tài khoản => Wait 50");
                    Thread.Sleep(50000);
                }

            }
          //  hotmail.pass = pass();
        
            hotmail.lname = lname();
            hotmail.day = day();
            hotmail.month = month();
            hotmail.year = year();
            return hotmail;
        }
        public static string Numbe()
        {
            int leng = 4;
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < leng--)
            {
                res.Append("abcdefghijklmnopqrstuvwxyz"[rnd.Next("abcdefghijklmnopqrstuvwxyz".Length)]);
            }
            return res.ToString();
        }
        public static string LocDau(string str)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            return str;
        }
        private static readonly string[] VietNamChar = new string[]
       {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõððôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
       };
        private static string[] nameViet = File.ReadAllLines("name\\VN\\firstname_male.txt");

        // Token: 0x04000039 RID: 57
        private static string[] HoViet = File.ReadAllLines("name\\VN\\lastname.txt");

        // Token: 0x0400003A RID: 58
        private static string[] nameUs = File.ReadAllLines("name\\US\\firstname_male.txt");

        // Token: 0x0400003B RID: 59
        private static string[] HoUs = File.ReadAllLines("name\\US\\firstname_male.txt");
        static Random rd = new Random();
        public static string pass()
        {
            return CreateRadom(10);
        }
        public static string fname()
        {
            return CreateRadom(5);
        }
        public static string lname()
        {
            return CreateRadom(5);
        }
        static Random random = new Random();
        public static int day()
        {
            return random.Next(11, 30);
        }
        public static int month()
        {
            return random.Next(1,11);
        }
        public static int year()
        {
            return random.Next(1980, 2000);
        }
        public static string CreateRadom(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ12345678901234567890";
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[rd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
