using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace kat
{
  class Program
  {
    const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;
    const ConsoleColor HINT_COLOR = ConsoleColor.Cyan;
    const string COMMON_EMAIL_SUFFIXES_PATH = "email-suffixes.csv";
    const string INPUT_PREFIX = " > ";

    static readonly ConsoleColor prevForeground = Console.ForegroundColor;
    static readonly ConsoleColor prevBackground = Console.BackgroundColor;
    public static string[] commonEmailSuffixes;

    static Regex validCharsRegex = new Regex(@"^[A-Za-z@.+-]$");

    static async Task Main(string[] args)
    {
      ApplyColors();
      Console.Clear();
      Console.WriteLine("Loading...");
      commonEmailSuffixes = await LoadCommonEmailSuffices();
      Console.Clear();
      Console.Title = "Kat CLI";

      string email = null;

      do
      {
        email = AcceptInput(message: email == null
          ? "Please enter your email to continue"
          : "Invalid email, try again");
      } while (!VerifyEmail(email: email));

      Console.WriteLine(email);
      Console.ForegroundColor = prevForeground;
      Console.BackgroundColor = prevBackground;
    }

    private static async Task<string[]> LoadCommonEmailSuffices()
    {
      return await File.ReadAllLinesAsync(COMMON_EMAIL_SUFFIXES_PATH);
    }



    private static void ApplyColors(
      ConsoleColor foregroundColor = FOREGROUND_COLOR,
      ConsoleColor backgroundColor = BACKGROUND_COLOR
    )
    {
      Console.ForegroundColor = foregroundColor;
      Console.BackgroundColor = backgroundColor;
    }

    private static bool VerifyEmail(string email)
    {
      string[] parts = email.Split("@");
      return email.Contains("@") && parts[1].Contains(".");
    }

    private static string AcceptInput(string message)
    {
      string status = "";
      string text = "";
      string prediction = null;

      bool enterPressed = false;

      while (!enterPressed)
      {
        ApplyColors();
        Console.WriteLine(message);
        Console.Write(INPUT_PREFIX);
        Console.Write(text);
        ApplyColors(foregroundColor: ConsoleColor.DarkGray);
        if (prediction != null)
        {
          Console.Write(prediction);
          ApplyColors(HINT_COLOR);
          Console.Write(" (press tab to autocomplete)");
          Console.SetCursorPosition(INPUT_PREFIX.Length + text.Length, 1);
        }
        ApplyColors();
        ConsoleKeyInfo cki = Console.ReadKey();
        Console.Clear();

        switch (cki.Key)
        {
          case ConsoleKey.Backspace:
            if (text.Length > 0)
            {
              status = "backspace";
              text = text.Substring(0, text.Length - 1);
            }
            break;

          case ConsoleKey.Enter:
            enterPressed = true;
            break;
          case ConsoleKey.Tab:
            if (prediction.Length > 0)
            {
              text += prediction;
              prediction = null;
            }
            break;
          default:
            break;
        }

        if (validCharsRegex.IsMatch(cki.KeyChar.ToString()))
        {
          status = "legal character";
          text += cki.KeyChar;
        }
        else status = "non-allowed character or other key: " + cki.Key.ToString();

        prediction = null;

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
                  prediction = item.Substring(server.Length);
                  break;
                }
              }
            }
          }
        }

        // if (text.EndsWith("@g"))
        // {
        //   prediction = "mail.com";
        // }
        // else prediction = null;
      }

      return text;
    }
  }
}
