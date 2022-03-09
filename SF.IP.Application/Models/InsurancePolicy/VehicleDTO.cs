using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models.InsurancePolicy
{
    public class VehicleDTO
    {   
        public string Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
    }
}
