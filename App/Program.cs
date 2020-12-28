using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;
using System.Json;
using System.Net.Http;
using System.Threading;
using static App.ConsoleUtils;

namespace App
{
  class Program
    {
        public const string Version = "0.3.1";

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine(GetHelpPrintout());
            } else if (args.Length >= 1) {
                switch (args[0]){
                    case "submit":
                        if (args.Length == 2){
                            await CommandSubmit.RunSubmitAsync(args[1]);
                        } else {
                            Console.WriteLine(
                                "Usage: " +
                                "katpis submit <filename>".Bold().Blue() +
                                "\n" +
                                "Example: " +
                                "$ katpis submit hello.java"
                            );
                        }
                        break;
                    case "test":
                        CommandTest.runTest();
                        break;
                    case "fetch":
                        if (args.Length == 2){
                            CommandFetch.RunFetch(args[1]);
                        } else {
                            Console.WriteLine(
                                "Usage: " +
                                "katpis fetch <problem_id>".Bold().Blue() +
                                "\n" +
                                "Example: " +
                                "$ katpis fetch twostones"
                            );
                        }
                        break;
                    case "template":
                        if (args.Length == 2) {
                            CommandTemplate.RunTemplate(args[1]);
                        } else {
                            Console.WriteLine(
                                "Usage: " +
                                "katpis template <filename>".Bold().Blue() +
                                "\n" +
                                "Example: " +
                                "$ katpis template hello.java"
                            );
                        }
                        break;
                    case "dir":
                        GetProjectDir();
                        break;
                    default:
                        Console.WriteLine("Unknown first argument: " + args[0]);
                        break;
                }
            }
        }

        private static string GetHelpPrintout()
        {
            return @"
   A.-.A
 =[O . O]=
 o(___UU)".Cyan() +
      @"

" +
      "USAGE".White().Bold() +
      @"
" +
      @"
  katpis <command> <arguments>

" +
      "COMMANDS".White().Bold() +
      @"

  fetch    Fetch sample files if available

  test     Tests any matching .in and .ans files on
           any .java program in the current directory.

  submit   Submits a .java file to kattis with a matching
           name to a kattis problem shortname.

  template Generates a .java file with some boilerplate/
           template content for parsing, input and output.

" +
       "LEARN MORE".White().Bold() +

      @"

   Read the guide in the README.md

" +
       "FEEDBACK".Bold() +
      @"

   Open an issue on github at github.com/joglr/katpis/issues


";
        }

        private static void GetProjectDir()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            dir = dir.Substring(0,dir.LastIndexOf("\\App"));
            var configFilePath = Directory.GetFiles(dir).Where(x => x.EndsWith(".kattisrc")).First();
        }

    }
}
