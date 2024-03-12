using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RegMAX.captcha;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegMAX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //load setup
            clearchrome();
            Console.WriteLine("LOADDING SETUP...");
            settings();
            Console.WriteLine("START APP...");
            Task.Run(() =>
            {
                runTask();
            });
            Console.ReadLine();

        }
        static string setUpload = string.Empty;
        static List<manager> ListProxy = null;
        static Random rd = new Random();
        static void settings()
        {
            setUpload = File.ReadAllText("config\\config.json");
            var lines = File.ReadAllLines("config\\proxy.txt");
            ListProxy = new List<manager>();
            foreach (var line in lines)
            {
                var sp = line.Split('\t');
                if (sp.Length == 1)
                {
                    sp = line.Split('|');
                }
                ListProxy.Add(new manager(sp[0].Trim(), sp[1].Trim()));
            }
        }
        static void runTask()
        {
            for (int i = 0; i < Convert.ToInt32(ListProxy.Count); i++)
            {
                Task.Run(() =>
                {
                    createThread(i);
                });
                Thread.Sleep(500);
            }
        }

        static bool sendPass(UndetectChromeDriver driver, string pass)
        {
            for (int i = 0; i < 6; i++)
            {
                try { driver.FindElement(By.Id("PasswordInput")).SendKeys(pass); Thread.Sleep(1500); return true; } catch { Thread.Sleep(1000); }
            }
            return false;
        }

        static void createThread(int thead)
        {
            var setup = JObject.Parse(setUpload);
            manager man = ListProxy[thead];



            if (doiip == true)
            {
                Console.WriteLine($"RESET: {man.link}");
            chaylai:
                if (request.get(man.link) == null)
                {
                    Thread.Sleep(5000);

                }


                int delay = Convert.ToInt32(setup["DELAY"].ToString());
                Console.WriteLine($"WAIT CONNECT:{delay} -> {man.link}");
                Thread.Sleep(TimeSpan.FromSeconds(delay));


                string get = request.get($"http://192.168.1.4:6868/status?proxy={man.proxy}");
                JObject obj = JObject.Parse(get);
                if (!((bool)obj["status"]))
                {
                    goto chaylai;
                }
                doiip = false;
            }
             
         
            int thread = Convert.ToInt32(setup["THREAD"]);
            for (int i = 0; i < thread; i++)
            {
                Task.Run(() =>
                {
                    create(thead, i);
                });
                Thread.Sleep(2000);
            }
        }
        static void stopThread(int thead)
        {
            var setup = JObject.Parse(setUpload);
            ListProxy[thead].count += 1;
            int thread = Convert.ToInt32(setup["THREAD"]);
            if (ListProxy[thead].count == thread)
            {
                ListProxy[thead].count = 0;
                createThread(thead);
            }
        }
        static bool tryLogin(UndetectChromeDriver driver)
        {
            try { driver.FindElement(By.Name("MemberName")).SendKeys(" "); Thread.Sleep(2000); return false; } catch { return true; }
        }
        static bool tryYes(UndetectChromeDriver driver)
        {
            ///html/body/div[1]/div/div/div[2]/div/div[1]/div[3]/div/div[1]/div[5]/div/div/form/div[5]/div[1]/div[2]/input
            try { driver.FindElement(By.Id("idSIButton9")).Click(); Thread.Sleep(4000); return true; }
            catch
            {
                return false;
            }
        }
        static bool acceptSave(UndetectChromeDriver driver)
        {
            ///html/body/div[1]/div/div/div[2]/div/div[1]/div[3]/div/div[1]/div[5]/div/div/form/div[5]/div[1]/div[2]/input
            try { driver.FindElement(By.Id("acceptButton")).Click(); Thread.Sleep(4000); return true; }
            catch
            {
                return false;
            }
        }
        static bool tryPhone(UndetectChromeDriver driver)
        {
            ///html/body/div[1]/div/div/div[2]/div/div[1]/div[3]/div/div[1]/div[5]/div/div/form/div[5]/div[1]/div[2]/input
            try { driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[2]/div/div[1]/div[3]/div/div[1]/div[5]/div/div/form/div[5]/div[1]/div[2]/input")).SendKeys("Bị very phone"); Thread.Sleep(2000); return false; }
            catch
            {
                return true;
            }
        }
        static bool kiemtramail(UndetectChromeDriver driver)
        {
            int kiemtra = 60;
            while (kiemtra > 0)
            {
                kiemtra -= 1;
                if (driver.Url.Contains("inbox"))//
                    return true;
                if (driver.Url.Contains("/mail/"))//
                    return true;
                if (driver.Url.Contains("error.aspx"))//
                    return false;
                if (!tryPhone(driver))
                { return false; }
                if (!tryLogin(driver))
                { return false; }
                if (tryYes(driver))
                {
                    return true;
                }
                if (acceptSave(driver))
                {
                    return true;
                }
                Thread.Sleep(3000);
            }
            return false;
        }
        private static Random random = new Random();
        private static int ransleep()
        {
            return random.Next(1000, 2000);
        }

       static bool doiip = false;
        static void create(int thead, int attemp)
        {
            manager man = ListProxy[thead];
            var setup = JObject.Parse(setUpload);
            int typeUp = Convert.ToInt32(setup["TYPE"].ToString());
            hotmail mail = createInfo.getInfo();
            Console.WriteLine($"CREATE: {mail.toString(typeUp)}");
            UndetectChromeDriver driver = null;
            string id_profile = "";
            GPMLoginAPI api = null;
            acton sts = null;
            JObject ojb = null;
            api = new GPMLoginAPI(setup["APP_URL"].ToString());
            sts = new acton(api);
            try { ojb = sts.getLst(mail.mail); }
            catch
            {
                Console.WriteLine($"OPEN GPM");
                return;
            }
            if (ojb == null)
            {
                try
                {
                    ojb = api.Create(mail.mail, "All", $"{man.proxy}", true);
                }
                catch
                {
                    return;
                }
                Thread.Sleep(3000);
                if (ojb != null)
                {
                    //Tạo thành công
                    id_profile = ojb["profile_id"].ToString();
                }
            }
            else
            {
                try { id_profile = ojb["id"].ToString(); } catch { }
            }
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    Thread.Sleep(2000);
                    driver = sts.openProfile(attemp, id_profile, thead, 200, 300, Convert.ToInt32(setup["WIDTH"].ToString()), Convert.ToInt32(setup["HEIGHT"].ToString()));
                    break;
                }
                catch
                {
                    if (driver != null)
                    {
                        try { sts.close(); } catch { }
                    }
                    Thread.Sleep(4000);
                }
            }
            //countinue
            if (driver == null)
            {
                goto ketthuc;
            }
            //--- quy trinfh 

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            // save(setup["FILE_SUCCESS"].ToString() + $"_{DATA}", mail, typeUp);
            driver.Navigate().GoToUrl("https://platform.openai.com/signup");
      
            try
            {
                IWebElement inputElement = driver.FindElement(By.CssSelector("body > div > main > section > div > div > div > div.c09ffc6ab.ca83ba8b4 > form.cae724a40.c80c6bdcb.c664f6261 > button"));
                Thread.Sleep(ransleep());
                inputElement.Click();


                driver.FindElement(By.CssSelector("#i0116")).SendKeys(mail.mail + Keys.Enter);

                Thread.Sleep(ransleep());

                driver.FindElement(By.CssSelector("#i0118")).SendKeys(mail.pass + Keys.Enter);
                try
                {
                    driver.FindElement(By.CssSelector("#StartAction")).Click();
                    Console.WriteLine("Tài Khoản Bị Khóa");
                    goto ketthuc;
                }
                catch
                {

                }
                 inputElement = driver.FindElement(By.CssSelector("#declineButton"));
                Thread.Sleep(ransleep());
                inputElement.Click();

                inputElement = driver.FindElement(By.CssSelector("#idBtn_Accept"));
                Thread.Sleep(ransleep());
                inputElement.Click();


                //  sendonecharacter(driver,"input[placeholder='Last name']", name);
                inputElement = driver.FindElement(By.CssSelector("input[placeholder='Full name']"));
                Thread.Sleep(ransleep());
                // Xóa hết giá trị trong trường input
                inputElement.Clear();

                // Nhập giá trị mới bằng cách sử dụng SendKeys
                sendKey.sendNoEnter(driver, "input[placeholder='Full name']", mail.fname);
                Thread.Sleep(ransleep());

                sendKey.sendHaveEnter(driver, "input.text-input.text-input-lg.text-input-full[type='text'][placeholder='Birthday']", mail.birthday());
                Thread.Sleep(ransleep());
                string localstorage = SessData.GetLocalStorageData(driver);
                if (localstorage != "")
                {
                    save("openai.txt", mail, typeUp, localstorage);
                    Console.WriteLine($"SUCCESS: {mail.toString(typeUp)}");
                    // request.get(setup["OUPUT"].ToString() + mail.toString(typeUp));
                    //  request.UploadAsync(mail.to(typeUp),mail.pass);
                    //thành công
                }
                else
                {
                    doiip = true;
                }
            }
            catch { }
        ketthuc:
            if (driver != null)
            {
                try { sts.close(); } catch { }
            }
            stopThread(thead);
        }


        static void save(string nameFIle, hotmail mail, int type, string localStorage = "")
        {
            while (true)
            {
                try
                {
                    File.AppendAllText($"data\\{nameFIle}.txt", mail.toString(type, localStorage) + "\r\n");
                    break;
                }
                catch { Thread.Sleep(100); }
            }
        }
        static void clearchrome()
        {

            try
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("gpmdriver");
                foreach (var chromeDriverProcess in chromeDriverProcesses)
                {
                    try { chromeDriverProcess.Kill(); } catch { }
                }
            }
            catch { { } }
            try
            {
                Process[] chromed = Process.GetProcessesByName("chrome");
                foreach (var chrome in chromed)
                {
                    try { chrome.Kill(); } catch { }
                }
            }
            catch { }
        }
    }
}
