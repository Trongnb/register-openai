using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegMAX
{
    internal class BypassCLF
    {
        private static readonly Random random = new Random();

        public static void Click(UndetectChromeDriver driver)
        {
            try
            {
                for (int j = 0; j < 10; j++)
                {
                    driver.SwitchTo().DefaultContent();
                    driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("iframe[src*='challenges.cloudflare.com'][allow='cross-origin-isolated; fullscreen'][sandbox='allow-same-origin allow-scripts allow-popups']")));
                    Thread.Sleep(random.Next(4000,4500));
                    IWebElement element = driver.FindElement(By.CssSelector("#challenge-stage > div > label > input[type=checkbox]"));
                    Actions action = new Actions(driver);
                    Thread.Sleep(random.Next(1000,1200));

                    // Execute JavaScript to click on the element
             
                    action.Click(element).Perform();
                



                }
            }
            catch
            {
                Console.WriteLine("Không tồn tại CloudFlare");
            }
        }
    }
}
