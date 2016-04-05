using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    /// <summary>
    /// CommandManager let you build an interactive CLI app.
    /// It's strongly inspired by <a href="https://github.com/dthree/vorpal">Vorpal</a>.
    /// </summary>
    public class CommandManager
    {
        private ISession _session;

        internal string InterpreterDelimiter { get; private set; }
        internal Action InitAction { get; private set; }
        internal CommandCollection Commands { get; } = new CommandCollection();

        public Command Command(string command)
        {
            return new Command(command);
        }

        public Mode Mode(string mode)
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

        public void Exec(string input)
        {
            var nameRegex = new Regex(@"(?<command>(?:[\w\d]+))");
            var argumentsRegex = new Regex(@"");
            var optionsRegex = new Regex(@"(?<option>-+[\w\d]+)\s+(?:(?<value>[\S\d]+))*");

            var name = nameRegex.Match(input).Value;
            var args = argumentsRegex.Matches(input)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToArray();
            var options = optionsRegex.Matches(input)
                .Cast<Match>()
                .ToDictionary(x => x.Groups["option"], y => y.Groups["value"]);
        }
    }
}
