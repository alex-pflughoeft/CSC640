using BLUE.ChocAn.Library.Commands;
using BLUE.ChocAn.Library.Database;
using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library
{
    public class TerminalView
    {
        #region Private Variables

        private string _readPrompt;
        private CommandPresenter _commandPresenter;

        #endregion

        #region Constructors

        public TerminalView(string readPrompt)
        {
            this._readPrompt = readPrompt;
            this.HistoricalQueue = new Queue<string>();
            this.IsInteractiveMode = false;
            this._commandPresenter = new CommandPresenter(this);
        }

        #endregion

        #region Public Properties

        public Queue<string> HistoricalQueue { get; private set; }
        public string TerminalName { get { return "ChocAn Terminal v1.0.0"; } }
        public bool IsInteractiveMode { get; private set; }

        #endregion

        #region Public Methods

        public void Run()
        {
            this._commandPresenter.reboot();

            while (true)
            {
                var consoleInput = this.ReadTermInput();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;

                try
                {
                    this.HistoricalQueue.Enqueue(consoleInput);
                    var command = new ConsoleCommand(consoleInput);
                    string result = this._commandPresenter.Execute(command);
                    this.WriteConsoleOutput(result);
                }
                catch (Exception e)
                {
                    this.WriteConsoleOutput(e.Message);
                }
            }
        }

        public void PrintHistory()
        {
            int initialCount = this.HistoricalQueue.Count;

            for (int i = 0; i < initialCount; i++)
            {
                string command = this.HistoricalQueue.Dequeue();

                Console.WriteLine(command.ToString());

                this.HistoricalQueue.Enqueue(command);
            }
        }

        public void WriteConsoleOutput(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }

        public void UpdateTerminalPrompt(string terminalPrompt)
        {
            this._readPrompt = terminalPrompt;
        }

        public void ClearHistoricalQueue()
        {
            this.HistoricalQueue.Clear();
        }

        public string TerminalInfo()
        {
            var result = string.Empty;

            result += "\n**********************************************************************";
            result += "\n*               Chocoholics Anonymous Terminal Program               *";
            result += "\n*                               v1.0.0                               *";
            result += "\n**********************************************************************";

            return result + "\n";
        }

        public void EnableInteractiveMode(int secondsUntilExit = -1)
        {
            this.IsInteractiveMode = true;

            if (secondsUntilExit > 0)
            {
                this.DisableInteractiveMode(secondsUntilExit);
            }
        }

        public void DisableInteractiveMode(int secondsUntilExit = -1)
        {
            if (secondsUntilExit == -1)
            {
                this.IsInteractiveMode = false;
            }
            else
            {
                var delayInterval = TimeSpan.FromMilliseconds(secondsUntilExit * 1000);
                var runningTask = this.DoActionAfter(delayInterval, () => this.DisableInteractiveMode());
            }
        }

        #endregion

        #region Private Methods

        private Task DoActionAfter(TimeSpan delay, Action action)
        {
            return Task.Delay(delay).ContinueWith(_ => action());
        }

        private string ReadTermInput(string promptMessage = "")
        {
            Console.Write(this._readPrompt + promptMessage);
            return Console.ReadLine();
        }

        #endregion
    }
}