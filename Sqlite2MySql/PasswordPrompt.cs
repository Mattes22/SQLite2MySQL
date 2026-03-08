namespace Sqlite2MySql;

internal static class PasswordPrompt
{
    public static string ReadHidden(string prompt)
    {
        Console.Error.Write(prompt);
        var buffer = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.Error.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Count > 0)
                {
                    buffer.RemoveAt(buffer.Count - 1);
                }
                continue;
            }
            buffer.Add(key.KeyChar);
        }
        return new string(buffer.ToArray());
    }
}
