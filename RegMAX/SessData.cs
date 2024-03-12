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

namespace RegMAX
{
    internal class SessData
    {

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
                    if (Convert.ToInt32(sesss.Split('|')[2]) > 0)
                    {
                        return $"{sesss.Split('|')[0]}|{sesss.Split('|')[1]}";

                    }
                    else
                    {
                        return "";
                    }

                }
                Console.WriteLine("Không tìm thấy sau " + attemp * 5 + " giây");
                Thread.Sleep(5000);
                attemp++;
            }
            return "";
        }
    }
}
