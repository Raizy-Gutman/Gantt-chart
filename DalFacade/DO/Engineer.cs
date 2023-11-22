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
(
    int Id,
    string? Name,
    string? Email,
    EngineerExperience Level,
    double? Cost
)
{
    public Engineer() : this(0, "", "", 0, 0.0) { }  //empty ctor
}
