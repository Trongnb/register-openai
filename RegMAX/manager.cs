using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMAX
{
    internal class manager
    {
        public string proxy { get; set; }
        public string link { get; set; }
        public int count { get; set; }
        public manager(string proxy, string link)
        {
            this.proxy = proxy;
            this.link = link;
            this.count = 0;
        }
    }
}
