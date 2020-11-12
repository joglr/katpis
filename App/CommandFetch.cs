using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace Katpis
{
    class CommandFetch
    {
        public static void RunFetch(string problemName)
        {

            Console.WriteLine("Fetching...");

            string cd = System.Environment.CurrentDirectory;
            string url = $"https://open.kattis.com/problems/{problemName}/file/statement/samples.zip";
            string zipPath = $@"{cd}\samples.zip";

            using (var client = new WebClient())
            {
                client.DownloadFile(url, zipPath);
            }
            try {
                ZipFile.ExtractToDirectory(zipPath, cd);
            } catch (IOException err) {
                Console.WriteLine(
                    $"Filename conflict with one or more of the sample files, '{err.Message.Split(@"\").Last()} Please remove existing files with this name from the current directory."
                    // Todo display all conflicts, and give option to override
                );
            }
            File.Delete(zipPath);

        }
    }
}