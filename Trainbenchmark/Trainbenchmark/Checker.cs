using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using VDS.RDF;
using VDS.RDF.Parsing;
using TrainBenchmarkTSLProject;

namespace Trainbenchmark
{
    class Checker
    {
        public void check()
        {
            if (routeSensor())
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Wrong model!");
            }
        }

        public bool routeSensor()
        {
            var routes = from r in Global.LocalStorage.Route_Accessor_Selector()
                         from swp in Global.LocalStorage.SwitchPosition_Accessor_Selector()
                         join sw in Global.LocalStorage.Switch_Accessor_Selector() on swp.target equals sw.CellID
                         from sensor in Global.LocalStorage.Sensor_Accessor_Selector()
                         where r.follows.Contains((int)swp.CellID) && sw.positions.Contains((int)sensor.CellID) &&
                                r.requires.Contains((int)sensor.CellID)
                         select r.CellID;
            if(routes.Count()>0)
                return false;
            return true;
        }

        public bool switchSet()
        {


            return false;
        }
    }
}
