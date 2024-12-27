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


    }
}
