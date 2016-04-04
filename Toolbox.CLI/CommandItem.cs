using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    public abstract class CommandBase
    {
        private string _description;
        private List<string> _aliases = new List<string>();
        private List<string> _options = new List<string>();
        private List<Action<Action, Dictionary<string, string>>> _actions = new List<Action<Action, Dictionary<string, string>>>();

        public CommandBase Description(string description)
        {
            this._description = description;
            return this;
        }

        public CommandBase Alias(string alias)
        {
            this._aliases.Add(alias);
            return this;
        }

        public CommandBase Option(string option)
        {
            this._options.Add(option);
            return this;
        }

        public CommandBase Action(Action<Action, Dictionary<string, string>> action)
        {
            this._actions.Add(action);
            return this;
        }
    }
}
