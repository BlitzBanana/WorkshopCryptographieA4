using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    internal class CommandCollection : List<CommandBase>
    {
        internal CommandBase Find(string name)
        {
            return this.First(x => x.Name == name);
        }

        internal Command FindCommand(string name)
        {
            return this.OfType<Command>().First(x => x.Name == name);
        }

        internal Mode FindMode(string name)
        {
            return this.OfType<Mode>().First(x => x.Name == name);
        }
    }
}
