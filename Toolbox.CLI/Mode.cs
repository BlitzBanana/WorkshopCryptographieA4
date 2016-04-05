using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    public class Mode : CommandBase
    {
        internal string InterpreterDelimiter;
        internal Action InitAction;
        internal Action<string> MainAction;

        public Mode(string mode)
        {
            this.Name = mode;
        }

        public Mode Delimiter(string delimiter)
        {
            this.InterpreterDelimiter = delimiter;
            return this;
        }

        public Mode Init(Action action)
        {
            this.InitAction = action;
            return this;
        }

        public Mode Action(Action<string> action)
        {
            if (this.MainAction != null)
                Console.WriteLine($"WARNING: You are redefining an action for the mode {this.Name}");
            this.MainAction = action;
            return this;
        }

        public new Mode Description(string description)
        {
            base.Description(description);
            return this;
        }
    }
}
