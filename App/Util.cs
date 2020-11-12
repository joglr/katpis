using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static System.ConsoleColor;

namespace Katpis
{
  class ConsoleUtils
  {
    const ConsoleColor PREDICTION_COLOR = DarkGray;

    public class EmptyPredictor : Predictor
    {
      public string Predict(string input) {
        return "";
      }
    }

    public static bool EmailVerifier(string email)
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

    public static void Colored(
      Action act,
      ConsoleColor? foregroundColor = null,
      ConsoleColor? backgroundColor = null
    )
    {
      ConsoleColor prevForeground = Console.ForegroundColor;
      ConsoleColor prevBackground = Console.BackgroundColor;
      Console.ForegroundColor = foregroundColor == null ? Console.ForegroundColor : (ConsoleColor)foregroundColor;
      Console.BackgroundColor = backgroundColor == null ? Console.BackgroundColor : (ConsoleColor)backgroundColor;
      act();
      Console.ForegroundColor = prevForeground;
      Console.BackgroundColor = prevBackground;
    }

    public static void ClearCurrentConsoleLine()
    {
      int currentLineCursor = Console.CursorTop;
      Console.SetCursorPosition(0, Console.CursorTop);
      for (int i = 0; i < Console.WindowWidth; i++)
        Console.Write(" ");
      Console.SetCursorPosition(0, currentLineCursor);
    }

    public static string AcceptInput(
      string helpMessage,
      string errorMessage,
      Predicate<string> checkValid,
      Predictor predictor = null,
      string acceptedCharsPattern = ".")
    {
      Console.TreatControlCAsInput = true;
      if (predictor == null) predictor = new EmptyPredictor();
      string message = helpMessage;
      string text = "";
      string prediction = null;

      bool enterPressed = false;
      bool isValid = false;

      while (!enterPressed)
      {
        isValid = checkValid(text);

        ConsoleUtils.Colored(() =>
        {
          ConsoleUtils.ClearCurrentConsoleLine();
          Console.Write(" ");
          if (text.Length.Equals(0)) Console.Write(" ");
          else ConsoleUtils.Colored(() => Console.Write(isValid ? "✓" : "×"), foregroundColor: isValid ? Green : Red);
          Console.Write(" " + message);
          Console.Write(text);
        });

        if (prediction != null)
        {
          var cl = Console.CursorLeft;
          var ct = Console.CursorTop;
          ConsoleUtils.Colored(() => Console.Write(prediction), foregroundColor: PREDICTION_COLOR);
          Console.SetCursorPosition(cl, ct);
        }

        ConsoleKeyInfo cki = Console.ReadKey(true);
        // Terminate program on Ctrl + C
        if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
          if (cki.Key.Equals(ConsoleKey.C))
          {
            System.Environment.Exit(128);
          }
        }

        switch (cki.Key)
        {
          case ConsoleKey.Backspace:
            if (text.Length > 0)
            {
              text = text.Substring(0, text.Length - 1);
            }
            break;

          case ConsoleKey.Enter:
            if (isValid)
              enterPressed = true;
            break;

          case ConsoleKey.Tab:
            if (prediction != null)
            {
              text += prediction;
              prediction = null;
            }
            break;
          default:
            var regex = new Regex(acceptedCharsPattern);

            if (!char.IsControl(cki.KeyChar))
            {
              text += cki.KeyChar;
              text = text.Trim();
            }
            break;
        }

        prediction = predictor.Predict(text);
      }

      Console.TreatControlCAsInput = false;
      return text;
    }
  }

  public interface Predictor {
     string Predict(string input);
  };

  public class EmailPredicter : Predictor {
    private string COMMON_EMAIL_SUFFIXES_PATH =  @"\Resources\email-suffixes.csv";
    const string validCharsPattern = @"^\S$";
    public static string[] commonEmailSuffixes;

    public EmailPredicter() {

      commonEmailSuffixes = File.ReadAllLines(Path.Join(
          Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
          COMMON_EMAIL_SUFFIXES_PATH
      ));
    }

    public string Predict(string input)  {
      if (input.Contains("@"))
      {
        var parts = input.Split("@");
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
    }
  }
}
