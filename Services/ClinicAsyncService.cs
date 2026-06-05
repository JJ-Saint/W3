namespace VetClinic.Services;

public class AsyncTaskResult
{
    public string TaskName { get; set; } = "";
    public string Outcome { get; set; } = "";
    public long ElapsedMs { get; set; }
}

public class ParallelBatchResult
{
    public string BatchName { get; set; } = "";
    public List<AsyncTaskResult> Tasks { get; set; } = new();
    public string WhenAllSummary { get; set; } = "";
    public string WhenAnySummary { get; set; } = "";
    public long TotalElapsedMs { get; set; }
}

public static class ClinicAsyncService
{
    public static async Task<AsyncTaskResult> RegisterPatientAsync(string patientName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await Task.Delay(120);
        sw.Stop();
        return new AsyncTaskResult
        {
            TaskName   = $"RegisterPatientAsync({patientName})",
            Outcome    = $"Patient '{patientName}' saved to database.",
            ElapsedMs  = sw.ElapsedMilliseconds
        };
    }

    public static async Task<AsyncTaskResult> LoadMedicalHistoryAsync(string petName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await Task.Delay(200);
        sw.Stop();
        return new AsyncTaskResult
        {
            TaskName  = $"LoadMedicalHistoryAsync({petName})",
            Outcome   = $"Medical history for '{petName}' loaded — 3 prior visits found.",
            ElapsedMs = sw.ElapsedMilliseconds
        };
    }

    public static async Task<AsyncTaskResult> ScheduleAppointmentAsync(string patientName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await Task.Delay(80);
        sw.Stop();
        return new AsyncTaskResult
        {
            TaskName  = $"ScheduleAppointmentAsync({patientName})",
            Outcome   = $"Appointment scheduled for '{patientName}' — next available slot confirmed.",
            ElapsedMs = sw.ElapsedMilliseconds
        };
    }

    public static async Task<AsyncTaskResult> SendNotificationAsync(string patientName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await Task.Delay(50);
        sw.Stop();
        return new AsyncTaskResult
        {
            TaskName  = $"SendNotificationAsync({patientName})",
            Outcome   = $"SMS reminder sent to '{patientName}' — delivery confirmed.",
            ElapsedMs = sw.ElapsedMilliseconds
        };
    }

    public static async Task<AsyncTaskResult> RegisterPetAsync(string petName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await Task.Delay(new Random().Next(60, 220));
        sw.Stop();
        return new AsyncTaskResult
        {
            TaskName  = $"RegisterPetAsync({petName})",
            Outcome   = $"Pet '{petName}' registered and assigned an ID.",
            ElapsedMs = sw.ElapsedMilliseconds
        };
    }

    public static async Task<ParallelBatchResult> RunWhenAllDemoAsync(List<string> patientNames)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var tasks = patientNames.Select(name => Task.Run(() => RegisterPatientAsync(name))).ToList();
        var results = await Task.WhenAll(tasks);

        sw.Stop();

        return new ParallelBatchResult
        {
            BatchName       = "Task.WhenAll — Register all patients in parallel",
            Tasks           = results.ToList(),
            WhenAllSummary  = $"All {results.Length} registrations completed. Total wall-clock time: {sw.ElapsedMilliseconds} ms (sequential would have taken ~{results.Sum(r => r.ElapsedMs)} ms).",
            TotalElapsedMs  = sw.ElapsedMilliseconds
        };
    }

    public static async Task<ParallelBatchResult> RunWhenAnyDemoAsync(List<string> petNames)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var tasks = petNames
            .Select(name => Task.Run(() => RegisterPetAsync(name)))
            .ToList();

        var firstCompleted = await Task.WhenAny(tasks);
        var firstResult    = await firstCompleted;

        var allResults = await Task.WhenAll(tasks);
        sw.Stop();

        return new ParallelBatchResult
        {
            BatchName      = "Task.WhenAny — First pet registered wins",
            Tasks          = allResults.ToList(),
            WhenAnySummary = $"'{firstResult.TaskName}' finished first at {firstResult.ElapsedMs} ms. Task.WhenAny returned immediately without waiting for the others.",
            TotalElapsedMs = sw.ElapsedMilliseconds
        };
    }

    public static async Task<ParallelBatchResult> RunParallelClinicWorkflowAsync(string patientName)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var historyTask     = LoadMedicalHistoryAsync(patientName);
        var appointmentTask = ScheduleAppointmentAsync(patientName);
        var notificationTask = SendNotificationAsync(patientName);

        var results = await Task.WhenAll(historyTask, appointmentTask, notificationTask);
        sw.Stop();

        return new ParallelBatchResult
        {
            BatchName      = $"Parallel clinic workflow for {patientName}",
            Tasks          = results.ToList(),
            WhenAllSummary = $"All 3 concurrent operations finished in {sw.ElapsedMilliseconds} ms.",
            TotalElapsedMs = sw.ElapsedMilliseconds
        };
    }
}
