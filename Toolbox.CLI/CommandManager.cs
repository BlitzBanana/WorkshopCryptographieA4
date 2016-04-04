using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    public class CommandManager
    {
        private ISession _session;

        internal string InterpreterDelimiter { get; private set; }
        internal Action InitAction { get; private set; }
        internal List<CommandBase> Commands { get; } = new List<CommandBase>();

        public CommandBase Command(string command)
        {
            return new Command(command);
        }

        public CommandBase Mode(string mode)
        {
            return new Mode(mode);
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine("ERROR: " + message);
        }

        public CommandManager Delimiter(string delimiter)
        {
            this.InterpreterDelimiter = delimiter;
            return this;
        }

        public CommandManager Init(Action action)
        {
            this.InitAction = action;
            return this;
        }

        public void Show()
        {
            this._session = new ConsoleSession(this);
            this._session.Start();
        }
    }
}
