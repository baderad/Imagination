using Autofac;
using Autofac.Extensions.DependencyInjection;
using Imagination.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Imagination.Infrastructure;

public sealed class CommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IImageConverter, JpegConverter>();

        builder.Populate(services);
    }
}