using Dal;
using DalApi;
using DO;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {
        //static readonly IDal s_dal = new DalList(); //stage 2
        static readonly IDal s_dal = new DalXml(); //stage 3
        private static readonly Random s_rand = new();
        static void DisplayMainMenu()
        {
            Console.WriteLine("Main Menu:\n1. Engineers\n2. Tasks\n3. Dependencies\nPress 0 to exit.");

        }
        static void DisplayEntitysMenu(string entity)
        {
            Console.WriteLine($"{entity} entity menu:\r\n1. Add a {entity}\r\n2. Present a {entity}\r\n3. View all {entity}s\r\n4. Update a {entity}\r\n5. Delete a {entity}\r\nPress 0 to exit back to the main menu");
        }

        static void TaskMenu()
        {
            while (true)
            {
                DisplayEntitysMenu("task");
                Crud choise = (Crud)int.Parse(Console.ReadLine()!);
                switch (choise)
                {
                    case Crud.CREATE:
                        Console.WriteLine("Enter Task description, alias, milestone(True or False),all relevant dates, a short description for the product, any remarks, the engineer Id and complexity level: ");
                        int id = 0;
                        string? description = Console.ReadLine();
                        string? alias = Console.ReadLine();
                        bool milestone = bool.Parse(Console.ReadLine() ?? "false");
                        DateTime creatAt = DateTime.Parse(Console.ReadLine() ?? $"{DateTime.Today}");
                        DateTime scheduledDate = DateTime.Parse(Console.ReadLine() ?? $"{creatAt.AddDays(s_rand.Next(1, 31))}");
                        DateTime start = DateTime.Parse(Console.ReadLine() ?? $"{scheduledDate.AddDays(s_rand.Next(0, 3))}");
                        TimeSpan forecastDate = TimeSpan.Parse(Console.ReadLine() ?? $"{new TimeSpan(s_rand.Next(1, 15), 0, 0)}");
                        DateTime deadLine = DateTime.Parse(Console.ReadLine() ?? $"{start.Add(forecastDate)}");
                        DateTime complete = DateTime.Parse(Console.ReadLine() ?? $"{deadLine.AddDays(s_rand.Next(0, 6))}");
                        string? productDescription = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int engineerId = int.Parse(Console.ReadLine() ?? "0");
                        EngineerExperience complexityLevel = (EngineerExperience)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                        s_dal.Task!.Create(new(id, description, alias, milestone, creatAt, start, scheduledDate, forecastDate, deadLine, complete, productDescription, remarks, engineerId, complexityLevel));
                        break;

                    case Crud.READ:
                        Console.WriteLine("Enter Task ID: ");
                        int idRead = int.Parse(Console.ReadLine()!);
                        DO.Task? readTask = s_dal.Task!.Read(idRead);
                        Console.WriteLine(readTask is null ? "Task was not found!\n" : readTask);
                        break;

                    case Crud.READALL:
                        foreach (var task in s_dal.Task.ReadAll())
                        {
                            Console.WriteLine(task);
                        }
                        break;

                    case Crud.DELETE:
                        Console.WriteLine("Enter Task ID: ");
                        int idDelete = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Task!.Delete(idDelete);
                        }
                        catch (DalDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter task Id:\n");
                            int updateId = int.Parse(Console.ReadLine()!);
                            DO.Task? updateTask = s_dal.Task!.Read(updateId) ?? throw new DalDoesNotExistException($"Can't update, task with ID: {updateId} does not exist!!");
                            Console.WriteLine("For each detaile, if it's need to be update, insert updated value. else,  press ENTER.\n");
                            string? updatedDescription = Console.ReadLine() ?? updateTask.Description;
                            string? updatedAlias = Console.ReadLine() ?? updateTask.Alias;
                            bool updatedMilestone = bool.Parse(Console.ReadLine() ?? $"{updateTask.IsMilestone}");
                            DateTime updatedCreatAt = DateTime.Parse(Console.ReadLine() ?? $"{updateTask.CreatedAtDate}");
                            DateTime updatedScheduledDate = DateTime.Parse(Console.ReadLine() ?? $"{updateTask.SchedualDate}");
                            DateTime updatedStart = DateTime.Parse(Console.ReadLine() ?? $"{updateTask.StartDate}");
                            TimeSpan updatedForecastDate = TimeSpan.Parse(Console.ReadLine() ?? $"{updateTask.Duration}");
                            DateTime updatedDeadLine = DateTime.Parse(Console.ReadLine() ?? $"{updateTask.DeadlineDate}");
                            DateTime updatedComplete = DateTime.Parse(Console.ReadLine() ?? $"{updateTask.CompleteDate}");
                            string? updatedProductDescription = Console.ReadLine() ?? updateTask.Deliverables;
                            string? updatedRemarks = Console.ReadLine() ?? updateTask.Remarks;
                            int updatedEngineer = int.Parse(Console.ReadLine() ?? $"{updateTask.EngineerId}");
                            EngineerExperience updatedComplexityLevel = (EngineerExperience)int.Parse(Console.ReadLine() ?? $"{updateTask.ComplexityLevel}");
                            s_dal.Task.Update(new(updateId, updatedDescription, updatedAlias, updatedMilestone, updatedCreatAt, updatedStart, updatedScheduledDate, updatedForecastDate, updatedDeadLine, updatedComplete, updatedProductDescription, updatedRemarks, updatedEngineer, updatedComplexityLevel));
                        }
                        catch (DalDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case Crud.NONE:
                        Console.WriteLine("Exiting CRUD menu...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }
            }
        }

        static void EngineerMenu()
        {
            while (true)
            {
                DisplayEntitysMenu("engineer");
                Crud choise = (Crud)int.Parse(Console.ReadLine()!);
                switch (choise)
                {
                    case Crud.CREATE:
                        Console.WriteLine("Enter engineer ID, Name, Email, Cost, and level: ");
                        int id = int.Parse(Console.ReadLine()!);
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        double? cost = double.Parse(Console.ReadLine() ?? $"{s_rand.Next(9000, 15000)}");
                        EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Engineer!.Create(new(id, name, email, level, cost));
                        }
                        catch (DalAlreadyExistsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case Crud.READ:
                        Console.WriteLine("Enter engineer ID: ");
                        int idRead = int.Parse(Console.ReadLine()!);
                        Engineer? readEngineer = s_dal.Engineer!.Read(idRead);
                        Console.WriteLine(readEngineer is null ? "Engineer was not found!\n" : readEngineer);
                        break;

                    case Crud.READALL:
                        foreach (var engineer in s_dal.Engineer.ReadAll())
                        {
                            Console.WriteLine(engineer);
                        }
                        break;

                    case Crud.DELETE:
                        Console.WriteLine("Enter engineer ID: ");
                        int idDelete = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Engineer!.Delete(idDelete);
                        }
                        catch (DalDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter the engineer's id:\n");
                            int updatedId = int.Parse(Console.ReadLine()!);
                            Engineer updatedEngineer = s_dal.Engineer.Read(updatedId) ?? throw new DalDoesNotExistException($"Can't update, engineer with ID: {updatedId} does not exist!!");
                            Console.WriteLine("For each detaile, if it's need to be update, insert updated value. else,  press ENTER.\n");
                            string updatedName = Console.ReadLine() ?? $"{updatedEngineer.Name}";
                            string updetedEmail = Console.ReadLine() ?? $"{updatedEngineer.Email}";
                            double updatedCost = double.Parse(Console.ReadLine() ?? $"{updatedEngineer.Cost}");
                            EngineerExperience updatedLevel = (EngineerExperience)int.Parse(Console.ReadLine() ?? $"{updatedEngineer.Level}");
                            s_dal.Engineer!.Update(new(updatedId, updatedName, updetedEmail, updatedLevel, updatedCost));
                        }
                        catch (DalDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case (int)Crud.NONE:
                        Console.WriteLine("Exiting CRUD menu...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }
            }
        }


        static void DependencyMenu()
        {
            while (true)
            {
                DisplayEntitysMenu("dependency");
                Crud choise = (Crud)int.Parse(Console.ReadLine()!);
                switch (choise)
                {
                    case Crud.CREATE:
                        Console.WriteLine("Enter ID for task and id for the  task that depends on it:\n");
                        int id = 0;
                        int dependentTask = int.Parse(Console.ReadLine()!);
                        int dependsOnTask = int.Parse(Console.ReadLine()!);
                        s_dal.Dependency!.Create(new(id, dependentTask, dependsOnTask));
                        break;

                    case Crud.READ:
                        Console.WriteLine("Enter Dependency ID: ");
                        int readId = int.Parse(Console.ReadLine()!);
                        Dependency? readDependency = s_dal.Dependency!.Read(readId);
                        Console.WriteLine(readDependency is null ? "Dependency was not found!\n" : readDependency);
                        break;

                    case Crud.READALL:

                        foreach (var dependency in s_dal.Dependency.ReadAll())
                        {
                            Console.WriteLine(dependency);
                        }
                        break;

                    case Crud.DELETE:
                        Console.WriteLine("Enter Dependency ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);
                        try {s_dal.Dependency!.Delete(deleteId);}
                        catch(DalDoesNotExistException ex) { Console.WriteLine(ex.Message);}
                        
                        break;
                    case Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter the requested dependency number, and two updated task codes:");
                            int updatedId = int.Parse(Console.ReadLine()!);
                            Dependency? updatedDependency = s_dal.Dependency.Read(updatedId) ?? throw new DalDoesNotExistException($"Can't update, dependency with ID: {updatedId} does not exist!!");
                            int updatedTask = int.Parse(Console.ReadLine()?? $"{updatedDependency.DependentTask}");
                            int updatedDepentOn = int.Parse(Console.ReadLine()?? $"{updatedDependency.DependsOnTask}");
                            s_dal.Dependency!.Update(new(updatedId, updatedTask, updatedDepentOn));
                        }
                        catch (DalDoesNotExistException ex) { Console.WriteLine(ex.Message); }
                        break;

                    case (int)Crud.NONE:
                        Console.WriteLine("Exiting dependency menu...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }
            }

        }
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dal);
                while (true)
                {
                    DisplayMainMenu();
                    int userChoice = int.Parse(Console.ReadLine()!);
                    switch (userChoice)
                    {
                        case 0: return;
                        case 1: EngineerMenu(); break;
                        case 2: TaskMenu(); break;
                        case 3: DependencyMenu(); break;
                        default: Console.WriteLine("Invalid selection, please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}