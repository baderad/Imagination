using System.Diagnostics;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Imagination.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Imagination;

internal static class Program
{
    internal static readonly ActivitySource Telemetry = new("Server");

    private static void Main(string[] args)
    {
        Sdk.SetDefaultTextMapPropagator(new B3Propagator());
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .ConfigureContainer<ContainerBuilder>((_, builder) => builder.RegisterModule(new CommonModule()));
    }
}