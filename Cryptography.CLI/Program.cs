using System;
using System.Collections.Generic;
using System.IO;
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
                    if (!options.ContainsKey("decrypt") && !options.ContainsKey("encrypt"))
                    {
                        commandManager.Error("Please use --decrypt OR --encrypt option.");
                        return;
                    }
                    var key = options["key"];
                    var input = ReadTextFile(options["in"]);
                    var outputPath = options["out"];
                    var result = string.Empty;
                    if (options.ContainsKey("decrypt") && !options.ContainsKey("encrypt"))
                        result = Program.Decrypt(input, key);
                    else if (options.ContainsKey("encrypt") && !options.ContainsKey("decrypt"))
                        result = Program.Encrypt(input, key);
                    Program.SaveTextFile(outputPath, result);
                });

            commandManager
                .Init(() =>
                {
                    commandManager.Log("Started interactive Cryptography CLI.");
                })
                .Delimiter("$Crypto:")
                .Show();
        }

        static string Decrypt(string text, string key)
        {
            return string.Empty;
        }

        static string Encrypt(string text, string key)
        {
            return string.Empty;
        }

        static string ReadTextFile(string path)
        {
            var file = new StreamReader(path);
            var content = file.ReadToEnd();
            file.Close();
            return content;
        }

        static void SaveTextFile(string path, string content)
        {
            File.Create(path);
            TextWriter tw = new StreamWriter(path);
            tw.WriteLine(content);
            tw.Close();
        }
    }
}
