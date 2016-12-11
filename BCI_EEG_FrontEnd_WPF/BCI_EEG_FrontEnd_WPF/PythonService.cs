using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI_EEG_FrontEnd_WPF
{
    public class PythonService
    {

        string pythonlocation = @"C:\Users\barse\AppData\Local\Programs\Python\Python35\python.exe";

        public void RunCommand(string command, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            start.FileName = pythonlocation;
            start.Arguments = $"{command} {args}";

            Console.WriteLine(start.Arguments);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            Console.WriteLine("Python starting");

            using (Process process = Process.Start(start))
            {

                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }

        }
    }
}
