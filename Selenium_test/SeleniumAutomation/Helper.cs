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
            FileStream fs = new FileStream(path + testName + "\\out.txt", FileMode.Append);
            // First, save the standard output.
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Console.WriteLine(DateTime.Now.ToString() + " : " + message);
            Console.SetOut(tmp);
            Console.WriteLine(message);
            sw.Close();

        }
    }
}
