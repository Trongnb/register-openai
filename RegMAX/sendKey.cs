using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegMAX
{
    public class sendKey
    {
        private static Random random=new Random();

        public static void sendNoEnter(UndetectChromeDriver driver, string css, string inputText)
        {
            IWebElement inputElement = driver.FindElement(By.CssSelector(css));
            Thread.Sleep(random.Next(200, 500));
            foreach (char c in inputText)
            {
                // Sử dụng SendKeys để gửi từng ký tự
                inputElement.SendKeys(c.ToString());
                Thread.Sleep(random.Next(70, 150));
            }
          
        }

        public static void sendHaveEnter(UndetectChromeDriver driver,string css,string inputText)
        {
            sendNoEnter(driver, css, inputText);
            Thread.Sleep(random.Next(60, 100));
            driver.FindElement(By.CssSelector("#root > div.route-container > div > div.onb-page.onb-uinfo > form > button")).Click();
        }


        public static bool sendKeyLastName(UndetectChromeDriver driver,string key)
        {
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    driver.FindElement(By.Name("LastName")).SendKeys(key); 
                    return true;
                }
                catch
                {
                    Thread.Sleep(1500);
                }
            }
            return false; 
        }
        public static bool sendKeyFirstName(UndetectChromeDriver driver, string key)
        {
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    driver.FindElement(By.Name("FirstName")).SendKeys(key);
                    return true;
                }
                catch
                {
                    Thread.Sleep(1500);
                }
            }
            return false;
        }
        public static bool sendKeyDay(UndetectChromeDriver driver, string key)
        {
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    var selectDay1 = new SelectElement(driver.FindElement(By.Name("BirthDay")));
                    selectDay1.SelectByValue(key);
                    return true;
                }
                catch
                {
                    Thread.Sleep(1500);
                }
            }
            return false;
        }
    }
}
