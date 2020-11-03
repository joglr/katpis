using System.Net.Mail;
using System.IO;
using System.Text.RegularExpressions;
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
    const ConsoleColor PREDICTION_COLOR = DarkGray;
    const string COMMON_EMAIL_SUFFIXES_PATH = @"Resources\email-suffixes.csv";
    const string validCharsPattern = @"^\S$";

    public static string[] commonEmailSuffixes;


    static async Task Main(string[] args)
    {

      Util.Colored(() => Console.Write("Loading..."));
      ClearCurrentConsoleLine();
      commonEmailSuffixes = await File.ReadAllLinesAsync(COMMON_EMAIL_SUFFIXES_PATH);
      Console.Title = "Kat CLI";

      string email = null;

      do
      {
        ClearCurrentConsoleLine();
        Util.Colored(() =>
        {
          email = AcceptInput(message: email == null
              ? "Enter your email: "
              : "Invalid email, try again");
        });
      } while (!VerifyEmail(email: email));

      ClearCurrentConsoleLine();
      Console.WriteLine(email);
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

    public static void ClearCurrentConsoleLine()
    {
      int currentLineCursor = Console.CursorTop;
      Console.SetCursorPosition(0, Console.CursorTop);
      for (int i = 0; i < Console.WindowWidth; i++)
        Console.Write(" ");
      Console.SetCursorPosition(0, currentLineCursor);
    }

    private static string AcceptInput(string message, String acceptedCharsPattern = validCharsPattern)
    {
      string status = "";
      string text = "";
      string prediction = null;

      bool enterPressed = false;

      while (!enterPressed)
      {

        Util.Colored(() =>
        {
          ClearCurrentConsoleLine();
          Console.Write(message);
          Console.Write(text);
        });

        if (prediction != null)
        {
          var cl = Console.CursorLeft;
          var ct = Console.CursorTop;
          Util.Colored(() => Console.Write(prediction), foregroundColor: PREDICTION_COLOR);
          Console.SetCursorPosition(cl, ct);
        }
        ConsoleKeyInfo cki = Console.ReadKey();

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
            if (prediction != null)
            {
              text += prediction;
              prediction = null;
            }
            break;

          case ConsoleKey.UpArrow:
          case ConsoleKey.DownArrow:
          case ConsoleKey.LeftArrow:
          case ConsoleKey.RightArrow:

            break;

          default:
            if (new Regex(validCharsPattern).IsMatch(cki.KeyChar.ToString()))
            {
              status = "legal character: \"" + cki.KeyChar + "\"";
              text += cki.KeyChar;
              text = text.Trim();
            }
            else status = "non-allowed character or other key: " + cki.Key.ToString();
            break;
        }

        prediction = null;

        if (text.Contains("@") && (text.Split("@").Length > 1))
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
      }

      return text;
    }
  }
}
