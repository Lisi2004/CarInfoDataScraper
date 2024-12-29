using DataScraper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataScraper;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"./appsettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DevConnection")))
            .AddTransient<RssService>()
            .BuildServiceProvider();

        var rssService = serviceProvider.GetRequiredService<RssService>();
        await rssService.ExecuteRss();
    }
}

