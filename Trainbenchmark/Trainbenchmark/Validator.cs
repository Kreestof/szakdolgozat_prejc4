using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using TrainBenchmarkTSLProject;

namespace Trainbenchmark
{
    class Validator
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
            var routes = from r in Global.LocalStorage.Route_Selector()
                         select new { r.CellID, r.follows, r.requires };
            foreach (var r in routes)
            {
                foreach (var switchPositions in r.follows)
                {
                    var swp = Global.LocalStorage.LoadSwitchPosition(switchPositions);
                    var sw = Global.LocalStorage.LoadSwitch(swp.target);
                    var sensors = from s in Global.LocalStorage.Sensor_Selector()
                                  //where s.monitors.Contains<long>(sw.CellID)
                                  select new { s.CellID, s.monitors };
                    foreach (var s in sensors)
                    {
                        
                        if (s.monitors.Contains(sw.CellID) && !r.requires.Contains(s.CellID))
                            return false;
                    }

                }
            }
            return true;
        }

        public bool switchSet()
        {


            return false;
        }
    }
}
