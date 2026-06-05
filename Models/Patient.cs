namespace VetClinic.Models;

public class Patient : IRegistrable, INotifiable
{
    private string _name;
    private int _age;
    private string _address;
    private string _phone;
    private readonly List<Pet> _pets;

    public string Name { get => _name; set => _name = value; }
    public int Age { get => _age; set => _age = value; }
    public string Address { get => _address; set => _address = value; }
    public string Phone { get => _phone; private set => _phone = value; }
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

    public Patient(string name, int age, string address, string phone)
    {
        if (age < 0 || age > 120)
            throw new InvalidPatientAgeException(age);

        _name = name;
        _age = age;
        _address = address;
        _phone = phone;
        _pets = new List<Pet>();
    }

    public void AddPet(Pet pet)
    {
        if (_pets.Any(p => p.Name.Equals(pet.Name, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicatePetException(pet.Name);

        _pets.Add(pet);
    }

    public Pet FindPet(string name)
    {
        var pet = _pets.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (pet is null)
            throw new PetNotFoundException(name);

        return pet;
    }

    public string Register() =>
        $"[PATIENT] {Name} | Age: {Age} | Address: {Address} | Phone: {Phone}";

    public string SendNotification() =>
        $"Reminder sent to {Name} at {Phone}: Your pet's appointment is scheduled. Please confirm.";

    public string DisplayInfo() =>
        $"{Name}, {Age} yrs — {Address}";
}
