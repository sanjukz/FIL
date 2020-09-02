using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class State
    {
        public int id { get; set; }
        public int stateMapId { get; set; }
        public string name { get; set; }
        public string urlSegment { get; set; }
        public string Country { get; set; }
        public int countryId { get; set; }
        public int countrymapId { get; set; }

    }

    public class StateList
    {
        public List<State> States { get; set; }
    }
}
