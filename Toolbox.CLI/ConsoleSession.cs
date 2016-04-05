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
        private readonly InputHistory _history = new InputHistory();
        private bool _running = false;
        private Mode _currenMode;

        public ConsoleSession(CommandManager commandManager)
        {
            if (commandManager == null)
                throw new ArgumentNullException($"{nameof(commandManager)} argument cannot be null.");
            this._commandManager = commandManager;
        }

        public void Start()
        {
            this._running = true;
            this._commandManager?.InitAction();
            while (this._running)
            {
                this.Write(this._commandManager.InterpreterDelimiter);

                if (this._currenMode != null)
                    this.Write(" " + this._currenMode.InterpreterDelimiter + " ");
                
                var key = Console.ReadKey();
                // TODO: detect Up & Down arrows to navigate into history
                var input = key.KeyChar + this.ReadLine();
                if (input == "exit")
                {
                    if (this._currenMode != null)
                    {
                        this._currenMode = null;
                    }
                    else
                    {
                        this._running = false;
                        break;
                    }
                }
                this._history.Push(this._currenMode, input);
                if (this._currenMode != null)
                {
                    this._currenMode?.MainAction(input);
                }
                else
                {
                    this._commandManager.Exec(input);
                }
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
