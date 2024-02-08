﻿using Dal;
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
        static readonly IDal s_dal = new DalXml();
        // = new DalXml(); //stage 3???
        private static readonly Random s_rand = new();
        //static Program()
        //{
        //    s_dal = new DalXml();
        //}
        #region casting functions
        //Functions to convert empty values received from the user to default values or previous values.
        static string? toString(string? source, string? defaulte) => string.IsNullOrEmpty(source) ? defaulte : source;
        static DateTime? toDateTime(string? source, DateTime? defaulte) => string.IsNullOrEmpty(source) ? defaulte : DateTime.Parse(source);
        static TimeSpan? toTimeSpan(string? source, TimeSpan? defaulte) => string.IsNullOrEmpty(source) ? defaulte : TimeSpan.Parse(source);
        static EngineerExperience toEngineerExperience(string? source, EngineerExperience defaulte)
        {
            if (string.IsNullOrEmpty(source)) return defaulte;
            Enum.TryParse(source, out EngineerExperience level);
            return level;
        }
        static int toInt(string? source, int defaulte) => string.IsNullOrEmpty(source) ? defaulte : int.Parse(source);
        static double? toDouble(string? source, double? defaulte) => string.IsNullOrEmpty(source) ? defaulte : double.Parse(source);
        static bool toBool(string? source, bool defaulte) => string.IsNullOrEmpty(source) ? defaulte : bool.Parse(source);

        //static void Initialize()
        //{
        //    Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        //    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        //    if (ans == "Y") //stage 3
        //        Initialization.Do(s_dal); //stage 2
        //}
        #endregion
        static void DisplayMainMenu()
        {
            Console.WriteLine("Main Menu:\n1. Engineers\n2. Tasks\n3. Dependencies\n4. Initialization\nPress 0 to exit.");

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
                        bool milestone = toBool(Console.ReadLine(), false);
                        DateTime? creatAt = toDateTime(Console.ReadLine(), DateTime.Today);
                        DateTime? scheduledDate = toDateTime(Console.ReadLine(), ((DateTime)creatAt!).AddDays(s_rand.Next(1, 31)));
                        DateTime? start = toDateTime(Console.ReadLine(), ((DateTime)scheduledDate!).AddDays(s_rand.Next(0, 3)));
                        TimeSpan? forecastDate = toTimeSpan(Console.ReadLine(), new TimeSpan(s_rand.Next(1, 15), 0, 0));
                        DateTime? deadLine = toDateTime(Console.ReadLine(), ((DateTime)start!).Add((TimeSpan)forecastDate!));
                        DateTime? complete = toDateTime(Console.ReadLine(), ((DateTime)deadLine!).AddDays(s_rand.Next(0, 6)));
                        string? productDescription = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int engineerId = toInt(Console.ReadLine(), s_rand.Next(200000000, 400000000));
                        EngineerExperience complexityLevel = toEngineerExperience(Console.ReadLine(), (EngineerExperience)s_rand.Next(0, 5));
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
                            string? updatedDescription = toString(Console.ReadLine(), updateTask.Description);
                            string? updatedAlias = toString(Console.ReadLine(), updateTask.Alias);
                            bool updatedMilestone = toBool(Console.ReadLine(), updateTask.IsMilestone);
                            DateTime? updatedCreatAt = toDateTime(Console.ReadLine(), updateTask.CreatedAtDate);
                            DateTime? updatedScheduledDate = toDateTime(Console.ReadLine(), updateTask.SchedualDate);
                            DateTime? updatedStart = toDateTime(Console.ReadLine(), updateTask.StartDate);
                            TimeSpan? updatedForecastDate = toTimeSpan(Console.ReadLine(), updateTask.Duration);
                            DateTime? updatedDeadLine = toDateTime(Console.ReadLine(), updateTask.DeadlineDate);
                            DateTime? updatedComplete = toDateTime(Console.ReadLine(), updateTask.CompleteDate);
                            string? updatedProductDescription = toString(Console.ReadLine(), updateTask.Deliverables);
                            string? updatedRemarks = toString(Console.ReadLine(), updateTask.Remarks);
                            int updatedEngineer = toInt(Console.ReadLine(), updateTask.EngineerId);
                            EngineerExperience updatedComplexityLevel = toEngineerExperience(Console.ReadLine(), updateTask.ComplexityLevel);
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
                        Console.WriteLine("Enter engineer ID, Name, Email, level, and Cost: ");
                        int id = int.Parse(Console.ReadLine()!);
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        EngineerExperience level = toEngineerExperience(Console.ReadLine(), (EngineerExperience)s_rand.Next(0, 5));
                        double? cost = toDouble(Console.ReadLine(), s_rand.Next(7000, 15000));
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
                            Console.WriteLine("Enter the engineer's id:");
                            int updatedId = int.Parse(Console.ReadLine()!);
                            Engineer updatedEngineer = s_dal.Engineer.Read(updatedId) ?? throw new DalDoesNotExistException($"Can't update, engineer with ID {updatedId} does not exist!!");
                            Console.Write("you can update name, email, level and cost");
                            Console.WriteLine("For each detaile, if it's need to be update, insert updated value. else,  press ENTER.\n");
                            string? updatedName = toString(Console.ReadLine(), updatedEngineer.Name);
                            string? updetedEmail = toString(Console.ReadLine(), updatedEngineer.Email);
                            EngineerExperience updatedLevel = toEngineerExperience(Console.ReadLine(), updatedEngineer.Level);
                            double? updatedCost = toDouble(Console.ReadLine(), updatedEngineer.Cost);
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
                        try { s_dal.Dependency!.Delete(deleteId); }
                        catch (DalDoesNotExistException ex) { Console.WriteLine(ex.Message); }

                        break;
                    case Crud.UPDATE:
                        try
                        {
                            Console.WriteLine("Enter the requested dependency number, and two updated task codes:");
                            int updatedId = int.Parse(Console.ReadLine()!);
                            Dependency? updatedDependency = s_dal.Dependency.Read(updatedId) ?? throw new DalDoesNotExistException($"Can't update, dependency with ID {updatedId} does not exist!!");
                            int updatedTask = toInt(Console.ReadLine(), updatedDependency.DependentTask);
                            int updatedDepentOn = toInt(Console.ReadLine(), updatedDependency.DependsOnTask);
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

        static void Main()
        {
            try
            {
                //s_dal = new DalXml();
                //Initialization.Do(s_dal);
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
                        case 4:
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y") //stage 3
                                Initialization.Do(s_dal); //stage 2
                            break;
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