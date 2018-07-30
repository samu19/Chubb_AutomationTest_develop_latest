using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAutomation
{
    public class Helper
    {
        public static void redirectConsoleLog(string testName, string message)
        {
            string path = ConfigurationManager.AppSettings["testFolder"].ToString();
            FileStream fs = new FileStream(path + testName + "\\out.txt", FileMode.Create);
            // First, save the standard output.
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Console.WriteLine(DateTime.Now.ToString() + " : " + message);
            Console.SetOut(tmp);
            Console.WriteLine(message);
            sw.Close();

        }

        public static bool isAlertPresent()
        {
            try
            {
                Driver.Instance.SwitchTo().Alert();
                return true;
            } // try
            catch (Exception e)
            {
                return false;
            } // catch
        }

        public static void WriteToCSV(string category, string description, bool status, string moreInfo = null, string testId = null, string testScenario = null)
        {
            var csv = new StringBuilder();
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", DateTime.Now, testId, testScenario, category, description, (status ? "Pass" : "Fail"), moreInfo);
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());
        }


    }
}
