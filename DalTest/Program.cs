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
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static readonly Random s_rand = new();
        static void DisplayMainMenu()
        {
            Console.WriteLine("Main Menu:\n1. Engineers\n2. Tasks\n3. Dependencies");

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
                int choise = int.Parse(Console.ReadLine()!);
                switch (choise)
                {

                    case (int)Crud.CREATE:
                        Console.WriteLine("Enter Task description, alias, milestone(True or False),created date time, a short description for the product, any remarks, the engineer Id and complexity level: ");
                        int _id = 0;
                        string _description = Console.ReadLine()!;
                        string _alias = Console.ReadLine()!;
                        bool _milestone = bool.Parse(Console.ReadLine()!);
                        DateTime _creatAt = DateTime.Parse(Console.ReadLine()!);
                        DateTime _start = _creatAt.AddDays(s_rand.Next(0, 6));
                        DateTime _scheduledDate = _start.AddDays(s_rand.Next(14, 20)); 
                        DateTime _deadLine = _scheduledDate.AddDays(s_rand.Next(0, 6));
                        DateTime _forecastDate = _deadLine;
                        DateTime _complete = _deadLine.AddDays(s_rand.Next(0, 6));
                        string? _productDescription = Console.ReadLine();
                        string? _remarks = Console.ReadLine();
                        int _engineerId = int.Parse(Console.ReadLine());
                        EngineerExperience _complexityLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                        DO.Task newTask = new DO.Task(_id, _description, _alias, _milestone, _creatAt, _start, _scheduledDate, _deadLine,_forecastDate, _complete, _productDescription, _remarks, _engineerId, _complexityLevel);
                        s_dalTask.Create(newTask);
                        break;
                    case (int)Crud.READ:
                        Console.WriteLine("Enter Task ID: ");
                        int _idRead = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dalTask.Read(_idRead));
                        break;
                    case (int)Crud.READALL:
                        foreach (DO.Task task in s_dalTask.ReadAll())
                        {
                            Console.WriteLine(task);
                        }
                        break;
                    case (int)Crud.DELETE:
                        Console.WriteLine("Enter Task ID: ");
                        int _idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_dalTask.Delete(_idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case (int)Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter a task Id ,task description, alias, milestone(True or False) ,Date it was cerated at, אhe ID of the responsible engineer and his level: ");
                            int _idUpdate = int.Parse(Console.ReadLine());
                            string _descriptionUpdate = Console.ReadLine();
                            string _aliasUpdate = Console.ReadLine();
                            bool _milestoneUpdate = bool.Parse(Console.ReadLine());
                            DateTime _creatAtUpdate = DateTime.Parse(Console.ReadLine());
                            int _engineerNum = int.Parse(Console.ReadLine()!);
                            EngineerExperience _complexityLevelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                            DO.Task updateTask = new DO.Task(_idUpdate, _descriptionUpdate, _aliasUpdate, _milestoneUpdate, _creatAtUpdate, null, null, null, null, null, null, null, _engineerNum, _complexityLevelUpdate);
                            s_dalTask.Update(updateTask);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case (int)Crud.EXIT:
                        Console.WriteLine("Exiting CRUD menu...");
                        break;

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
                int choise = int.Parse(Console.ReadLine()!);
                switch (choise)
                {
                    case (int)Crud.CREATE:
                        Console.WriteLine("Enter engineer ID, Name, Email, Cost, and level: ");
                        int _id = int.Parse(Console.ReadLine()!);
                        string _name = Console.ReadLine()!;
                        string _email = Console.ReadLine()!;
                        double _cost = double.Parse(Console.ReadLine()!);
                        EngineerExperience _level = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);
                        Engineer newEngineer = new(_id, _name, _email, _level, _cost);
                        s_dalEngineer.Create(newEngineer);
                        break;
                    case (int)Crud.READ:
                        Console.WriteLine("Enter engineer ID: ");
                        int _idRead = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalEngineer.Read(_idRead));
                        break;
                    case (int)Crud.READALL:
                        foreach (Engineer engineer in s_dalEngineer.ReadAll())
                        {
                            Console.WriteLine(engineer);
                        }
                        break;

                    case (int)Crud.DELETE:
                        Console.WriteLine("Enter engineer ID: ");
                        int _idDelete = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dalEngineer.Delete(_idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case (int)Crud.UPDATE:
                        Console.WriteLine("Enter the engineer's ןג, and the details for the update (name, email, level, and cost)");
                        int _idUpdate = int.Parse(Console.ReadLine()!);
                        string? _nameUpdate = Console.ReadLine();
                        string? _emailUpdate = Console.ReadLine();
                        double _costUpdate = double.Parse(Console.ReadLine()!);
                        EngineerExperience _levelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);
                        try
                        {
                            Engineer updateEngineer = new(_idUpdate, _nameUpdate, _emailUpdate, _levelUpdate, _costUpdate);
                            s_dalEngineer.Update(updateEngineer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case (int)Crud.EXIT:
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
                int choise = int.Parse(Console.ReadLine()!);
                switch (choise)
                {
                    case (int)Crud.CREATE:
                        Console.WriteLine("Enter ID numbers for two tasks:");
                        int _Id = 0;
                        int _DependentTask = int.Parse(Console.ReadLine()!); ;
                        int _DependsOnTask = int.Parse(Console.ReadLine()!); ;
                        DO.Dependency newDependency = new DO.Dependency(_Id, _DependentTask, _DependsOnTask);
                        s_dalDependency.Create(newDependency);
                        break;
                    case (int)Crud.READ:
                        Console.WriteLine("Enter Dependency ID: ");
                        int _idRead = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalDependency.Read(_idRead));
                        break;
                    case (int)Crud.READALL:
                        foreach (Dependency dependency in s_dalDependency.ReadAll())
                        {
                            Console.WriteLine(dependency);
                        }
                        break;
                    case (int)Crud.DELETE:
                        Console.WriteLine("Enter Dependency ID: ");
                        int _idDelete = int.Parse(Console.ReadLine()!);
                        s_dalDependency.Delete(_idDelete);
                        break;
                    case (int)Crud.UPDATE:
                        Console.WriteLine("Enter the requested dependency number, and two updated task codes:");
                        int _IdUpdate = int.Parse(Console.ReadLine()!);
                        int _DependentTaskUpdate = int.Parse(Console.ReadLine()!);
                        int _DependsOnTaskUpdate = int.Parse(Console.ReadLine()!);
                        DO.Dependency updateDependency = new DO.Dependency(_IdUpdate, _DependentTaskUpdate, _DependsOnTaskUpdate);
                        s_dalDependency.Update(updateDependency);
                        break;
                    case (int)Crud.EXIT:
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
                Initialization.Do(s_dalEngineer, s_dalDependency, s_dalTask);
                while (true)
                {
                    DisplayMainMenu();
                    int userChoice = int.Parse(Console.ReadLine()!);
                    switch (userChoice)
                    {
                        case 0: return;
                        case 1: EngineerMenu(); break;
                        case 2: DependencyMenu(); break;
                        case 3: TaskMenu(); break;
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






