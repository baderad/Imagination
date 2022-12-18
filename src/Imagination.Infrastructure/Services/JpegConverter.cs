using ImageMagick;

namespace Imagination.Infrastructure.Services;

internal sealed class JpegConverter : ImageConverter
{
    protected override int Quality => 100;
    protected override MagickFormat Foramt => MagickFormat.Jpeg;
}