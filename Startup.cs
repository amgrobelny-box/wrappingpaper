using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BoxyQL.BoxPlatform.BoxCache;
using BoxyQL.BoxPlatform.Service;
using BoxyQL.GraphQL;
using BoxyQL.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BoxyQL
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //Work around for issue https://github.com/dotnet/corefx/issues/8768
      string redisHost = Configuration["Redis:Host"],
        redisPort = Configuration["Redis:Port"],
        redisPassword = Configuration["Redis:Password"],
        redisInstance = Configuration["Redis:InstanceName"];

      var dnsTask = Dns.GetHostAddressesAsync(redisHost);
      var addresses = dnsTask.Result;
      string connection;
      if (string.IsNullOrWhiteSpace(redisPassword))
      {
        connection = string.Join(",", addresses.Select(x => x.MapToIPv4().ToString() + ":" + redisPort));
      }
      else
      {
        connection = string.Join(",", addresses.Select(x => x.MapToIPv4().ToString() + ":" + redisPort + ",password=" + redisPassword));
      }
      //End work around for issue https://github.com/dotnet/corefx/issues/8768

      services.Configure<BoxPlatformSettings>(Configuration.GetSection("BoxPlatform"));

      services.AddMemoryCache();
      services.AddDistributedRedisCache(options =>
      {
        options.Configuration = connection;
        options.InstanceName = redisInstance;
      });

      // Add framework services.
      services.AddMvc();

      // Add functionality to inject IOptions<T>
      services.AddOptions();

      services.AddSingleton<IBoxPlatformCache, BoxPlatformCacheImplementation>();
      services.AddSingleton<IBoxPlatformService, BoxPlatformService>();
      services.AddTransient<GraphQLUserContext>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseMvc();
    }
  }
}
