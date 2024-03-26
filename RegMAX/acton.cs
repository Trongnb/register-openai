using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegMAX
{
    internal class acton
    {
        GPMLoginAPI api { get; set; }
        UndetectChromeDriver driver { get; set; }
        public acton(GPMLoginAPI api)
        {
            this.api = api;
        }
        public JObject getLst(string names)
        {
            List<JObject> profiles = api.GetProfiles();
            if (profiles != null)
            {
                foreach (JObject profile in profiles)
                {
                    string name = Convert.ToString(profile["name"]);
                    string id = Convert.ToString(profile["id"]);
                    if (name.Contains(names))
                    {
                        return profile;
                    }
                }
            }
            return null;
        }

    

        public UndetectChromeDriver openProfile(int attemp,string createdProfileId,int thread,int x,int y,int width = 350, int height = 700,int limit = 400)
        {
            // Tính số lượng cửa sổ tối đa trên mỗi hàng dựa vào chiều rộng màn hình.
            int windowsPerRow = 1920 / 300;

            // Tính vị trí x và y dựa vào số thứ tự của luồng.
            // Chia 'attempt' cho 'windowsPerRow' sẽ cho vị trí hàng, và lấy phần dư sẽ cho vị trí cột.
            int positionX = ((thread % windowsPerRow) * 800) + (90 * attemp);
            int positionY = ((thread / windowsPerRow) * 500)+500;
            JObject startedResult = api.Start(createdProfileId);
            if (startedResult != null)
            {
                string browserLocation = Convert.ToString(startedResult["browser_location"]);
                string seleniumRemoteDebugAddress = Convert.ToString(startedResult["selenium_remote_debug_address"]);
                string gpmDriverPath = Convert.ToString(startedResult["selenium_driver_location"]);

                // Init selenium
                FileInfo gpmDriverFileInfo = new FileInfo(gpmDriverPath);

                ChromeDriverService service = ChromeDriverService.CreateDefaultService(gpmDriverFileInfo.DirectoryName, gpmDriverFileInfo.Name);
                service.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();
                options.BinaryLocation = browserLocation;
                options.DebuggerAddress = seleniumRemoteDebugAddress;
                driver = new UndetectChromeDriver(service, options);
                driver.Manage().Window.Position = new Point(positionX, positionY);
                driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(150);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(150);
                driver.Manage().Window.Size = new Size(width, height);
            }
            return driver;
        }
        public void close()
        {
            try { driver.Close(); } catch { }
            try { driver.Quit(); }catch { }
        }
        public void delete(string profile)
        {
            api.Delete(profile);
        }
    }
}
