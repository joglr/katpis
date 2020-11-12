using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Katpis
{
    static class CommandTemplate
    {
        
        public static void RunTemplate(string filename)
        {
            string pattern = @".java$";
            string input = filename;
            if (Regex.Matches(input, pattern).Count() == 1) {
                string className = filename.Split(".").First();
                string javaPattern = GetTemplate(className);
                File.WriteAllText(filename, javaPattern);
            } else {
                Console.WriteLine($"filename: {filename} doesn't end with any of the supported file extensions: [ .java ]");
            }
        }

        public static string GetTemplate(string className)
        {
            return $@"import java.util.Scanner;
import java.io.*;
import java.util.StringTokenizer;

public class {className} {{
    public static void main(String[] args) throws IOException {{
        var reader = new BufferedReader(new InputStreamReader(System.in));
        StringTokenizer st = new StringTokenizer(reader.readLine());
        int N = Integer.parseInt(st.nextToken());
        for (int i = 0; i < N; i++) {{
            String[] line = reader.readLine().split("" "");
        }}
        System.out.println(""Output"");
    }}
}}";
        }

    }
}