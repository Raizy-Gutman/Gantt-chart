namespace BO;

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(Exception innerException) : base(innerException.Message, innerException) { }
}

public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message, Exception innerException) : base(message, innerException) { }
}
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message, Exception innerException) : base(message, innerException) { }
}

public class BlInvalidException : Exception
{
    public string InvalidPropertyName {  get; private set; }
    public BlInvalidException(string InvalidPropertyName) : base($"Invalid property {InvalidPropertyName}") { this.InvalidPropertyName = InvalidPropertyName; }
}

public class BlNullException : Exception
{
    public string NullPropertyName { get; private set; }
    public BlNullException(string NullPropertyName) : base($"Null property {NullPropertyName}") { this.NullPropertyName = NullPropertyName; }
}