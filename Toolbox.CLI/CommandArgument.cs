using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    internal class CommandArgument
    {
        internal enum Type
        {
            Required,
            Optional,
            Variadic
        }
    }
}
