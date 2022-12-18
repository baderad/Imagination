using ImageMagick;
using Imagination.Infrastructure.Exceptions;

namespace Imagination.Infrastructure.Services;

internal abstract class ImageConverter : IImageConverter
{
    protected abstract int Quality { get; }
    protected abstract MagickFormat Foramt { get; }

    public async Task<byte[]> ConvertAsync(Stream image, CancellationToken token)
    {
        try
        {
            using var magickImage = new MagickImage
            {
                Quality = Quality
            };

            await magickImage.ReadAsync(image, token).ConfigureAwait(false);

            await using var outputStream = new MemoryStream();
            await magickImage.WriteAsync(outputStream, Foramt, token).ConfigureAwait(false);

            return outputStream.ToArray();
        }
        catch (MagickErrorException ex)
        {
            throw new ImageConversionException($"Image conversion failed (message={ex.Message})");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}