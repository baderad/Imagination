namespace Imagination.Infrastructure.Services;

public interface IImageConverter
{
    Task<byte[]> ConvertAsync(Stream image, CancellationToken token);
}