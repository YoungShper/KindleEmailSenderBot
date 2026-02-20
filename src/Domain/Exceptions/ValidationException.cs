using System.Diagnostics.CodeAnalysis;

namespace KindleEmailSenderBot.Domain;

public class ValidationException : Exception
{
    public ValidationException(string? message, Exception innerException) 
        : base(message, innerException)
    {
        
    }
    public ValidationException(string? message) 
        : base(message)
    {
        
    }

    public static T ThrowIfNull<T>([NotNull]T? value, string message) where T : class =>
        value ?? throw new ValidationException(message);
    
    public static T ThrowIfNull<T>([NotNull]T? value, string message) where T : struct =>
        value ?? throw new ValidationException(message);
}