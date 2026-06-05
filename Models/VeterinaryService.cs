namespace VetClinic.Models;

public abstract class VeterinaryService : IAttendable
{
    public string ServiceName { get; }

    protected VeterinaryService(string serviceName)
    {
        ServiceName = serviceName;
    }

    public abstract string Attend();
}
