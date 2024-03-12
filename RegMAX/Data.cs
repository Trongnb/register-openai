using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMAX
{
    internal class Data
    {
        public string profile_name { get; set; }
        public string group_name { get; set; }
        public string raw_proxy { get; set; }
        public bool is_noise_canvas { get; set; }
        public bool is_noise_webgl { get; set; }
        public bool is_masked_font { get; set; }

        public int webrtc_mode {  get; set; }
    }
}
