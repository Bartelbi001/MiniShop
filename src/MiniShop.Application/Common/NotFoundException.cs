namespace MiniShop.Application.Common;

public sealed class NotFoundException : Exception
{
    public string? Resource { get; }
    public string? Key { get; }

    public NotFoundException(string message, string? resource = null, string? key = null)
        : base(message)
    {
        Resource = resource;
        Key = key;
    }
}