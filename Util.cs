using System;

namespace kat
{
  class Util
  {
    const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;

    public static void Colored(
      Action act,
      ConsoleColor foregroundColor = FOREGROUND_COLOR,
      ConsoleColor backgroundColor = BACKGROUND_COLOR
    )
    {
      ConsoleColor prevForeground = Console.ForegroundColor;
      ConsoleColor prevBackground = Console.BackgroundColor;
      Console.ForegroundColor = foregroundColor;
      Console.BackgroundColor = backgroundColor;
      act();
      Console.ForegroundColor = prevForeground;
      Console.BackgroundColor = prevBackground;
    }
  }
}
