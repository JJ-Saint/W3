using VetClinic.Services;

namespace VetClinic.Models;

public class ClinicViewModel
{
    public List<Patient> Patients { get; set; } = new();
    public List<VeterinaryService> Services { get; set; } = new();
    public List<ExceptionDemoResult> ExceptionDemos { get; set; } = new();
    public List<string> LogEntries { get; set; } = new();
    public List<NotificationResult> Notifications { get; set; } = new();
    public ParallelBatchResult WhenAllDemo { get; set; } = new();
    public ParallelBatchResult WhenAnyDemo { get; set; } = new();
    public List<ParallelBatchResult> WorkflowDemos { get; set; } = new();
}

public class ExceptionDemoResult
{
    public string Scenario { get; set; } = "";
    public string ExceptionType { get; set; } = "";
    public string Message { get; set; } = "";
    public string FinallyBlock { get; set; } = "";
    public bool WasHandled { get; set; }
}

public class NotificationResult
{
    public string PatientName { get; set; } = "";
    public string Message { get; set; } = "";
}
