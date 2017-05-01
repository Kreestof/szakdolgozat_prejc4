using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using TrainBenchmarkTSLProject;
using System.IO;

namespace Trainbenchmark
{


    class Modifier
    {
        public void routeSensorInject(int numberOfRoutes)
        {
            int i = 1;
            Random random = new Random();
            var routes = Global.LocalStorage.Route_Accessor_Selector();
            foreach (var route in routes)
            {
                if (route.requires.Count > 0)
                {
                    int removeableEdge = random.Next(0, route.requires.Count - 1);
                    route.requires.RemoveAt(removeableEdge);
                    if (++i > numberOfRoutes) break;
                }
            }
        }

        public void switchSetInject(int numberOfSwitches)
        {
            Random random = new Random();
            var switches = Global.LocalStorage.Switch_Accessor_Selector().Take(numberOfSwitches);
            foreach (var sw in switches)
            {
                sw.currentPosition = sw.currentPosition == Position.DIVERGING ? Position.FAILURE : sw.currentPosition + 1;
            }
        }

        public void routeSensorRepair()
        {
            List<Route> rs = new List<Route>();
            var routes = from r in Global.LocalStorage.Route_Selector()
                         select new { r.CellID, r.follows, r.requires };
            foreach (var r in routes)
            {
                rs.Add(new Route(cell_id: r.CellID, follows: r.follows, requires: r.requires));
            }
            foreach (var r in rs)
            {
                foreach (var switchPositionID in r.follows)
                {
                    long target;
                    using (var switchPosition = Global.LocalStorage.UseSwitchPosition(switchPositionID))
                    {
                        target = switchPosition.target;
                    }
                    List<long> missingRequireEdges = new List<long>();
                    using (var sw = Global.LocalStorage.UseSwitch(target))
                    {
                        missingRequireEdges = sw.monitoredBy.Except(r.requires).ToList();
                    }
                    using (var route = Global.LocalStorage.UseRoute(r.CellID))
                    {
                        foreach (var missingRequireEdge in missingRequireEdges)
                        {
                            route.requires.Add(missingRequireEdge);
                        }
                    }
                }
            }
        }

        public void switchSetRepair()
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
                            if(sw.currentPosition!= swP.position)
                            {
                                sw.currentPosition = swP.position;
                            }
                        }
                    }
                }
            }
        }
    }
}
