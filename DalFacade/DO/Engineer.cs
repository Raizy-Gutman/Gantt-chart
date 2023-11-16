namespace DO;

/// <summary>
/// The engineer entity contains the engineers' personal details and their employment details.
/// </summary>
/// <param name="id">Unique ID number of the engineer in the company.</param>
/// <param name="name">The engineer name</param>
/// <param name="email"> The engineer email</param>
/// <param name="Level">The experience and rank of the engineer</param>
/// <param name="cost">hourly wage</param>
/// 

public record Engineer
{
    public int Id;
    public string? Name = null;
    public string? Email = null;
    // public EngineerExperience? Level
    public double? Cost = null;

    Engineer() { this.Id=0; }
    Engineer(int id, string name, string email, double cost)
    {
        Id = id;
        Name = name;
        Email = email;
        Cost = cost;

    }

}
