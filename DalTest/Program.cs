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
                int choise = int.Parse(Console.ReadLine()!);
                switch (choise)
                {

                    case (int)Crud.CREATE:
                        Console.WriteLine("Enter Task description, alias, milestone(True or False),all relevant dates, a short description for the product, any remarks, the engineer Id and complexity level: ");
                        int _id = 0;
                        string _description = Console.ReadLine()??"";
                        string _alias = Console.ReadLine()??"";
                        bool _milestone = bool.Parse(Console.ReadLine()??"false");
                        DateTime _creatAt = DateTime.Parse(Console.ReadLine()??$"{ DateTime.Today}");
                        DateTime _scheduledDate = DateTime.Parse(Console.ReadLine() ??$"{_creatAt.AddDays(s_rand.Next(0, 6))}");                       
                        DateTime _start = DateTime.Parse(Console.ReadLine() ?? $"{_scheduledDate.AddDays(s_rand.Next(14, 20))}"); 
                        TimeSpan _forecastDate = TimeSpan.Parse(Console.ReadLine() ?? $"{new TimeSpan(s_rand.Next(0, 30),0,0)}");
                        DateTime _deadLine = DateTime.Parse(Console.ReadLine() ?? $"{_start.Add(_forecastDate)}");
                        DateTime _complete = DateTime.Parse(Console.ReadLine() ?? $"{_deadLine.AddDays(s_rand.Next(0, 6))}");
                        string? _productDescription = Console.ReadLine();
                        string? _remarks = Console.ReadLine();
                        int _engineerId = int.Parse(Console.ReadLine()??"0");
                        EngineerExperience _complexityLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()??"${(EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length)}");
                        DO.Task newTask = new DO.Task(_id, _description, _alias, _milestone, _creatAt, _start, _scheduledDate, _forecastDate,_deadLine, _complete, _productDescription, _remarks, _engineerId, _complexityLevel);
                        s_dal.Task!.Create(newTask);
                        break;
                    case (int)Crud.READ:
                        Console.WriteLine("Enter Task ID: ");
                        int _idRead = int.Parse(Console.ReadLine()??"0");
                        Console.WriteLine(s_dal.Task!.Read(_idRead));
                        break;
                    case (int)Crud.READALL:
                        if (s_dal.Task != null)
                        {
                            foreach (var task in s_dal.Task.ReadAll())
                            {
                                Console.WriteLine(task);
                            }
                        }
                        else
                        {
                            Console.WriteLine("there are not tasks.");
                        }
                        break;
                    case (int)Crud.DELETE:
                        Console.WriteLine("Enter Task ID: ");
                        string? todelete = Console.ReadLine();
                        if (todelete != null)
                        {
                            int _idDelete = int.Parse(todelete);
                            try
                            {
                                s_dal.Task!.Delete(_idDelete);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("you didn't press any ID to delete");
                        }
                        break;
                    case (int)Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter a task Id ,task description, alias, milestone(True or False) ,Date it was cerated at, אhe ID of the responsible engineer and his level: ");
                            string? toupdate = Console.ReadLine();
                            if (toupdate != null)
                            {
                                int _idUpdate = int.Parse(toupdate);
                                string _descriptionUpdate = Console.ReadLine()??"";
                                string _aliasUpdate = Console.ReadLine()??"";
                                bool _milestoneUpdate = bool.Parse(Console.ReadLine()??"false");
                                DateTime _creatAtUpdate = DateTime.Parse(Console.ReadLine()??"${DateTime.New}");
                                int _engineerNum = int.Parse(Console.ReadLine()!);
                                EngineerExperience _complexityLevelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()??"${(EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length)}");
                                DO.Task updateTask = new DO.Task(_idUpdate, _descriptionUpdate, _aliasUpdate, _milestoneUpdate, _creatAtUpdate, null, null, null, null, null, null, null, _engineerNum, _complexityLevelUpdate);
                                s_dal.Task!.Update(updateTask);
                            }
                            else { Console.WriteLine("you didn't press any ID to update."); }
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

        static void EngineerMenu()
        {
            while (true)
            {
                DisplayEntitysMenu("engineer");
                string? schoise = Console.ReadLine();
                if (schoise == null) { Console.WriteLine("uoy didn't prees any choise."); }
                else
                {
                    int choise = int.Parse(schoise);
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
                            s_dal.Engineer!.Create(newEngineer);
                            break;
                        case (int)Crud.READ:
                            Console.WriteLine("Enter engineer ID: ");
                            int _idRead = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dal.Engineer!.Read(_idRead));
                            break;
                        case (int)Crud.READALL:
                            if(s_dal.Engineer != null) { 
                            foreach (var engineer in s_dal.Engineer.ReadAll())
                            {
                                Console.WriteLine(engineer);
                            }
                            }
                            else
                            {
                                Console.WriteLine("there are not engineers");
                            }
                            break;

                        case (int)Crud.DELETE:
                            Console.WriteLine("Enter engineer ID: ");
                            int _idDelete = int.Parse(Console.ReadLine()!);
                            try
                            {
                                s_dal.Engineer!.Delete(_idDelete);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;

                        case (int)Crud.UPDATE:
                            Console.WriteLine("Enter the engineer's , and the details for the update (name, email, level, and cost)");
                            int _idUpdate = int.Parse(Console.ReadLine()!);
                            string? _nameUpdate = Console.ReadLine();
                            string? _emailUpdate = Console.ReadLine();
                            double _costUpdate = double.Parse(Console.ReadLine()!);
                            EngineerExperience _levelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);
                            try
                            {
                                Engineer updateEngineer = new(_idUpdate, _nameUpdate, _emailUpdate, _levelUpdate, _costUpdate);
                                s_dal.Engineer!.Update(updateEngineer);
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
                        int _DependentTask = int.Parse(Console.ReadLine()!); 
                        int _DependsOnTask = int.Parse(Console.ReadLine()!); 
                        DO.Dependency newDependency = new(_Id, _DependentTask, _DependsOnTask);
                        s_dal.Dependency!.Create(newDependency);
                        break;
                    case (int)Crud.READ:
                        Console.WriteLine("Enter Dependency ID: ");
                        int _idRead = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dal.Dependency!.Read(_idRead));
                        break;
                    case (int)Crud.READALL:
                        if(s_dal.Dependency != null) { 
                        foreach (var dependency in s_dal.Dependency.ReadAll())
                        {
                            Console.WriteLine(dependency);
                        }
                        }
                        else
                        {
                            Console.WriteLine("there are not Dependencies.");
                        }
                        break;
                    case (int)Crud.DELETE:
                        Console.WriteLine("Enter Dependency ID: ");
                        int _idDelete = int.Parse(Console.ReadLine()!);
                        s_dal.Dependency!.Delete(_idDelete);
                        break;
                    case (int)Crud.UPDATE:
                        Console.WriteLine("Enter the requested dependency number, and two updated task codes:");
                        int _IdUpdate = int.Parse(Console.ReadLine()!);
                        int _DependentTaskUpdate = int.Parse(Console.ReadLine()!);
                        int _DependsOnTaskUpdate = int.Parse(Console.ReadLine()!);
                        DO.Dependency updateDependency = new DO.Dependency(_IdUpdate, _DependentTaskUpdate, _DependsOnTaskUpdate);
                        s_dal.Dependency!.Update(updateDependency);
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