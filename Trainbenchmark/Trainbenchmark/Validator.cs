using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using TrainBenchmarkTSLProject;
using FanoutSearch.LIKQ;

namespace Trainbenchmark
{
    class Validator
    {
        public void check()
        {
            if (routeSensor() && switchSet())
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Wrong model!");
            }
        }

        public void test()
        {
            var semaphores = from s in Global.LocalStorage.Semaphore_Selector()
                             where s.signal == Signal.GO
                             select s.CellID;
            List<long> semaphoreIDs = semaphores.ToList();
            foreach (var semaphoreID in semaphoreIDs)
            {
                //KnowledgeGraph.StartFrom(semaphoreID)
                //    .FollowEdge("")
            }

        }

        public bool routeSensor()
        {
            //var routes = from r in Global.LocalStorage.Route_Selector()
            //             from swp in r.follows
            //             where swp.
            //             select new { r.CellID };
            //if (routes.FirstOrDefault() != null) return false;
            //return true;

            List<Route> rs = new List<Route>();
            var routes = from r in Global.LocalStorage.Route_Selector()
                         select new { r.CellID, r.follows, r.entry };
            foreach (var r in routes)
            {
                rs.Add(new Route(cell_id: r.CellID, follows: r.follows, entry: r.entry));
            }
            foreach (var r in rs)
            {
                bool semaphoreGo = true;
                using (var semaphore = Global.LocalStorage.UseSemaphore(r.entry))
                {
                    if (semaphore.signal != Signal.GO) semaphoreGo = false;
                }
                if (semaphoreGo)
                    foreach (var switchPositions in r.follows)
                    {
                        long target = -1;
                        using (var swp = Global.LocalStorage.UseSwitchPosition(switchPositions))
                        {
                            target = swp.target;
                        }
                        var sensors = from s in Global.LocalStorage.Sensor_Selector()
                                      where s.monitors.Contains(target)
                                      select s.CellID;
                        foreach (var s in sensors)
                        {
                            if (!r.requires.Contains(s))
                                return false;
                        }
                    }
            }

            //foreach (var r in routes)
            //{
            //    foreach (var switchPositions in r.follows)
            //    {
            //        var swp = Global.LocalStorage.LoadSwitchPosition(switchPositions);
            //        var sw = Global.LocalStorage.LoadSwitch(swp.target);




            //        var sensors = from s in Global.LocalStorage.Sensor_Selector()
            //                          //where s.monitors.Contains(sw.CellID)
            //                      select new { s.CellID, s.monitors };

            //        foreach (var s in sensors)
            //        {
            //            if (s.monitors.Contains(sw.CellID))
            //                using (var sensor = Global.LocalStorage.UseSensor(s.CellID))
            //                {

            //                    if (sensor.monitors.Contains(sw.CellID) && !r.requires.Contains(s.CellID))
            //                        return false;
            //                }
            //        }

            //    }
            //}
            return true;
        }

        public bool switchSet()
        {
            List<long> semaphoreIDs = new List<long>();
            var semaphores = from s in Global.LocalStorage.Semaphore_Selector()
                             where s.signal == Signal.GO
                             select s.CellID;
            semaphoreIDs = semaphores.ToList();
            foreach (var semaphoreID in semaphoreIDs)
            {
                List<List<long>> routeFollows = new List<List<long>>();
                var routes = from r in Global.LocalStorage.Route_Selector()
                             where r.entry == semaphoreID
                             select r.follows;
                routeFollows = routes.ToList();
                foreach (var switchPositionIDs in routeFollows)
                {
                    foreach (var swithcPositionID in switchPositionIDs)
                    {
                        SwitchPosition swP;
                        using (var switchPosition = Global.LocalStorage.UseSwitchPosition(swithcPositionID))
                        {
                            swP = new SwitchPosition(cell_id: swithcPositionID, position: switchPosition.position, target: switchPosition.target);
                        }
                        using (var sw = Global.LocalStorage.UseSwitch(swP.target))
                        {
                            if (sw.currentPosition != swP.position)
                                return false;
                        }
                    }
                }
            }


            //var routes = from r in Global.LocalStorage.Route_Selector()
            //             where r.active == true && r.entry == 
            //             select new { r.CellID, r.follows, r.requires };
            //foreach (var r in routes)
            //{
            //    foreach (var s in r.requires)
            //    {
            //        Console.WriteLine(Global.LocalStorage.LoadSensor(s));
            //    }
            //}

            return true;
        }
    }
}
