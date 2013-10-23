using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FamilyPortal.Common
{
    public class Log
    {
        /// <summary>
        /// Add Log
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="fileName">method name</param>
        public static void WriteLog(string message, string methodName)
        {
            string directoryPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\logs";
            methodName = methodName + ".log";
            string path = directoryPath + "\\" + methodName;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine("{0} {1}", System.DateTime.Now, message);
            sw.Close();
        }
    }
}
