using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScraper
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Customs { get; set; }
        public string Registration { get; set; }
        public string VehicleType { get; set; }
        public string Year { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public string Color { get; set; }
        public string Seats { get; set; }
        public string EngineSize { get; set; }
        public string Mileage { get; set; }
        public string LinkId { get; set; }
    }
}
