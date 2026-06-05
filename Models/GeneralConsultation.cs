namespace VetClinic.Models;

public class GeneralConsultation : VeterinaryService
{
    public GeneralConsultation() : base("General Consultation") { }

    public override string Attend() =>
        "Performing full physical examination, reviewing symptoms and medical history.";
}
