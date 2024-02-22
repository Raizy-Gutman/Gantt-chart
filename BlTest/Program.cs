using DalTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BlImplementation;
using BO;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace BlTest
{
    internal class Program
    {//איפה לבצע את כל הבדיקות והזריקות???
        //לשאול את יהודית אם הוא עובר דרך הטסט ואיפה נראה לה נכון לזרוק את השגיאות.
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static void Main()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                Initialization.Do();

            while (true)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Engineer");
                Console.WriteLine("2. Task");
                Console.WriteLine("3. Milestone");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        EngineerMenu();
                        break;

                    case "2":
                        TaskMenu();
                        break;

                    case "3":
                        MilestoneMenu();
                        break;

                    case "4":
                        Console.WriteLine("Exiting the program.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void EngineerMenu()
        {
            while (true)
            {
                Console.WriteLine("\nEngineer Menu:");
                Console.WriteLine("1. Add Engineer");
                Console.WriteLine("2. Delete Engineer");
                Console.WriteLine("3. Get Engineer Details");
                Console.WriteLine("4. Get all Engineers");
                Console.WriteLine("5. Update Engineer");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Engineer details:");
                        Console.Write("Enter ID: ");
                        //int id = int.Parse(Console.ReadLine());
                        int id = -1;
                        if (int.TryParse(Console.ReadLine(), out int result))
                        {
                            id = result;
                        }
                        Console.Write("Enter Name: ");
                        string Name = Console.ReadLine()??"";
                        Console.Write("Enter Email: ");
                        string Email = Console.ReadLine()??"";
                        Console.Write("Enter  Level:");
                        BO.EngineerExperience level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()??"0", true);// או שצריך לכתוב:??"Beginnerלשאול את יהודית אם זה נכון"
                        Console.Write("Enter Cost: ");
                        double cost = -1;
                        if (int.TryParse(Console.ReadLine(), out int resultParse))
                        {
                            cost = resultParse;
                        }
                        Console.Write("Did you want assign a task to an engineer? (Y/N)");
                        string ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        TaskInEngineer? taskToEngineer = null;
                        if (ans == "Y")//הTaskImplementation כבר יבדוק אותם ויזרוק שגיאות? או שצריך לבדוק אותם כאן???
                        {
                            Console.Write("Enter Id Task For Engineer:");
                            string strIdTask = Console.ReadLine()??"";
                            int idTask = -1;
                            if (int.TryParse(strIdTask, out int resoultTaskId))
                            {
                                idTask = resoultTaskId;
                            };
                            //if (!int.TryParse(strIdTask, out idTask))
                            //    throw new FormatException("Wrong input");

                            Console.Write("Enter Alias Task For Engineer:");
                            string aliasTask = Console.ReadLine()??"";
                            //if (aliasTask == string.Empty)
                            //    throw new FormatException("Wrong input");

                            taskToEngineer = new ()
                            {
                                Id = idTask,
                                Alias = aliasTask
                            };
                        }


                        BO.Engineer newEngineer = new ()
                        {
                            Id = id,
                            Name = Name,
                            Email = Email,
                            Level = level,
                            Cost = cost,
                            Task = taskToEngineer
                        };

                        try
                        {
                            int newEngineerId = s_bl.Engineer.CreateEngineer(newEngineer);
                            Console.WriteLine($"Engineer with id: {newEngineerId} was succesfully created!");
                        }
                        catch (BO.BlAlreadyExistsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "2":
                        Console.Write("Enter Engineer ID to delete: ");
                        int engineerIdToDelete = -1;
                        if (int.TryParse(Console.ReadLine(), out int answer))
                        {
                            engineerIdToDelete = answer;
                        }
                        try
                        {
                            s_bl.Engineer.DeleteEngineer(engineerIdToDelete);
                            Console.WriteLine($"Engineer with id: {engineerIdToDelete} was succesfully deleted!");
                        }
                        catch (BO.BlDeletionImpossible ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "3":
                        Console.Write("Enter Engineer ID to get details: ");
                        int engineerIdToGetDetails = -1;
                        if (int.TryParse(Console.ReadLine(), out int answerb))
                        {
                            engineerIdToGetDetails = answerb;
                        }
                        try
                        {
                            var engineerDetails = s_bl.Engineer.GetEngineer(engineerIdToGetDetails);
                            Console.WriteLine($"Engineer Details:\n{engineerDetails}");
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                        break;

                    case "4":

                        var engineers = s_bl.Engineer.ReadAllEngineers();
                        Console.WriteLine("All Engineers:");
                        foreach (var engineer in engineers)
                        {
                            Console.WriteLine(engineer);
                        }
                        break;

                    case "5":
                        Console.Write("Enter updated Engineer details: ");
                        Console.Write("Enter ID: ");
                        int idUpdate = -1;
                        if (int.TryParse(Console.ReadLine(), out int resultIdUpdate))
                        {
                            idUpdate = resultIdUpdate;
                        }
                        Console.Write("Enter updated Name: ");
                        string NameUpdate = Console.ReadLine()??"";
                        Console.Write("Enter updated Email: ");
                        string EmailUpdate = Console.ReadLine()??"";
                        Console.Write("Enter updated Level: ");
                        BO.EngineerExperience levelUpdate = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()??"0", true);//אותו שאלה
                        Console.Write("Enter Cost: ");
                        double CostUpdate = -1;
                        if (int.TryParse(Console.ReadLine(), out int resultParseCost))
                        {
                            CostUpdate = resultParseCost;
                        }
                        Console.Write("Did you want assign a task to an engineer? (Y/N)");
                        string ansYOrN = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        TaskInEngineer? taskToUpdateEngineer = null;
                        if (ansYOrN == "Y")//הTaskImplementation כבר יבדוק אותם ויזרוק שגיאות? או שצריך לבדוק אותם כאן???
                        {
                            Console.Write("Enter Id Task For Engineer:");
                            string strIdTask = Console.ReadLine()??"";
                            int idTask = -1;
                            if (int.TryParse(strIdTask, out int resoultTaskId))
                            {
                                idTask = resoultTaskId;
                            };
                            //if (!int.TryParse(strIdTask, out idTask))
                            //    throw new FormatException("Wrong input");

                            Console.Write("Enter Alias Task For Engineer:");
                            string aliasTask = Console.ReadLine()??"";
                            //if (aliasTask == string.Empty)
                            //    throw new FormatException("Wrong input");

                            taskToUpdateEngineer = new()
                            {
                                Id = idTask,
                                Alias = aliasTask
                            };
                        }


                        BO.Engineer updateEngineer = new()
                        {
                            Id = idUpdate,
                            Name = NameUpdate,
                            Email = EmailUpdate,
                            Level = levelUpdate,
                            Cost = CostUpdate,
                            Task = taskToUpdateEngineer//לבדוק איך הפונקציה של update עובדת ואם היא לוקחת את הערך הזה?
                        };
                        try
                        {
                            s_bl.Engineer.UpdateEngineer(updateEngineer);
                            Console.WriteLine($"Engineer with id: {idUpdate} was succesfully updated!");
                        }
                        catch (BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "6":
                        Console.WriteLine("retuning to main");
                        return; // Returning to the main menu איך??
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void TaskMenu()
        {
            while (true)
            {
                Console.WriteLine("\nTask Menu:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Delete Task");
                Console.WriteLine("3. Get Task Details");
                Console.WriteLine("4. Get all Tasks");
                Console.WriteLine("5. Update Task");
                Console.WriteLine("6. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Task details:");
                        Console.Write("Enter Task Description: ");
                        string taskDescription = Console.ReadLine()??"";
                        Console.Write("Enter Task Alias: ");
                        string taskAlias = Console.ReadLine()??"";
                        Console.Write("Enter list of dependencies: ");
                        //the dependencies that this task is dependent on
                        List<BO.TaskInList> dependencies = new();
                        Console.WriteLine("Enter dependency id");
                        int dependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        while (dependencyId > 0)
                        {
                            BO.Task task = s_bl.Task.GetTask(dependencyId);
                            dependencies.Add(new BO.TaskInList()
                            {
                                Id = task.Id,
                                Description = task.Description,
                                Alias = task.Alias,
                                Status = task.Status//לשאול את יהודית אם הstatus יכול להיות nullגם במשימה וגם במשימה ברשימה
                            });
                            Console.WriteLine("enter another task, your task is dependent on it");
                            dependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        }
                        Console.Write("Enter Required Effort Time: ");
                        TimeSpan requiredEffortTime = TimeSpan.Parse(Console.ReadLine()??"7.00:00:00");

                        Console.Write("Enter deliverables: ");
                        string deliverables = Console.ReadLine()??"";
                        Console.Write("Enter Remarks: ");
                        string remarks = Console.ReadLine()??"";
                        Console.Write("Enter Engineer Id in task: ");
                        int engineerId = int.Parse(Console.ReadLine()??"-1");
                        BO.Engineer engineer = s_bl.Engineer.GetEngineer(engineerId);//לבדוק אם הוא בודק
                        BO.EngineerInTask? engineerInTask = null;
                        if (engineer != null)
                        {
                            try
                            {
                                engineerInTask = new BO.EngineerInTask()
                                {
                                    Id = engineerId,
                                    Name = engineer.Name!
                                };
                            }
                            catch (Exception)
                            {
                                engineerInTask = null;
                            }
                        }
                        Console.Write("Enter Complexity Level: ");
                        BO.EngineerExperience complexityLevel = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()??"0");//???
                        BO.Task newTask = new()
                        {
                            Id = 0,
                            Description = taskDescription,
                            Alias = taskAlias,
                            Status = (BO.Status)0,
                            Dependencies = dependencies,
                            Milestone = null,
                            CreatedAtDate = DateTime.Now,
                            StartDate = null,
                            SchedualDate = null,
                            Duration = requiredEffortTime,//?
                            ForecastDate = null,
                            DeadlineDate = null,
                            CompleteDate = null,
                            Deliverables = deliverables,
                            Remarks = remarks,
                            Engineer = engineerInTask,
                            ComplexityLevel = complexityLevel,
                        };
                        try
                        {
                            s_bl.Task.CreateTask(newTask);
                        }
                        catch (BO.BlAlreadyExistsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "2":
                        Console.Write("Enter Task ID to delete: ");
                        int taskIdToDelete = int.Parse(Console.ReadLine()??"-1");
                        try
                        {
                            s_bl.Task.DeleteTask(taskIdToDelete);
                        }
                        catch (BO.BlDeletionImpossible ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "3":
                        Console.Write("Enter Task ID to get details: ");
                        int taskIdToGetDetails = int.Parse(Console.ReadLine()??"-1");
                        try
                        {
                            var taskDetails = s_bl.Task.GetTask(taskIdToGetDetails);
                            Console.WriteLine($"Task Details:\n{taskDetails}");
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case "4":
                        var tasks = s_bl.Task.ReadAllTasks();
                        Console.WriteLine("All Tasks:");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine(task);
                        }
                        break;
                    case "5":
                        Console.WriteLine("Enter Task details to update:");
                        Console.Write("Enter Task ID: ");
                        int taskIdUpdate = int.Parse(Console.ReadLine()??"-1");//נראה לי עדיף לעשות trypars
                        BO.Task updateTask = s_bl.Task.GetTask(taskIdUpdate);
                        Console.Write("Enter Task Description: ");
                        updateTask.Description = Console.ReadLine()??"";
                        Console.Write("Enter Task Alias: ");
                        updateTask.Alias = Console.ReadLine()??"";
                        Console.Write("Enter Status: ");
                        updateTask.Status = (BO.Status)Enum.Parse(typeof(BO.Status), Console.ReadLine()??"0");//זה טוב?
                        List<BO.TaskInList> newDependencies = new();
                        Console.WriteLine("Enter dependency id");
                        int newDependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        while (newDependencyId > 0)
                        {
                            BO.Task task = s_bl.Task.GetTask(newDependencyId);
                            newDependencies.Add(new BO.TaskInList()
                            {
                                Id = task.Id,
                                Alias = task.Alias,
                                Description = task.Description,
                                Status = task.Status
                            });
                            Console.WriteLine("enter another task, your task is dependent on it");
                            newDependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        }
                        updateTask.Dependencies= newDependencies;
                        Console.Write("Enter Required Effort Time: ");
                        updateTask.Duration = TimeSpan.Parse(Console.ReadLine()??"7.00:00:00");
                        Console.Write("Enter Short Description for the Product: ");
                        updateTask.Deliverables = Console.ReadLine();
                        Console.Write("Enter Remarks: ");
                        updateTask.Remarks = Console.ReadLine();
                        Console.Write("Enter Engineer Id: ");
                        BO.Engineer newEngineer = s_bl.Engineer.GetEngineer(int.Parse(Console.ReadLine()??"-1"));
                        updateTask.Engineer = new EngineerInTask { Id = newEngineer.Id, Name = newEngineer.Name };
                        Console.Write("Enter Complexity Level: ");
                        updateTask.ComplexityLevel = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()??"0");//??
                        try
                        {
                            s_bl.Task.UpdateTask(updateTask);
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "6":
                        Console.WriteLine("returning to main");
                        return; // how returning to the main menu?

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }



        static void MilestoneMenu()
        {
            while (true)
            {
                Console.WriteLine("\nMilestone Menu:");
                Console.WriteLine("1. Create Project Schedule");
                Console.WriteLine("2. Get Milestone Details");
                Console.WriteLine("3. Update Milestone");
                Console.WriteLine("4. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine()??"";

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter start date and time (yyyy-MM-dd HH:mm:ss): ");
                        string? startInput = Console.ReadLine();
                        DateTime startDateTime;
                        while (!DateTime.TryParseExact(startInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
                        {
                            Console.WriteLine("Invalid input. Please enter the start date and time in the format yyyy-MM-dd HH:mm:ss.");
                            startInput = Console.ReadLine();
                        }

                        Console.Write("Enter end date and time (yyyy-MM-dd HH:mm:ss): ");
                        string? endInput = Console.ReadLine();
                        DateTime endDateTime;
                        while (!DateTime.TryParseExact(endInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
                        {
                            Console.WriteLine("Invalid input. Please enter the end date and time in the format yyyy-MM-dd HH:mm:ss.");
                            endInput = Console.ReadLine();
                        }

                        try
                        {
                            s_bl.Milestone.CreateSchedule(startDateTime, endDateTime);//לבדוק מה יהודית עשתה
                            Console.WriteLine("Project schedule created successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error in creating project schedule: {ex.Message}");
                        }
                        break;

                    case "2":

                        Console.Write("Enter Milestone ID: ");
                        if (int.TryParse(Console.ReadLine(), out int milestoneId))
                        {
                            try
                            {
                                var milestoneDetails = s_bl.Milestone.GetMilestone(milestoneId);
                                Console.WriteLine(milestoneDetails);
                                foreach (var dependency in milestoneDetails.Dependencies)
                                {
                                    Console.WriteLine("dependencies:");
                                    Console.WriteLine(dependency);
                                }

                            }
                            catch (BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid Milestone ID. Please enter a valid number.");
                        }

                        break;


                    case "3":

                        Console.Write("Enter Milestone ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int milestoneIdToUpdate))
                        {
                            try
                            {//במיילסטון אין את הtest נראה לי צריך לבדוק את זה.
                                BO.Milestone? existingMilestone = s_bl.Milestone.GetMilestone(milestoneIdToUpdate);
                                if (existingMilestone != null)
                                {
                                    // Get updated values from the user
                                    Console.Write("Enter new Description: ");
                                    string newDescription = Console.ReadLine()??"";
                                    Console.Write("Enter new Alias: ");
                                    string newAlias = Console.ReadLine()??"";
                                    Console.Write("Enter new Remarks: ");
                                    string newRemarks = Console.ReadLine()??"";
                                    string comments = "";//לשאול את יהודית למה צריך את זה הרי אין את זה בישות?!
                                    //// Update the milestone
                                    //existingMilestone.Description = newDescription;
                                    //existingMilestone.Alias = newAlias;
                                    //existingMilestone.Remarks = newRemarks;
                                    // Update the milestone in the data layer
                                    var updatedMilestone = s_bl.Milestone.UpdateMilestone(milestoneIdToUpdate, newAlias, newDescription, comments);
                                    Console.WriteLine("Milestone updated successfully!");
                                }
                            }
                            catch (BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Milestone ID. Please enter a valid number.");
                        }
                        break;
                    case "4":
                        Console.WriteLine("retuning to main");
                        return; // retuning to the main menu

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}