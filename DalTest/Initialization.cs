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
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        CraeteEngineers();
        CraeteTask();
        CraeteDependency();

    }

    private static readonly Random s_rand = new();
    private static void CraeteEngineers()
    {
        string[] engineerNames =
        {
            "Raizy Gutman", "Yeudit Itamar", "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        int level = 0;
        foreach (var _name in engineerNames)
        {
            int _id = s_rand.Next(200000000, 400000000);
            string _email = _name.Replace(" ", "") + "@gmail.com";
            double _cost = s_rand.Next(5000, 15000);
            EngineerExperience _level = (EngineerExperience)((level++) % 5);
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            try
            {
                s_dal!.Engineer.Create(newEngineer);
            }
            catch (DalAlreadyExistsException)
            {
                continue;
            }
        }
    }

    private static void CraeteTask()
    {
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
            TimeSpan _duration = new(s_rand.Next(1, 15), 0, 0);
            DateTime _deadlineDate = (_startDate.Add(_duration)).AddDays(s_rand.Next(0, 14));
            DateTime? _completeDate = null;
            string _deliverables = "Deliverable of the " + i.ToString() + " task.";
            string? _remarks = null;
            EngineerExperience _complexityLevel = (EngineerExperience)s_rand.Next(0, 5);
            Engineer? e = s_dal!.Engineer.Read(e => e!.Level == _complexityLevel);
            DO.Task newTask = new(_id, _Description, _alias, _isMilestone, _createdAtDate, _startDate, _schedualDate, _duration, _deadlineDate, _completeDate, _deliverables, _remarks, e!.Id, _complexityLevel);
            s_dal!.Task.Create(newTask);
        }
    }


    private static void CraeteDependency()
    {
        int i = 0;
        do
        {
            DO.Task _task = s_dal!.Task.Read(s_rand.Next(1, 31))!;
            int _taskNum = _task.Id;
            var optionalTasks = s_dal!.Task.ReadAll().ToList();
            int _dependencyTask = 0;
            foreach (var task in optionalTasks)
            {
                if (s_dal!.Dependency.Read(d => d?.DependentTask == _taskNum && d?.DependsOnTask == task?.Id) == null)
                {
                    DateTime taskDeadline = (DateTime)task!.DeadlineDate!;
                    DateTime dependentDeadline = (DateTime)_task.DeadlineDate!;
                    TimeSpan dependentDuration = (TimeSpan)_task.Duration!;
                    if (DateTime.Compare(taskDeadline + dependentDuration, dependentDeadline) < 0)
                    {
                        _dependencyTask = task!.Id;
                        Dependency d = new(0, _task.Id, _dependencyTask);
                        s_dal!.Dependency.Create(d);
                        if (++i == 40) break;
                    }
                }
            }
        } while (i < 40);
    }
}
