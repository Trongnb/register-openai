using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMAX
{
    internal class hotmail
    {
        public string mail { get; set; }
        public string pass { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string toString(int type,string localStorage="")

        {
            string DATA = type == 1 ? "@hotmail.com" : "@outlock.com";
            if (localStorage == "")
            {
                return mail + DATA + "|" + pass + $"|{fname} {lname}|" + $"|{day}/{month}/{year}";
            }
            else
            {
                return mail + DATA + "|" + pass +$"|{localStorage}|{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}";
            }    
        }

      

        public string to(int type)
        {
            string DATA = type == 1 ? "@hotmail.com" : "@outlock.com";
            return mail + DATA;
        }

        public string birthday()
        {
            string birth=day.ToString("D2")+month.ToString("D2")+year.ToString();
            return birth;
        }
    }
}
