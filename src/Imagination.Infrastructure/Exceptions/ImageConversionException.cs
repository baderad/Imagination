namespace Imagination.Infrastructure.Exceptions;

public sealed class ImageConversionException : Exception
{
    public ImageConversionException(string message) : base(message)
    {
    }
}