using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static App.CommandSubmit;

namespace App.Tests
{
    public class CommandSubmitTests
    {
        private readonly ITestOutputHelper output;

        public CommandSubmitTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Test1()
        {
            // dotnet test --logger:"console;verbosity=detailed"

            int printStatusCallCounter = 0;
            int statusId = 12;
            bool numOfTestcasesIsKnown = true;
            int numOfTestcases = 3;
            int testCaseIndex = 2;

            string statusMessage = CommandSubmit.PrintStatus(
                printStatusCallCounter,
                statusId,
                numOfTestcasesIsKnown,
                numOfTestcases,
                testCaseIndex
            );

            output.WriteLine(statusMessage);
            
        }
    }
}
