using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using static Katpis.ConsoleUtils;

namespace Katpis
{
    static class CommandSubmit
    {
        private static Dictionary<string,string> lang = new Dictionary<string, string>{
            { ".c", "C" },
            { ".c++", "C++" },
            { ".cc", "C++" },
            { ".c#", "C#" },
            { ".cpp", "C++" },
            { ".cs", "C#" },
            { ".cxx", "C++" },
            { ".cbl", "COBOL" },
            { ".cob", "COBOL" },
            { ".cpy", "COBOL" },
            { ".fs", "F#" },
            { ".go", "Go" },
            { ".h", "C++" },
            { ".hs", "Haskell" },
            { ".java", "Java" },
            { ".js", "JavaScript" },
            { ".kt", "Kotlin" },
            { ".lisp", "Common Lisp" },
            { ".cl", "Common Lisp" },
            { ".m", "Objective-C" },
            { ".ml", "OCaml" },
            { ".pas", "Pascal" },
            { ".php", "PHP" },
            { ".pl", "Prolog" },
            { ".rb", "Ruby" },
            { ".rs", "Rust" },
            { ".scala", "Scala" },
        };
        public static async System.Threading.Tasks.Task RunSubmitAsync(string filename)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory; //TODO this is maybe not best pratice
            dir = dir.Substring(0,dir.LastIndexOf("\\App"));
            string configPath;
            try {
                configPath = Directory.GetFiles(dir).Where(x => x.EndsWith(".kattisrc")).First();
            } catch (InvalidOperationException) {
                Console.WriteLine("Config file not found, please download a .kattisrc file from a kattis site like https://open.kattis.com/download/kattisrc and place it in root directory of katpis: " + dir);
                return; //TODO Prompt user to login and auto download .kattisrc file
            }
            Dictionary<string, Dictionary<string, string>> configObject = ParseConfigFile(configPath);

            // Login

            Console.Write("Logging in...");
            string loginurl = configObject["kattis"]["loginurl"];
            string submissionurl = configObject["kattis"]["submissionurl"];
            string submissionsurl = configObject["kattis"]["submissionsurl"];
            var contentObject = new Dictionary<string,string>
            {
                { "user", configObject["user"]["username"]},
                { "script", "true"},
                { "token", configObject["user"]["token"]},
                { "User-Agent", "katpis-client" },
            };
            var content = new FormUrlEncodedContent(contentObject);
            HttpClient client = new HttpClient();
            var loginResponse = await client.PostAsync(loginurl, content);

            ClearCurrentConsoleLine();
            Console.WriteLine("Login sucessful.");
            // var sessionCookie = loginResponse.Headers.GetValues("Set-Cookie").Where(x => x.Contains("EduSiteCookie")).First().Split(";").First();

            var loginResponseString = await loginResponse.Content.ReadAsStringAsync();

            Console.Write("Submitting...");

            var form = new MultipartFormDataContent();
            string cd = System.Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(cd);
            string filePath = files.First(x => x.EndsWith(filename));
            var fileBytes = File.ReadAllBytes(filePath);
            form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "sub_file[]", filename);
            form.Add(new StringContent(configObject["user"]["username"]), "user");
            form.Add(new StringContent(configObject["user"]["token"]), "token");
            form.Add(new StringContent("katpis-client"), "User-Agent");
            form.Add(new StringContent("true"), "submit");
            form.Add(new StringContent("2"), "submit_ctr");
            form.Add(new StringContent(lang["." + filename.Split(".").Last()]), "language");
            form.Add(new StringContent(filename.Split(".").First()), "mainclass");
            form.Add(new StringContent(filename.Split(".").First().ToLower()), "problem");
            form.Add(new StringContent("true"), "script");

            HttpResponseMessage submitResponse = await client.PostAsync(submissionurl, form);
            string submitResponseString = await submitResponse.Content.ReadAsStringAsync();
            MatchCollection matchCollection = Regex.Matches(submitResponseString, @"Submission ID: (\d+)", RegexOptions.Multiline);
            if (matchCollection.Count <= 0) {
                Console.WriteLine("A submission id could not be found");
                return;
            }
            Match m = matchCollection[0];
            string submissionid = m.Groups[1].ToString();

            ClearCurrentConsoleLine();
            Console.WriteLine("Submission successful.");

            string afterSubmissionStatusUrl = $"{submissionsurl}/{submissionid}";
            Console.WriteLine("Submission ID: " + submissionid);
            Console.WriteLine(afterSubmissionStatusUrl);

            // Login again
            client = new HttpClient();

            loginResponse = await client.PostAsync(loginurl, content);
            loginResponseString = await loginResponse.Content.ReadAsStringAsync();

            // Status
            Console.WriteLine("Getting status...");
            contentObject = new Dictionary<string,string>
            {
                { "user", configObject["user"]["username"]},
                { "script", "true"},
                { "token", configObject["user"]["token"]},
                { "User-Agent", "katpis-client" },
            };
            content = new FormUrlEncodedContent(contentObject);

            int statusIdTracker = 0;
            int requestStatusCounter = 0;
            while (IsRunningStatus(statusIdTracker)) {
                string statusurl = $"{submissionsurl}/{submissionid}?json";
                HttpResponseMessage statusResponse = await client.PostAsync(statusurl, content);
                string statusResponseString = await statusResponse.Content.ReadAsStringAsync();
                JsonValue status = JsonObject.Parse(statusResponseString);
                int statusId = (int)status["status_id"];

                matchCollection = Regex.Matches(statusResponseString, @"Test case 1\\/(\d+): ");
                bool numOfTestcasesIsKnown = matchCollection.Count > 0;
                int numOfTestcases = 1; // 1 to avoid div by 0 exception

                ClearCurrentConsoleLine();
                Console.Write(
                    "Status: " +
                    GetMessageFromStatusID(statusId)
                );

                if(!numOfTestcasesIsKnown) Console.Write(LoadingDots(requestStatusCounter));

                if (numOfTestcasesIsKnown) {
                    numOfTestcases = int.Parse(matchCollection[0].Groups[1].Value);

                    string progressBar = "[";
                    double acceptedFraction = status["testcase_index"] / (double)numOfTestcases;
                    int progressBarLength = 25;
                    
                    // create boxes in progessbar
                    bool hasPrintedOneErrorBox = false;
                    for (int i = 0; i < progressBarLength; i++) {
                        var currentFraction = i / (double)progressBarLength;
                        if (currentFraction < acceptedFraction) {
                            progressBar += "■".Green();
                        } else {
                            if(IsErrorStatus(statusId) && !hasPrintedOneErrorBox){
                                progressBar += "■".Red();
                                hasPrintedOneErrorBox = true;
                            } else {
                                progressBar += ".";
                            }
                        }
                    }
                    
                    progressBar += "]";

                    Console.Write(
                        " " +
                        progressBar +
                        " case " +
                        status["testcase_index"].ToString().Bold() +
                        " of " +
                        numOfTestcases.ToString().Bold()
                    );
                }

                requestStatusCounter++;
                statusIdTracker = statusId;
                Thread.Sleep(500);
            }
        }

        private static string LoadingDots(int requestStatusCounter)
        {
        if (requestStatusCounter % 3 == 2){
            return "...";
        } else if(requestStatusCounter % 3 == 1) {
            return "..";
        } else {
            return ".";
        }
        }

        private static bool IsRunningStatus(int statusid)
        {
            return  statusid == 0 ||
                    statusid == 1 ||
                    statusid == 2 ||
                    statusid == 3 ||
                    statusid == 4 ||
                    statusid == 5 ||
                    statusid == 16;
        }

        private static bool IsErrorStatus(int statusid)
        {
            return  statusid != 0 &&
                    statusid != 1 &&
                    statusid != 2 &&
                    statusid != 3 &&
                    statusid != 4 &&
                    statusid != 5;
        }

        private static string GetMessageFromStatusID(int statusid)
        {
            switch (statusid)
            {
                case 0:
                    return "New".Cyan(); //invalid value
                case 1:
                    return "New".Cyan();
                case 2:
                    return "New".Cyan();
                case 3:
                    return "Compiling".Cyan();
                case 4:
                    return "Waiting for run".Cyan();
                case 5:
                    return "Running".Cyan();
                case 6:
                    return "Judge Error".Red();
                case 7:
                    return "Submission Error".Red();
                case 8:
                    return "Compile Error".Red();
                case 9:
                    return "Runtime Error".Red();
                case 10:
                    return "Memory Limit Exceeded".Red();
                case 11:
                    return "Output Limit Exceeded".Red();
                case 12:
                    return "Time Limit Exceeded".Red();
                case 13:
                    return "Illigal Function".Red();
                case 14:
                    return "Wrong Answer".Red();
                case 16:
                    return "Accepted".Green();
                default:
                    return "Unknown status id".Red();
            }

        }

        private static Dictionary<string, Dictionary<string, string>> ParseConfigFile(string configPath)
        {
            var sr = new StreamReader(configPath);
            var configObject = new Dictionary<string, Dictionary<string, string>>();
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                MatchCollection mc = Regex.Matches(line, @"\[(.+)\]", RegexOptions.Multiline);
                if (mc.Count >= 1)
                {
                    Match m = mc[0];
                    string sectionName = m.Groups[1].ToString();
                    var section = new Dictionary<string, string>();
                    configObject.Add(sectionName, section);

                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        mc = Regex.Matches(line, @"^(.+): (.+)$", RegexOptions.Multiline);
                        if (mc.Count <= 0) break;
                        m = mc[0];
                        string key = m.Groups[1].ToString();
                        string val = m.Groups[2].ToString();
                        configObject[sectionName].Add(key, val);
                    }
                }
            }

            return configObject;
        }

    }
}