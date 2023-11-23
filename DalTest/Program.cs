using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEngineer, s_dalDependency, s_dalTask);
                //Console.WriteLine("Hello, World!");

                while (true)
                {
                    DisplayMainMenu();
                    string userChoice = Console.ReadLine();

                    if (userChoice == "0")
                    {
                        break; // Exit the program
                    }

                    ProcessUserChoice(userChoice);//צריך לממש בחירת משתמש
                }


                private static void DisplayMainMenu()
                {
                    Console.WriteLine("Main Menu:");
                    Console.WriteLine("1. Entity Engineer");
                    Console.WriteLine("2. Entity Dependency");
                    Console.WriteLine("3. Entity Task");
                    Console.WriteLine("0. Exit");
                    Console.Write("Enter your choice: ");
                }

                
            }
            catch (Exception ex) 
            {//או הראשון או השני
                //Console.WriteLine($"Initialization failed. Exception: {ex.Message}");
                Console.WriteLine(ex);
            }
        }
    }
}


    
        
    

