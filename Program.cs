using System.Text.RegularExpressions;
using System.Diagnostics;
using System;

namespace kat
{
    class Program
    {

        static Regex validCharsRegex = new Regex(@"^[A-Za-z@.+-]$");

        static void Main(string[] args)
    {
      Console.Clear();
      // while (true) {
      Console.ForegroundColor = ConsoleColor.Gray;
      // Console.CursorVisible = false;

        string email = null;

        do {
            email = AcceptInput(email == null ? "What is your email?" : "Invalid email, try again");

        } while(!VerifyEmail(email));

        Console.WriteLine();

        // Console.Beep();d
        // Console.Clear();
        System.Console.WriteLine(email);
        // System.Environment.Exit(0);
        // }
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
      string prediction = "";

      while (true)
      {
        // if (text.Length > 10) break;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine(message);
        Console.Write(" > ");
        Console.Write(text);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(prediction);
        ConsoleKeyInfo cki = Console.ReadKey();

        if (cki.Key.Equals(ConsoleKey.Backspace))
        {
          if (text.Length > 0)
          {
            status = "backspace";
            text = text.Substring(0, text.Length - 1);
          }
        }
        if (cki.Key.Equals(ConsoleKey.Enter))
        {
          break;
        }
        if (cki.Key.Equals(ConsoleKey.Tab))
        {
          if (prediction.Length > 0)
          {
            text += prediction;
            prediction = "";
          }
        }

        if (validCharsRegex.IsMatch(cki.KeyChar.ToString()))
        {
          status = "legal character";
          text += cki.KeyChar;
        }
        else status = "non-allowed character or other key: " + cki.Key.ToString();
        if (text.EndsWith("@g"))
        {
          prediction = "mail.com";
        }
        else prediction = "";
      }

      return text;
    }
  }
}
