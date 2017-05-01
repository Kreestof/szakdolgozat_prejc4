﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using TrainBenchmarkTSLProject;
using VDS.RDF;
using VDS.RDF.Parsing;
using System.IO;
using Trinity.Network;

namespace Trainbenchmark
{
    class Validator
    {
        public int routeSensor()
        {
            int missingEdgeCount = 0;
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
                    using (var sw = Global.LocalStorage.UseSwitch(target))
                    {
                        foreach (var missingRequireEdge in sw.monitoredBy.Except(r.requires))
                        {
                            missingEdgeCount++;
                        }
                    }
                }

            }
            return missingEdgeCount;
        }

        public int switchSet()
        {
            int wrongPosition = 0;
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
                            {
                                wrongPosition++;
                            }
                        }
                    }
                }
            }
            return wrongPosition;
        }

    }
}
