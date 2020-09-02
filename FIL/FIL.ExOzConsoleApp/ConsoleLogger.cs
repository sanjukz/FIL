using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FIL.Logging.Enums;
using Microsoft.Extensions.Logging;

namespace FIL.ExOzConsoleApp
{
    public interface IConsoleLogger
    {
        void Log(string message);
        string Update(int i, int count, string line);
        void StartMsg(string category);
        void FinishMsg(int count, string category);
    }

    public class ConsoleLogger : IConsoleLogger
    {
        private readonly FIL.Logging.ILogger _logger;

        public ConsoleLogger(FIL.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        { 
            if(message.Contains("Error") || message.Contains("Exception"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
                _logger.Log(LogCategory.Error, message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        public string Update(int i,int count, string line)
        {
            string backup = new string('\b', line.Length);
            Console.Write(backup);
            line = string.Format("Downloading: {0} out of {1} completed.", i, count);
            Console.Write(line);
            return line;
        }

        public void FinishMsg(int count, string category)
        {
            Console.WriteLine();
            Console.WriteLine(count + " " + category + " Updated." );
        }

        public void StartMsg(string category)
        {
            Console.WriteLine();
            Console.WriteLine("--Updating " + category + "--");
        }
    }
}
