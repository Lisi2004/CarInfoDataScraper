using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScraper
{
    public class RssService
    {
        private readonly AppDbContext _dbContext;

        public RssService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    public async Task ExecuteRss()
{
    string rssFeedUrl = "https://www.veturaneshitje.com/home/rss";
    var carUrls = await GetCarUrlsFromRssFeed(rssFeedUrl);

    var cars = new List<Car>();
    foreach (var carUrl in carUrls)
    {
        try
        {
            var car = await GetCarDetailsFromUrl(carUrl);
            if (car != null)
            {
                cars.Add(car);
            }
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Error processing URL {carUrl}: {ex.Message}");
        }
    }

    await SaveCarsToDb(cars);
}


    }
}
```
public async Task<List<string>> GetCarUrlsFromRssFeed(string rssFeedUrl)
{
    var carUrls = new List<string>();
    var httpClient = new HttpClient();
    var rssData = await httpClient.GetStringAsync(rssFeedUrl);

    var rssXml = XDocument.Parse(rssData);
    foreach (var item in rssXml.Descendants("item"))
    {
        var link = item.Element("link")?.Value?.Trim();
        if (!string.IsNullOrEmpty(link))
        {
            carUrls.Add(link);
        }
    }

    return carUrls;
}

```
