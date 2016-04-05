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

        public string Name { get; protected set; }

        public CommandBase Description(string description)
        {
            this._description = description;
            return this;
        }
    }
}
