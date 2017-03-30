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
    class RDFReader
    {
        public void batch()
        {
            TrinityConfig.CurrentRunningMode = RunningMode.Embedded;
            IGraph g = new Graph();
            TurtleParser ttlparser = new TurtleParser();
            //El kell helyezni az állományt!
            ttlparser.Load(g, @"c:\Kristof\BME\szakdolgozat\Trainbenchmark\Trainbenchmark\sources\railway-batch-1-inferred.ttl");

            var typeNode = g.CreateUriNode("rdf:type");
            var triples = g.GetTriplesWithPredicate(typeNode);
            foreach (var b in triples)
            {
                switch (((UriNode)b.Object).Uri.Fragment)
                {
                    case "#Semaphore":
                        int semaphoreId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        Signal semaphoreSignal = Signal.FAILURE;
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            if (triple.Predicate.ToString().Split('#')[1] == "signal")
                            {
                                semaphoreSignal = getSignalFromString(((UriNode)triple.Object).Uri.Fragment.Substring(1));
                            }
                        }
                        Semaphore semaphore = new Semaphore(cell_id: semaphoreId, signal: semaphoreSignal);
                        Global.LocalStorage.SaveSemaphore(semaphore);
                        break;
                    case "#Route":
                        int routeId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Split('_')[1]);
                        bool routeActive = false;
                        int routeEntry = -1;
                        int routeExit = -1;
                        List<long> routeRequires = new List<long>();
                        List<long> routeFollows = new List<long>();
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            switch (triple.Predicate.ToString().Split('#')[1])
                            {
                                case "active":
                                    routeActive = ((LiteralNode)triple.Object).Value == "true";
                                    break;
                                case "entry":
                                    routeEntry = int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2));
                                    break;
                                case "exit":
                                    routeExit = int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2));
                                    break;
                                case "requires":
                                    routeRequires.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                case "follows":
                                    routeFollows.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                default:
                                    break;
                            }
                        }
                        Route route = new Route(cell_id: routeId, requires: routeRequires, follows: routeFollows, entry: routeEntry, exit: routeExit, active: routeActive);
                        Global.LocalStorage.SaveRoute(route);
                        break;
                    case "#Region":
                        int regionId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        List<long> regionSensors = new List<long>();
                        List<long> regionElements = new List<long>();
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            switch (triple.Predicate.ToString().Split('#')[1])
                            {
                                case "sensors":
                                    regionSensors.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                case "elements":
                                    regionElements.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                default:
                                    break;
                            }
                        }
                        Region region = new Region(cell_id: regionId, sensors: regionSensors, elements: regionElements);
                        Global.LocalStorage.SaveRegion(region);
                        break;
                    case "#Switch":
                        int switchId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        Position switchCurrentPosition = Position.FAILURE;
                        List<long> switchMonitoredBy = new List<long>();
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            switch (triple.Predicate.ToString().Split('#')[1])
                            {
                                case "currentPosition":
                                    // az Id típusú mezőknél 2 karaktert kell levágni, mert "#_" kezdődik, itt viszont csak a '#' lesz benne
                                    switchCurrentPosition = getPositionFromString(((UriNode)triple.Object).Uri.Fragment.Substring(1));
                                    break;
                                case "monitoredBy":
                                    switchMonitoredBy.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                default:
                                    break;
                            }
                        }
                        Switch sw = new Switch(cell_id: switchId, currentPosition: switchCurrentPosition, monitoredBy: switchMonitoredBy);
                        Global.LocalStorage.SaveSwitch(sw);
                        break;
                    case "#Sensor":
                        int sensorId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        Sensor sensor = new Sensor(cell_id: sensorId);
                        Global.LocalStorage.SaveSensor(sensor);
                        break;
                    case "#Segment":
                        int segmentId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        int segmentLength = -1;
                        List<long> segmentSemaphores = new List<long>();
                        List<long> segmentMonitoredBy = new List<long>();
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            switch (triple.Predicate.ToString().Split('#')[1])
                            {
                                case "length":
                                    segmentLength = int.Parse(((LiteralNode)triple.Object).Value);
                                    break;
                                case "semaphores":
                                    segmentSemaphores.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                case "monitoredBy":
                                    segmentMonitoredBy.Add(int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2)));
                                    break;
                                default:
                                    break;
                            }
                        }
                        Segment segment = new Segment(cell_id: segmentId, length: segmentLength, semaphores: segmentSemaphores, monitoredBy: segmentMonitoredBy);
                        Global.LocalStorage.SaveSegment(segment);
                        break;
                    case "#SwitchPosition":
                        int switchPositionId = int.Parse(((UriNode)b.Subject).Uri.Fragment.Substring(2));
                        Position switchPositionPosition = Position.FAILURE;
                        int switchPositionRoute = -1;
                        int switchPositionTarget = -1;
                        foreach (var triple in g.GetTriplesWithSubject(b.Subject))
                        {
                            switch (triple.Predicate.ToString().Split('#')[1])
                            {
                                case "position":
                                    switchPositionPosition = getPositionFromString(((UriNode)triple.Object).Uri.Fragment.Substring(1));
                                    break;
                                case "route":
                                    switchPositionRoute = int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2));
                                    break;
                                case "target":
                                    switchPositionTarget = int.Parse(((UriNode)triple.Object).Uri.Fragment.Substring(2));
                                    break;
                                default:
                                    break;
                            }
                        }
                        SwitchPosition switchPosition = new SwitchPosition(cell_id: switchPositionId, position: switchPositionPosition, route: switchPositionRoute, target: switchPositionTarget);
                        Global.LocalStorage.SaveSwitchPosition(switchPosition);
                        break;
                    case "#TrackElement":
                        //egyelőre
                        break;
                    default:
                        Console.WriteLine("Error: unknown type: " + ((UriNode)b.Object).Uri.Fragment);
                        break;
                }
            }
            Global.LocalStorage.SaveStorage();
        }

        public static Signal getSignalFromString(string s)
        {
            switch (s)
            {
                case "SIGNAL_FAILURE":
                    return Signal.FAILURE;
                case "SIGNAL_STOP":
                    return Signal.STOP;
                case "SIGNAL_GO":
                    return Signal.GO;
                default:
                    return Signal.FAILURE;
            }
        }

        public static Position getPositionFromString(string s)
        {
            switch (s)
            {
                case "POSITION_FAILURE":
                    return Position.FAILURE;
                case "POSITION_STATE":
                    return Position.STATE;
                case "POSITION_DIVERGING":
                    return Position.DIVERGING;
                default:
                    return Position.FAILURE;
            }
        }

    }
}
