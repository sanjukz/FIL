using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class Geolocation
    {
        public string latitude { get; set; }
        public string longitude { get; set; }

        public Geolocation()
        {
            this.latitude =string.Empty;
            this.longitude = string.Empty;
        }
    }
}
