using System.Net.Mail;
using System.IO;
using System;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace kat
{
  class Program
  {
    const ConsoleColor FOREGROUND_COLOR = White;
    const ConsoleColor BACKGROUND_COLOR = Black;
    const ConsoleColor HINT_COLOR = Cyan;
    const string COMMON_EMAIL_SUFFIXES_PATH = @"Resources\email-suffixes.csv";
    const string validCharsPattern = @"^\S$";

    public static string[] commonEmailSuffixes;

    public static Func<string, string> emailPredicter = text =>
    {
      if (text.Contains("@"))
      {
        var parts = text.Split("@");
        if (parts.Length.Equals(2))
        {
          var server = parts[1];
          if (server.Length > 0)
          {
            foreach (var item in commonEmailSuffixes)
            {
              if (item.StartsWith(server) && item.Contains(server) && !item.Equals(server))
              {
                return item.Substring(server.Length);
              }
            }
          }
        }
      }
      return null;
    };

    static async Task Main(string[] args)
    {


      Console.TreatControlCAsInput = true;
      ConsoleUtils.Colored(() => Console.Write("Loading..."));
      ConsoleUtils.ClearCurrentConsoleLine();
      commonEmailSuffixes = await File.ReadAllLinesAsync(COMMON_EMAIL_SUFFIXES_PATH);
      Console.Title = "Kat CLI";

      string email = null;

      ConsoleUtils.Colored(() =>
      {
        email = ConsoleUtils.AcceptInput(
          helpMessage: "Enter your email: ",
          errorMessage: "Invalid email, enter your email: ",
          checkValid: VerifyEmail,
          getPrediction: emailPredicter,
          acceptedCharsPattern: validCharsPattern
        );
      });
      Console.WriteLine();
      Console.WriteLine(email);
    }

    private static bool VerifyEmail(string email)
    {
      if (email.Length == 0) return false;
      try
      {
        var validsEmail = new MailAddress(email).Address;
      }
      catch (FormatException)
      {
        return false;
      }
      return true;
    }
  }
}
