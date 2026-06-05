using Microsoft.AspNetCore.Mvc;
using VetClinic.Models;
using VetClinic.Services;

namespace VetClinic.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        var rex   = new Pet("Rex",   3, "Dog",  "German Shepherd", "Carlos Mendez");
        var luna  = new Pet("Luna",  5, "Cat",  "Siamese",         "Carlos Mendez");
        var kiwi  = new Pet("Kiwi",  2, "Bird", "Canary",          "Sofia Torres");
        var mochi = new Pet("Mochi", 4, "Cat",  "Persian",         "Sofia Torres");
        var bolt  = new Pet("Bolt",  1, "Dog",  "Labrador",        "Andres Ruiz");

        var carlos = new Patient("Carlos Mendez", 34, "Calle 10 #45-12, Medellín",         "3001234567");
        carlos.AddPet(rex);
        carlos.AddPet(luna);

        var sofia = new Patient("Sofia Torres",  28, "Carrera 80 #33-50, Medellín",         "3119876543");
        sofia.AddPet(kiwi);
        sofia.AddPet(mochi);

        var andres = new Patient("Andres Ruiz",  41, "Avenida El Poblado #15-22, Medellín", "3204567890");
        andres.AddPet(bolt);

        var patients = new List<Patient> { carlos, sofia, andres };

        var notifications = patients
            .Select(p => new NotificationResult
            {
                PatientName = p.Name,
                Message     = p.SendNotification()
            })
            .ToList();

        var exceptionDemos = BuildExceptionDemos(carlos);

        var patientNames = patients.Select(p => p.Name).ToList();
        var petNames     = patients.SelectMany(p => p.Pets).Select(p => p.Name).ToList();

        var whenAllDemo   = await ClinicAsyncService.RunWhenAllDemoAsync(patientNames);
        var whenAnyDemo   = await ClinicAsyncService.RunWhenAnyDemoAsync(petNames);

        var workflowDemos = await Task.WhenAll(
            patients.Select(p => ClinicAsyncService.RunParallelClinicWorkflowAsync(p.Name))
        );

        ClinicLogger.LogInfo("Index (async) loaded", "HomeController");

        var model = new ClinicViewModel
        {
            Patients       = patients,
            Services       = new List<VeterinaryService> { new GeneralConsultation(), new Vaccination() },
            Notifications  = notifications,
            ExceptionDemos = exceptionDemos,
            LogEntries     = ClinicLogger.GetRecentEntries(8),
            WhenAllDemo    = whenAllDemo,
            WhenAnyDemo    = whenAnyDemo,
            WorkflowDemos  = workflowDemos.ToList()
        };

        return View(model);
    }

    private List<ExceptionDemoResult> BuildExceptionDemos(Patient carlos)
    {
        var results = new List<ExceptionDemoResult>();

        var demo1 = new ExceptionDemoResult { Scenario = "Search for a pet that does not exist" };
        try
        {
            carlos.FindPet("Firulais");
        }
        catch (PetNotFoundException ex)
        {
            demo1.ExceptionType = nameof(PetNotFoundException);
            demo1.Message       = ex.Message;
            demo1.WasHandled    = true;
            ClinicLogger.LogError(ex, "FindPet");
        }
        finally
        {
            demo1.FinallyBlock = "Search operation completed — resources released.";
        }
        results.Add(demo1);

        var demo2 = new ExceptionDemoResult { Scenario = "Add a pet with a name already registered" };
        try
        {
            carlos.AddPet(new Pet("Rex", 2, "Dog", "Poodle", "Carlos Mendez"));
        }
        catch (DuplicatePetException ex)
        {
            demo2.ExceptionType = nameof(DuplicatePetException);
            demo2.Message       = ex.Message;
            demo2.WasHandled    = true;
            ClinicLogger.LogError(ex, "AddPet");
        }
        finally
        {
            demo2.FinallyBlock = "Add pet operation completed — database transaction closed.";
        }
        results.Add(demo2);

        var demo3 = new ExceptionDemoResult { Scenario = "Create a patient with an invalid age (-5)" };
        try
        {
            var _ = new Patient("Ghost Patient", -5, "Unknown", "0000000000");
        }
        catch (InvalidPatientAgeException ex)
        {
            demo3.ExceptionType = nameof(InvalidPatientAgeException);
            demo3.Message       = ex.Message;
            demo3.WasHandled    = true;
            ClinicLogger.LogError(ex, "Patient constructor");
        }
        finally
        {
            demo3.FinallyBlock = "Patient creation attempt ended — validation log saved.";
        }
        results.Add(demo3);

        var demo4 = new ExceptionDemoResult { Scenario = "Division by zero in average age calculation" };
        try
        {
            int totalAge  = 0;
            int totalPets = 0;
            int avgAge    = totalAge / totalPets;
            demo4.Message = $"Average: {avgAge}";
        }
        catch (DivideByZeroException ex)
        {
            demo4.ExceptionType = nameof(DivideByZeroException);
            demo4.Message       = ex.Message;
            demo4.WasHandled    = true;
            ClinicLogger.LogError(ex, "AverageAgeCalculation");
        }
        finally
        {
            demo4.FinallyBlock = "Calculation block finished — no partial results stored.";
        }
        results.Add(demo4);

        return results;
    }
}
