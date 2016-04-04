﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CLI
{
    internal interface ISession
    {
        void Start();

        void Write(string message);

        void WriteLine(string message);

        string ReadLine();
    }
}
