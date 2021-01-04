﻿using Kuli;
using Kuli.Configuration;
using Kuli.Importing;
using Kuli.Rendering;
using Markdig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var loggingLevelSwitch = new LoggingLevelSwitch();

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        config.AddYamlFile("config.yml");
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .CreateLogger();
        
        logging.AddSerilog(logger);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(loggingLevelSwitch);
        
        services.AddSingleton<FragmentDiscoveryService>();
        services.AddSingleton<RawFragmentImporterService>();
        services.AddSingleton<FragmentImportService>();
        services.AddSingleton<TemplateImportService>();
        services.AddSingleton<GlobalContextService>();
        services.AddSingleton<FragmentRenderer>();
        services.AddSingleton<RenderResultWriter>();
        services.AddSingleton<SiteRenderer>();
        
        services.AddOptions<LoggingOptions>().BindConfiguration(LoggingOptions.Section);
        services.AddOptions<DirectoryDiscoveryOptions>().BindConfiguration(DirectoryDiscoveryOptions.Section);
        services.AddHostedService<KuliExecutionService>();

        services.AddSingleton(new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build());
        services.AddSingleton(new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
    });

hostBuilder.Build().Run();