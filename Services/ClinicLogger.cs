namespace VetClinic.Services;

public static class ClinicLogger
{
    private static readonly string LogPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "clinic_errors.log");

    public static void LogError(Exception ex, string context = "")
    {
        var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR | {context} | {ex.GetType().Name}: {ex.Message}";
        WriteEntry(entry);
    }

    public static void LogWarning(string message, string context = "")
    {
        var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WARN  | {context} | {message}";
        WriteEntry(entry);
    }

    public static void LogInfo(string message, string context = "")
    {
        var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO  | {context} | {message}";
        WriteEntry(entry);
    }

    private static void WriteEntry(string entry)
    {
        try
        {
            File.AppendAllText(LogPath, entry + Environment.NewLine);
        }
        catch
        {
            Console.WriteLine($"[Logger] Could not write to log file. Entry: {entry}");
        }
    }

    public static List<string> GetRecentEntries(int count = 10)
    {
        try
        {
            if (!File.Exists(LogPath))
                return new List<string> { "No log entries yet." };

            var lines = File.ReadAllLines(LogPath);
            return lines.TakeLast(count).Reverse().ToList();
        }
        catch
        {
            return new List<string> { "Could not read log file." };
        }
    }
}
