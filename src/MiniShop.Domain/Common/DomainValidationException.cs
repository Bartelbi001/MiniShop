namespace MiniShop.Domain.Common;

public sealed class DomainValidationException : Exception
{
    public string? PropertyName { get; }

    public DomainValidationException(string message, string? propertyName = null)
        : base(message)
    {
        PropertyName = propertyName;
    }
}