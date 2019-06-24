using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Route_Allocation
{
    class Path
    {
        public Station station;
        public Station parent;
        public string bus;
        public double Distance;
        public bool Is_used;
    }
    class Find_path
    {
        public static (List<String>, double) Route(string from, string to, Dictionary <string,Station> stations, Dictionary<string,double> waiting_time)
        {
            var dist = new Dictionary<string, Path>();

            foreach (var s in stations.Values)
            {
                var p = new Path();
                p.station = s;
                p.parent = null;
                p.Is_used = false;
                p.Distance = Double.PositiveInfinity;
                if (s.Name == from)
                {
                    p.Distance = 0;
                }
                dist.Add(s.Name, p);
            }

            while (true)
            {                
                var p = dist.Values.Where(x => !x.Is_used).OrderByDescending(x => x.Distance).Last();
               
                foreach ((string bus, string n, double time) in p.station.neighbours)
                {
                    if (((bus == p.bus)? 0 : waiting_time[bus]) + time + p.Distance < dist[n].Distance)
                    {
                        dist[n].parent = p.station;
                        dist[n].bus = bus;
                        dist[n].Distance = ((bus == p.bus) ? 0 : waiting_time[bus]) + time + p.Distance;
                    }
                }
                p.Is_used = true;
                if (p.station.Name == to)
                {
                    break;
                }
            }
            var res = new List<String>();
            var v = to;
            while (v != from)
            {
                if (dist[v].bus != dist[dist[v].parent.Name].bus)
                {
                    res.Add(v);
                }
                v = dist[v].parent.Name;
            }
            res.Reverse();
            return (res, dist[to].Distance);
        }
    }
}
