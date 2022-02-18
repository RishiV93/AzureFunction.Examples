using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunction.Examples.Models
{
    public class ThemePark : EntityBase
    {
        public string Owner { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Ride> Rides { get; set; } = Enumerable.Empty<Ride>();
    }
}