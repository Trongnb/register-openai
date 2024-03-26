using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Threading;
using System.Net.Http;
using HttpMethod = System.Net.Http.HttpMethod;

namespace RegMAX
{
    internal class SessData
    {


        private static string checkLimit(string beaer, string org)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/dashboard/billing/subscription");
                request.Headers.Add("authority", "api.openai.com");
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("accept-language", "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5");
                request.Headers.Add("authorization", $"Bearer {beaer}");
                request.Headers.Add("openai-organization", org);
                request.Headers.Add("origin", "https://platform.openai.com");
                request.Headers.Add("priority", "u=1, i");
                request.Headers.Add("referer", "https://platform.openai.com/");
                request.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"119\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"");
                request.Headers.Add("sec-ch-ua-mobile", "?0");
                request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-site", "same-site");
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                JObject obj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                int dola = Convert.ToInt32(obj["hard_limit_usd"]);
                if (dola > 1)
                {
                    return beaer + "|" + org;
                }
                else { return ""; }
            }
            catch
            {
                return "";
            }
        }

        public static string GetLocalStorageData(UndetectChromeDriver driver)
        {
            int attemp = 0;
            while(attemp<10)
            {
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                driver.SwitchTo().DefaultContent();
                string localStorageData = (string)jsExecutor.ExecuteScript("return window.localStorage.getItem('@@auth0spajs@@::DRivsnm2Mu42T3KOpqdtwB3NYviHYzwD::https://api.openai.com/v1::openid profile email offline_access');");
                // Parse the JSON string
                JObject jsonObject = JObject.Parse(localStorageData);

                // Access the 'access_token' property
                string accessToken = jsonObject["body"]["access_token"].ToString();
                HttpRequest http = new HttpRequest();
                // string sess= getsess(accessToken);
                string sesss = http.Get($"https://dbbx3fk15e.genhosting.net/get_info.php?token_acc=Bearer {accessToken}").ToString();

                if (sesss != "" )
                {

                   return checkLimit(sesss.Split('|')[0], sesss.Split('|')[1]);
                   

                }
                Console.WriteLine("Không tìm thấy sau " + attemp * 5 + " giây");
                Thread.Sleep(5000);
                attemp++;
            }
            return "";
        }
    }
}
