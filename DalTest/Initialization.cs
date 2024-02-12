namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

public static class Initialization
{
    private static IDal? s_dal; //stage 2
    //public static void Do(IDal dal)
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        //s_dal = DalApi.Factory.Get; //stage 4
        s_dal = Factory.Get; //stage 4

        CraeteEngineers();
        CraeteTask();
        CraeteDependency();

    }

    private static readonly Random s_rand = new();
    private static void CraeteEngineers()
    {
        //s_dal?.Engineer.Reset();
        string[] engineerNames =
        {
            "Raizy Gutman", "Yeudit Itamar", "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        //variable for engineer level, so that engineers of all levels will be created.
        int level = 0;
        for (int i = 0; i < engineerNames.Length; i++)
        {
            int _id = s_rand.Next(200000000, 400000000);
            string _email = engineerNames[i].Replace(" ", "") + "@gmail.com";
            double _cost = s_rand.Next(300, 700);
            EngineerExperience _level = (EngineerExperience)((level++) % 5);
            Engineer newEngineer = new(_id, engineerNames[i], _email, _level, _cost);
            try
            {
                s_dal!.Engineer.Create(newEngineer);
            }
            catch (DalAlreadyExistsException)
            {
                i--;
            }
        }
    }

    private static void CraeteTask()
    {
        //s_dal?.Task.Reset();
        DateTime currentDate = DateTime.Now;
        for (int i = 1; i < 31; i++)
        {
            int _id = 0;
            string _Description = "this is the " + "task number " + i.ToString();
            string _alias = "T" + i.ToString();
            bool _isMilestone = false;
            DateTime _createdAtDate = new(currentDate.Year, currentDate.Month, currentDate.Day);
            DateTime _schedualDate = _createdAtDate.AddDays(s_rand.Next(1, 31));
            DateTime _startDate = _schedualDate.AddDays(s_rand.Next(0, 3));
            TimeSpan _duration = new(s_rand.Next(1, 15), 0, 0, 0);
            DateTime _deadlineDate = (_startDate.Add(_duration)).AddDays(s_rand.Next(0, 14));
            string _deliverables = "Deliverable of the " + i.ToString() + " task.";
            EngineerExperience _complexityLevel = (EngineerExperience)s_rand.Next(0, 5);
            //Finding an engineer that fits the difficulty level of the task.
            Engineer? e = s_dal!.Engineer.Read(e => e!.Level == _complexityLevel);
            DO.Task newTask = new(_id, _Description, _alias, _isMilestone, _createdAtDate, _startDate, _schedualDate, _duration, _deadlineDate, null, _deliverables, null, e!.Id, _complexityLevel);
            s_dal!.Task.Create(newTask);
        }
    }

    private static void CraeteDependency()
    {
        //s_dal?.Dependency.Reset();
        int i = 0;
        do
        {
            DO.Task _task;
            do { _task = s_dal!.Task.Read(s_rand.Next(1, 31))!; } while (_task is null);
            int _taskNum = _task!.Id;
            var optionalTasks = s_dal!.Task.ReadAll().ToList();
            //A variable that limits the number of dependencies for _task.
            int countOfDependecies = 0;
            foreach (var task in optionalTasks)
            {
                //Checking whether this dependency has already been created in the past
                if (s_dal!.Dependency.Read(d => d?.DependentTask == _taskNum && d?.DependsOnTask == task?.Id) == null)
                {
                    //Conversion of the relevant dates from nullable to normal type in order to run + and Compare on them
                    DateTime taskDeadline = (DateTime)task!.DeadlineDate!;
                    DateTime dependentDeadline = (DateTime)_task.DeadlineDate!;
                    TimeSpan dependentDuration = (TimeSpan)_task.Duration!;
                    if (DateTime.Compare(taskDeadline + dependentDuration, dependentDeadline) < 0)
                    {
                        Dependency d = new(0, _task.Id, task.Id);
                        s_dal!.Dependency.Create(d);
                        i++;
                        if (++countOfDependecies >= 3) break;
                    }
                }
            }
        } while (i < 40);
    }
}
