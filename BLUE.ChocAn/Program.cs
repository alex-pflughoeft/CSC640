using BLUE.ChocAn.Library;
using System;

namespace BLUE.ChocAn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TerminalView terminal = new TerminalView("ChocAnon> ");

            Console.Title = terminal.TerminalName;

            terminal.Run();
        }
    }
}