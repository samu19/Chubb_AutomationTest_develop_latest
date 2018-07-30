using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            catch (Exception e)
            {
                try
                {
                    string path = ConfigurationManager.AppSettings["testFolder"].ToString();
                    
                    SaveScreenshot(path, "Error", TestName);
                }
                catch (Exception) { }

                throw;
            }
        }

        public void SaveScreenshot(string path, string prefix, string TestName)
            {
            var screenshot = ((ITakesScreenshot)Driver.Instance).GetScreenshot();
            string imageFolder = Path.GetFullPath(path + TestName + "\\Screenshots");
            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            var filePath = imageFolder + "\\" + prefix + "_" + TestName + "_" + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".jpg";

            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Jpeg);
        }
    }
}
