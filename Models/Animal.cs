namespace VetClinic.Models;

public abstract class Animal
{
    private string _name;
    private int _age;
    private string _species;

    public string Name { get => _name; set => _name = value; }
    public int Age { get => _age; set => _age = value; }
    public string Species { get => _species; set => _species = value; }

    protected Animal(string name, int age, string species)
    {
        _name = name;
        _age = age;
        _species = species;
    }

    public virtual string MakeSound() => "...";
}
