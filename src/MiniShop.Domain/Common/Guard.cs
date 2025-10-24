namespace MiniShop.Domain.Common;

public static class Guard
{
    public static string NotNullOrWhiteSpace(string? value, string property, int min = 1, int? max = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException($"{property} is required.", property);

        if (value.Length < min || (max.HasValue && value.Length > max.Value))
            throw new DomainValidationException($"{property} length must be {min}..{max}.", property);

        return value;
    }

    public static decimal NonNegativeMoney(decimal value, string property)
    {
        if (value < 0) throw new DomainValidationException($"{property} must be >= 0.", property);
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public static int NonNegative(int value, string property)
    {
        if (value < 0) throw new DomainValidationException($"{property} must be >= 0.", property);
        return value;
    }
}