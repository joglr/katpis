using System;
using System.Text.RegularExpressions;
using static System.ConsoleColor;

namespace kat
{
  class ConsoleUtils
  {
    const ConsoleColor PREDICTION_COLOR = DarkGray;
    public static readonly Func<string, string> emptyPredictor = _ => "";

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
      Func<string, string> getPrediction = null,
      string acceptedCharsPattern = ".")
    {
      if (getPrediction == null) getPrediction = emptyPredictor;
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

        prediction = getPrediction(text);
      }

      return text;
    }
  }
}
