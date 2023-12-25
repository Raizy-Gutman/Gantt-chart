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

        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(_id) != null);

            string _email = string.Join("", _name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)) + "@gmail.com";
            double _cost = s_rand.Next(5000, 15000);
            EngineerExperience _level = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dal!.Engineer.Create(newEngineer);
        }
    }

    private static void CraeteTask()
    {

        for (int i = 1; i < 31; i++)
        {
            int _id = 0;
            string _Description = "this is the " + i.ToString() + "task.";
            string _alias = "T" + i.ToString();
            bool _isMilestone = false;
            DateTime _createdAtDate = DateTime.Now;
            DateTime _schedualDate = _createdAtDate.AddDays(s_rand.Next(1, 7));
            DateTime _startDate = _schedualDate.AddDays(s_rand.Next(0, 3));
            TimeSpan _duration = new(s_rand.Next(1, 30), 0, 0, 0);
            DateTime _deadlineDate = (_startDate.Add(_duration)).AddDays(s_rand.Next(0, 14));
            DateTime? _completeDate = null;
            string _deliverables = "Deliverable of the " + i.ToString() + "task.";
            string? _remarks = null;
            EngineerExperience _complexityLevel = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            Engineer? e = s_dal!.Engineer.Read((e => e.Level <= _complexityLevel));
            DO.Task newTask = new(_id, _Description, _alias, _isMilestone, _createdAtDate, _startDate, _schedualDate, _duration, _deadlineDate, _completeDate, _deliverables, _remarks, e!.Id, _complexityLevel);
            s_dal!.Task.Create(newTask);
        }
    }


    private static void CraeteDependency()
    {
        for (int i = 0; i < 40; i++)
        {
            DO.Task _task = s_dal!.Task.Read(s_rand.Next(1, 31))!;
            int _taskNum = _task.Id;
            var optionalTasks = s_dal!.Task.ReadAll((t => t!.DeadlineDate + _task.Duration < _task.DeadlineDate));
            int _dependencyTask = 0;
            foreach (var task in optionalTasks)
            {
                if (s_dal!.Dependency.Read(d => d?.DependentTask == _taskNum && d?.DependsOnTask == task?.Id) == null)
                {
                    _dependencyTask = task!.Id;
                    break;
                }
            }
            if (_dependencyTask != 0)
            {
                Dependency d = new(0, _task.Id, _dependencyTask);
                s_dal!.Dependency.Create(d);
            }
            else
            {
                i--;
            }
        }
    }
}
