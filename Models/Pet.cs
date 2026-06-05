namespace VetClinic.Models;

public class Pet : Animal, IRegistrable
{
    private string _breed;
    private string _ownerName;

    public string Breed { get => _breed; set => _breed = value; }
    public string OwnerName { get => _ownerName; set => _ownerName = value; }

    public Pet(string name, int age, string species, string breed, string ownerName)
        : base(name, age, species)
    {
        _breed = breed;
        _ownerName = ownerName;
    }

    public override string MakeSound() => Species switch
    {
        "Dog" => "Woof!",
        "Cat" => "Meow!",
        "Bird" => "Tweet!",
        _ => "..."
    };

    public string Register() =>
        $"[PET] {Name} | {Species} | {Breed} | Age: {Age} | Owner: {OwnerName}";

    public string DisplayInfo() =>
        $"{Name} ({Species}, {Breed}) — {Age} yr(s) old";
}
