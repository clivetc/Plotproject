using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plot12API.Models
{
    public class Livestock
    {
        public int ID { get; set; }

        public string Cattle { get; set; }
        
        public string Pigs { get; set; }

        public string Goats { get; set; }

        public string Chicken { get; set; }

        public string Date { get; set; }

        public string BookedBy { get; set; }
        
    }
}
