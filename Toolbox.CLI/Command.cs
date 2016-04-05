using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    public class Command : CommandBase
    {
        private List<Action<Action, Dictionary<string, string>>> _actions = new List<Action<Action, Dictionary<string, string>>>();
        private List<string> _options = new List<string>();

        public Command(string command)
        {
            this.ParseCommand(command);
        }

        private void ParseCommand(string command)
        {
            var nameRegex = new Regex(@"(?<command>(?:[\w\d ]+)(?:\w|\d))");
            var requiredRegex = new Regex(@"<(?<required>\w+)>");
            var optionalRegex = new Regex(@"\[(?<optional>\w+)\]");
            var variadicRegex = new Regex(@"\[(?<variadic>\w+)\.\.\.\]");

            var name = nameRegex.Match(command).Value;
            var required = requiredRegex.Matches(command)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToArray();
            var optional = optionalRegex.Matches(command)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToArray();
            var variadic = variadicRegex.Matches(command)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToArray();

            if (variadic.Length > 1)
                throw new ArgumentException("The command must contain only one or less variadic argument.");

            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Malformed command name.");

            this.Name = name;
        }

        public Command Option(string option)
        {
            this._options.Add(option);
            return this;
        }

        public Command Action(Action<Action, Dictionary<string, string>> action)
        {
            this._actions.Add(action);
            return this;
        }

        public new Command Description(string description)
        {
            base.Description(description);
            return this;
        }
    }
}
