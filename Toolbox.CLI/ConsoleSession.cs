using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    class ConsoleSession : ISession
    {
        private readonly CommandManager _commandManager;
        private List<string> _history = new List<string>();

        public ConsoleSession(CommandManager commandManager)
        {
            if (commandManager == null)
                throw new ArgumentNullException($"{nameof(commandManager)} argument cannot be null.");
            this._commandManager = commandManager;
        }

        public void Start()
        {
            this._commandManager?.InitAction();
            while (true)
            {
                this.Write(this._commandManager.InterpreterDelimiter);
                var input = this.ReadLine();
                // TODO: detect Up & Down arrows to navigate into history
                this._history.Add(input);
                // TODO: Execute the command (make a CommandCollection class)
            }
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
