using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Imagination.Controllers;
using Imagination.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Imagination.Tests.Controllers;

public sealed class ImageControllerTests
{
    public ImageControllerTests()
    {
        var converter = Substitute.For<IImageConverter>();
        converter.ConvertAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Array.Empty<byte>()));

        _controller = new ImageController(converter)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Body = Stream.Null }
                }
            }
        };
    }

    private readonly ImageController _controller;

    [Fact]
    public async Task ShouldReturnFileContentResultOnConvertToJpeg()
    {
        var result = await _controller.ConvertToJpegAsync(CancellationToken.None).ConfigureAwait(false);

        result.ShouldNotBeNull();
        var fileContentResult = result as FileContentResult;
        fileContentResult.ShouldNotBeNull();
        fileContentResult.ContentType.ShouldBe("image/jpeg");
    }
}