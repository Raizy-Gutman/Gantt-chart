namespace BO;

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base($"{message} dosn't exsit.") { }
    public BlDoesNotExistException(Exception innerException) : base(innerException.Message, innerException) { }
}

public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(Exception innerException) : base(innerException.Message, innerException) { }
}

public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message, Exception innerException) : base(message, innerException) { }
}

public class BlInvalidException : Exception
{
    public string InvalidPropertyName {  get; private set; }
    public BlInvalidException(string invalidPropertyName) : base($"Invalid property {invalidPropertyName}") { this.InvalidPropertyName = invalidPropertyName; }
}

public class BlNullException : Exception
{
    public string NullPropertyName { get; private set; }
    public BlNullException(string nullPropertyName) : base($"Null property {nullPropertyName}") { this.NullPropertyName = nullPropertyName; }
}

public class BlIllegalDeletionException : Exception
{
    public string DeleteEntityName { get; private set; }
    public BlIllegalDeletionException(string deleteEntityName) : base($"Illegal deletion of {deleteEntityName}") { this.DeleteEntityName = deleteEntityName; }
}

public class BlIllegalUpdateException : Exception
{
    public string updatePropertyName { get; private set; }
    public BlIllegalUpdateException(string UpdatePropertyName) : base($"Illegal update of {UpdatePropertyName}") { this.updatePropertyName = UpdatePropertyName; }
}