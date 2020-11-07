using System.Diagnostics;
using System.IO;
using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1) {
                if (args[0].Equals("test")) {
                    Console.WriteLine("Testing");
                    var cd = System.Environment.CurrentDirectory;
                    Console.WriteLine(cd);



                    // (later) Download test files
                    // 1. Reads .in file(s) from tests



                    // Pipe infile into program
                        // Determine file extension
                        // Map to runner
                    // Get result
                    // Read output of program
                    // Compare to .ans file(s)
                    // Output differences or accepted
                }
            }
        }
    }
}
