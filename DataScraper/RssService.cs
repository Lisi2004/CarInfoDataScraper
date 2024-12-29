using HtmlAgilityPack;
using System.Web;
using System.Xml.Linq;

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
            Console.WriteLine("Starting RSS execution...");
            string rssFeedUrl = "https://www.veturaneshitje.com/home/rss";

            Console.WriteLine($"Fetching car URLs from RSS feed: {rssFeedUrl}");
            var carUrls = await GetCarUrlsFromRssFeed(rssFeedUrl);
            Console.WriteLine($"Fetched {carUrls.Count} car URLs.");

            var cars = new List<Car>();
            foreach (var carUrl in carUrls)
            {
                try
                {
                    Console.WriteLine($"Processing URL: {carUrl}");
                    var car = await GetCarDetailsFromUrl(carUrl);
                    if (car != null)
                    {
                        Console.WriteLine($"Successfully retrieved details for car: {car.Title}");
                        cars.Add(car);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve car details for URL: {carUrl}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing URL {carUrl}: {ex.Message}");
                }
            }

            Console.WriteLine("Saving car details to database...");
            await SaveCarsToDb(cars);
            Console.WriteLine("Finished saving car details.");
        }

        public async Task<List<string>> GetCarUrlsFromRssFeed(string rssFeedUrl)
        {
            var carUrls = new List<string>();
            try
            {
                Console.WriteLine($"Fetching RSS data from: {rssFeedUrl}");
                var httpClient = new HttpClient();
                var rssData = await httpClient.GetStringAsync(rssFeedUrl);

                Console.WriteLine("Parsing RSS feed...");
                var rssXml = XDocument.Parse(rssData);
                foreach (var item in rssXml.Descendants("item"))
                {
                    var link = item.Element("link")?.Value?.Trim();
                    if (!string.IsNullOrEmpty(link))
                    {
                        carUrls.Add(link);
                    }
                }

                Console.WriteLine($"Parsed {carUrls.Count} URLs from RSS feed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching or parsing RSS feed: {ex.Message}");
            }

            return carUrls;
        }

        public async Task<Car> GetCarDetailsFromUrl(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to retrieve data from URL: {url}. Status code: {response.StatusCode}");
                    return null;
                }

                var html = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var carNode = htmlDocument.DocumentNode.SelectSingleNode("//table[contains(@class, 'table-car-specifications')]");

                if (carNode != null)
                {

                    var title = HttpUtility.HtmlDecode(htmlDocument.DocumentNode.SelectSingleNode("//h1[contains(@class, 'no-margin')]")?.InnerText.Trim());
                    var price = HttpUtility.HtmlDecode(htmlDocument.DocumentNode.SelectSingleNode("//h3[contains(@class, 'text-orange price lead no-margin')]/strong")?.InnerText.Trim());
                    var customs = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//td[contains(., 'Dogana')]/../following-sibling::tr[1]/td[1]")?.InnerText.Trim());
                    var registration = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//td[contains(., 'Dogana')]/../following-sibling::tr[1]/td[2]")?.InnerText.Trim());
                    var vehicleType = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Viti']/../../following-sibling::tr[1]/td[1]")?.InnerText.Trim());
                    var year = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Viti']/../../following-sibling::tr[1]/td[2]")?.InnerText.Trim());
                    var fuelType = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Karburanti']/../../following-sibling::tr[1]/td[1]")?.InnerText.Trim());
                    var transmission = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Marshi']/../../following-sibling::tr[1]/td[2]")?.InnerText.Trim());
                    var color = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Ngjyra']/../../following-sibling::tr[1]/td[1]")?.InnerText.Trim());
                    var seats = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Ulese']/../../following-sibling::tr[1]/td[2]")?.InnerText.Trim());
                    var engineSize = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Kubikazha']/../../following-sibling::tr[1]/td[1]")?.InnerText.Trim());
                    var mileage = HttpUtility.HtmlDecode(carNode.SelectSingleNode(".//span[text()='Kilometrazhi']/../../following-sibling::tr[1]/td[2]")?.InnerText.Trim());


                    var linkId = ExtractIdFromUrl(url);

                    return new Car
                    {
                        Title = title,
                        Price = price,
                        Customs = customs,
                        Registration = registration,
                        VehicleType = vehicleType,
                        Year = year,
                        FuelType = fuelType,
                        Transmission = transmission,
                        Color = color,
                        Seats = seats,
                        EngineSize = engineSize,
                        Mileage = mileage,

                        LinkId = linkId
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching data from URL: {url}");
                Console.WriteLine($"Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            return null;
        }

        public async Task SaveCarsToDb(List<Car> cars)
        {
            try
            {
                _dbContext.Cars.AddRange(cars);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cars to the database: {ex.Message}");
            }
        }

        private string ExtractIdFromUrl(string url)
        {

            var segments = url.Split('/');


            if (segments.Length >= 4)
            {
                return segments[4];
            }
            else if (segments.Length < 4)
            {
                return segments[3];
            }

            return string.Empty;
        }
    }
}
