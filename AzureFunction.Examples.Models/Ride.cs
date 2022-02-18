using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunction.Examples.Models
{
    public class Ride : EntityBase
    {
        public int NumberOfCarraiges { get; set; }
        public RideType RideType { get; set; }
    }
}