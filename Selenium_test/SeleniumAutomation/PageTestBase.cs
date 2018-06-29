using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAutomation
{
    public class PageTestBase
    {


        protected void UITest(string TestName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver.Instance).GetScreenshot();

                    string imageFolder = Path.GetFullPath(@"C:\Users\edmund.toh\Source\Repos\Chubb_AutomationTest_develop\Selenium_test\Screenshots");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    var filePath = imageFolder + "\\" + TestName + "_" + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".jpg";

                    screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Jpeg);
                }
                catch (Exception) { }

                throw;
            }
        }
    }
}
