namespace BO;

[Serializable]
public class DoesNotExistException : Exception
{
    public DoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class XMLFileLoadCreateException : Exception
{
    public XMLFileLoadCreateException(string? message) : base(message) { }
}