using BLUE.ChocAn.Library;
using System;

namespace BLUE.ChocAn
{
    public class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal("ChocAnon> ", "BLUE.ChocAn.Library.Commands");

            Console.Title = terminal.TerminalName;

            terminal.Run();
        }
    }
}