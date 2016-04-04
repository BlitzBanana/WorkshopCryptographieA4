using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    class Command : CommandBase
    {
        public Command(string command)
        {
            this.ParseCommand(command);
        }

        private void ParseCommand(string command)
        {

        }
    }
}
