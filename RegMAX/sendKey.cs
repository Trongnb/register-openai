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
            foreach (char c in inputText)
            {
                // Sử dụng SendKeys để gửi từng ký tự
                inputElement.SendKeys(c.ToString());
                Thread.Sleep(random.Next(20, 60));
            }
          
        }

        public static void sendHaveEnter(UndetectChromeDriver driver,string css,string inputText)
        {
            IWebElement inputElement = driver.FindElement(By.CssSelector(css));
            foreach (char c in inputText)
            {
                // Sử dụng SendKeys để gửi từng ký tự
                inputElement.SendKeys(c.ToString());
                Thread.Sleep(random.Next(20,60));
            }
            inputElement.SendKeys(Keys.Enter);
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
