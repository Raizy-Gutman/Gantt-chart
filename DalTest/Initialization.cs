namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Xml.Linq;


public static class Initialization
{
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;
    private static ITask? s_dalTask;

    public static void Do(IEngineer? dalEngineer, IDependency? dalDependency, ITask? dalTask)
    {
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        craeteEngineers();
        craeteTask();
        craeteDependency();
    }

    private static readonly Random s_rand = new();
    private static void craeteEngineers()
    {//לפחות 5 מהנדסים

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
            while (s_dalEngineer!.Read(_id) != null);

            string? _email = string.Join("", _name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)) + "@gmail.com";
            double? _cost = s_rand.Next(5000, 15000);
            EngineerExperience _level = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dalEngineer!.Create(newEngineer);
        }
    }

    private static void craeteTask()
    {//לפחות 20 משימות

        for (int i = 0; i < 30; i++)
        {
            int _id = 0;
            string? _Description = "this is the " + i.ToString()+ "task.";
            string? _alias = "T" + i.ToString();
            bool _isMilestone = false;
            int days = s_rand.Next(1, 28);
            int month = s_rand.Next(1, 12);
            int year = s_rand.Next(2021, 2023);
            DateTime? _createdAtDate = new DateTime(year, month, days);
            DateTime? _startDate = null;
            DateTime? _schedualDate = null;
            DateTime? _forecastDate = null;
            DateTime? _deadlineDate = null;
            DateTime? _completeDate = null;
            string? _deliverables = "Deliverable of the " + i.ToString() + "task.";
            string? _remarks = null;
            int _engineerId = 0;//ללא הקצאת מהנדס?!
            EngineerExperience _complexityLevel = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            Task newTask = new(_id, _Description, _alias, _isMilestone, _createdAtDate, _startDate, _schedualDate, _forecastDate, _deadlineDate, _completeDate, _deliverables, _remarks, _engineerId, _complexityLevel);
            s_dalTask!.Create(newTask);
        }
    }


    private static void craeteDependency()
    {//לפחות 40 תלויות

        int _id = 0;// ללא הקצאת ID??
        for(int i = 0; i < 30; i++)
        {
            int _dependentTask = i;
            for (int j = 0; j < i; j++)
            {
                int DependsOnTask = j;
                Dependency newDependency = new(_id, _dependentTask, DependsOnTask);
                s_dalDependency!.Create(newDependency);
            }
        }
    }
}
