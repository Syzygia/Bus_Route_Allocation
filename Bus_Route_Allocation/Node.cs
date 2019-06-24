using System.Collections.Generic;

namespace Bus_Route_Allocation
{ 
    public class Station
    {
        public string Name;
        public List<(string bus, string station, double time)> neighbours;
    }
}