namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;


public static class Initialization
{
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;
    private static ITask? s_dalTask;

    private static readonly Random s_rand = new();
    private static void craeteEngineers()
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
            while (s_dalEngineer!.Read(_id) != null);

            string? _email = string.Join("", _name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)) + "@gmail.com";
            double? _cost = s_rand.Next(5000, 15000);  
            EngineerExperience _level = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dalEngineer!.Create(newEngineer);

        }
    }

    //private static craeteTask()
    //{
    //    

    //}


    //private static void craeteDependency()
    //{

    //    int Id,
    //    int DependentTask,
    //    int DependsOnTask
    //    לפחות 20 משימות
    //}






}
