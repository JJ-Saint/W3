namespace VetClinic.Models;

public class Vaccination : VeterinaryService
{
    public Vaccination() : base("Vaccination") { }

    public override string Attend() =>
        "Administering scheduled vaccines and updating immunization records.";
}
