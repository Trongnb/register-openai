using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace RegMAX.captcha
{
    public class rockcaptcha
    {

        public string apiKey {  get; set; }
        public string idRequest { get; set; }
        public string Token { get; set; }
        public rockcaptcha(string apiKey) { this.apiKey = apiKey; }
        public bool getRequestId()
        {
            try
            {
                var http = new HttpRequest();
                var res = http.Get($"https://api.rockcaptcha.com/FunCaptchaTokenTask?apikey={this.apiKey}&sitekey=B7D8911C-5CC8-A9A3-35B0-554ACEE604DA&siteurl=https://signup.live.com/signup").ToString();
                var req = JObject.Parse(res);
                if (req["Message"].ToString() == "OK")
                {
                    this.idRequest = req["TaskId"].ToString();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
        public bool getRequestResult()
        {
            try
            {
                var http = new HttpRequest();
                var res = http.Get($"https://api.rockcaptcha.com/getresult?apikey={this.apiKey}&taskId={this.idRequest}").ToString();
                var req = JObject.Parse(res);
                if (req["Status"].ToString() == "SUCCESS")
                {
                    this.Token = req["Data"]["Token"].ToString();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
