using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Imagination.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imagination.Controllers;

[ApiController]
[Route("/")]
public sealed class ImageController : ControllerBase
{
    private readonly IImageConverter _converter;

    public ImageController(IImageConverter converter)
    {
        _converter = converter;
    }

    [HttpPost]
    [Route("convert")]
    public async Task<IActionResult> ConvertToJpegAsync(CancellationToken token)
    {
        await using var inputImageStream = new MemoryStream();
        await Request.Body.CopyToAsync(inputImageStream, token);
        inputImageStream.Seek(0, SeekOrigin.Begin);

        var convertedImage = await _converter.ConvertAsync(inputImageStream, token);

        return File(convertedImage, MediaTypeNames.Image.Jpeg);
    }
}