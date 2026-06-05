namespace VetClinic.Models;

public class PetNotFoundException : Exception
{
    public string PetName { get; }

    public PetNotFoundException(string petName)
        : base($"Pet '{petName}' was not found in the system.")
    {
        PetName = petName;
    }
}

public class InvalidPatientAgeException : Exception
{
    public int AttemptedAge { get; }

    public InvalidPatientAgeException(int age)
        : base($"Patient age '{age}' is not valid. Age must be between 0 and 120.")
    {
        AttemptedAge = age;
    }
}

public class DuplicatePetException : Exception
{
    public string PetName { get; }

    public DuplicatePetException(string petName)
        : base($"A pet named '{petName}' is already registered to this patient.")
    {
        PetName = petName;
    }
}
