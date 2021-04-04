using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeMonitor.Models
{
    public class BoatModel
    {
        public List<Boat> BoatsBefore { get; set; }
        public List<Boat> BoatsAfter { get; set; }
    }
}