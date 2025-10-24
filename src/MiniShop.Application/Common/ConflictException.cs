namespace MiniShop.Application.Common;

public  sealed class ConflictException : Exception
{
    public string? PropertyName { get; }

    public ConflictException(string message, string? propertyName = null) 
        : base(message)
    {
        PropertyName = propertyName;
    }
}