namespace VetClinic.Models;

public interface IAttendable
{
    string Attend();
    string ServiceName { get; }
}
