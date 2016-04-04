using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandManager = new Toolbox.CLI.CommandManager();

            commandManager
                .Command("sdes")
                .Description("Descrypt a file with the given key.")
                .Option("e,encrypt")
                .Option("d,decrypt")
                .Option("k,key")
                .Option("i,in")
                .Option("o,out")
                .Action((callback, options) =>
                {
                    if (options.ContainsKey("decrypt") && !options.ContainsKey("encrypt"))
                    {
                        Program.Decrypt();
                    }
                    else if (options.ContainsKey("encrypt") && !options.ContainsKey("decrypt"))
                    {
                        Program.Encrypt();
                    }
                    else
                    {
                        commandManager.Error("Please use --decrypt OR --encrypt option.");
                    }
                });

            commandManager
                .Init(() =>
                {
                    commandManager.Log("Started interactive Cryptography CLI.");
                })
                .Delimiter("crypto:")
                .Show();
        }

        static void Decrypt()
        {

        }

        static void Encrypt()
        {

        }
    }
}
