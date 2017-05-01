using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trainbenchmark
{
    class Test
    {
        private const string ToolName = "GraphEngine";

        private int modelSize;

        private static string resultDirectory = System.IO.Directory.GetCurrentDirectory() + "\\results\\";
        private static string modelsDirectory = System.IO.Directory.GetCurrentDirectory() + "\\models\\";


        public Test(int modelSize)
        {
            this.modelSize = modelSize;
        }

        public void performBenchmark()
        {
            System.IO.Directory.CreateDirectory(resultDirectory);
            routeSensorInjectTest();
            routeSensorRepairTest();
            switchSetInjectTest();
            switchSetRepairTest();
        }

        private void routeSensorInjectTest()
        {
            string timesFileName = resultDirectory + "times-" + ToolName + "-RouteSensorInjectTest-railway-inject-" + modelSize + "-.csv";
            //createMatchesCSVHeader(resultDirectory + "matches-" + ToolName + "RouteSensorInjectTest-railway-batch-" + modelSize + "-.csv");
            createTimesCSVHeader(timesFileName);
            TimesResultRow timesResultRow = new TimesResultRow
            {
                Tool = ToolName,
                Workload = "RouteSensorInjectTest",
                Description = "",
                Model = "railway-inject-" + modelSize,
                Run = "1",
                Phase = "Read",
                Iteration = null,
                Time = 0
            };
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            readInject();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Check";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkRouteSensor();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks/(Stopwatch.Frequency/ (1000L * 1000L));
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Transformation";
            timesResultRow.Time = 0;
            timesResultRow.Iteration = 1;
            stopWatch.Restart();
            routeSensorInject();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Recheck";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkRouteSensor();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);
        }

        private void routeSensorRepairTest()
        {
            string timesFileName = resultDirectory + "times-" + ToolName + "-RouteSensorRepairTest-railway-repair-" + modelSize + "-.csv";
            createTimesCSVHeader(timesFileName);
            TimesResultRow timesResultRow = new TimesResultRow
            {
                Tool = ToolName,
                Workload = "RouteSensorRepairTest",
                Description = "",
                Model = "railway-repair-" + modelSize,
                Run = "1",
                Phase = "Read",
                Iteration = null,
                Time = 0
            };
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            readRepair();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Check";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkRouteSensor();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Transformation";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            routeSensorRepair();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Recheck";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkRouteSensor();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);
        }

        private int checkRouteSensor()
        {
            Validator validator = new Validator();
            return validator.routeSensor();
        }

        private void routeSensorInject()
        {
            Modifier modifier = new Modifier();
            modifier.routeSensorInject(10);
        }

        private void routeSensorRepair()
        {
            Modifier modifier = new Modifier();
            modifier.routeSensorRepair();
        }

        private void switchSetInjectTest()
        {
            string timesFileName = resultDirectory + "times-" + ToolName + "-SwitchSetInjectTest-railway-inject-" + modelSize + "-.csv";
            //createMatchesCSVHeader(resultDirectory + "matches-" + ToolName + "RouteSensorInjectTest-railway-batch-" + modelSize + "-.csv");
            createTimesCSVHeader(timesFileName);
            TimesResultRow timesResultRow = new TimesResultRow
            {
                Tool = ToolName,
                Workload = "SwitchSetInjectTest",
                Description = "",
                Model = "railway-inject-" + modelSize,
                Run = "1",
                Phase = "Read",
                Iteration = null,
                Time = 0
            };
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            readInject();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Check";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkSwitchSet();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Transformation";
            timesResultRow.Time = 0;
            timesResultRow.Iteration = 1;
            stopWatch.Restart();
            switchSetInject();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Recheck";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkSwitchSet();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);
        }

        private void switchSetRepairTest()
        {
            string timesFileName = resultDirectory + "times-" + ToolName + "-SwitchSetRepairTest-railway-inject-" + modelSize + "-.csv";
            //createMatchesCSVHeader(resultDirectory + "matches-" + ToolName + "RouteSensorInjectTest-railway-batch-" + modelSize + "-.csv");
            createTimesCSVHeader(timesFileName);
            TimesResultRow timesResultRow = new TimesResultRow
            {
                Tool = ToolName,
                Workload = "SwitchSetRepairTest",
                Description = "",
                Model = "railway-inject-" + modelSize,
                Run = "1",
                Phase = "Read",
                Iteration = null,
                Time = 0
            };
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            readInject();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Check";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkSwitchSet();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Transformation";
            timesResultRow.Time = 0;
            timesResultRow.Iteration = 1;
            stopWatch.Restart();
            switchSetRepair();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);

            timesResultRow.Phase = "Recheck";
            timesResultRow.Time = 0;
            stopWatch.Restart();
            checkSwitchSet();
            stopWatch.Stop();
            timesResultRow.Time = stopWatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)); ;
            appendTimesCSVLine(timesFileName, timesResultRow);
        }

        private int checkSwitchSet()
        {
            Validator validator = new Validator();
            return validator.switchSet();
        }

        private void switchSetInject()
        {
            Modifier modifier = new Modifier();
            modifier.switchSetInject(10);
        }

        private void switchSetRepair()
        {
            Modifier modifier = new Modifier();
            modifier.switchSetRepair();
        }

        private void readInject()
        {

            RDFReader reader = new RDFReader(modelsDirectory + "inject\\" + "railway-inject-" + modelSize + "-inferred.ttl");
            reader.read();
        }

        private void readRepair()
        {
            RDFReader reader = new RDFReader(modelsDirectory + "repair\\" + "railway-repair-" + modelSize + "-inferred.ttl");
            reader.read();
        }

        private void createMatchesCSVHeader(string fileName)
        {
            using (StreamWriter matchesStreamWriter = new StreamWriter(fileName))
            {
                matchesStreamWriter.WriteLine("Tool,Workload,Description,Model,Run,Query,Iteration,Matches");
            }
        }
        private void createTimesCSVHeader(string fileName)
        {
            using (StreamWriter timesStreamWriter = new StreamWriter(fileName))
            {
                timesStreamWriter.WriteLine("Tool,Workload,Description,Model,Run,Phase,Iteration,Time");
            }
        }

        private void appendMatchesCSVLine(string fileName, MatchesResultRow matchesResultRow)
        {
            using (StreamWriter matchesStreamWriter = new StreamWriter(fileName, append: true))
            {
                matchesStreamWriter.WriteLine(matchesResultRow.Tool + "," + matchesResultRow.Workload + "," + matchesResultRow.Description + "," +
                    matchesResultRow.Model + "," + matchesResultRow.Run + "," + matchesResultRow.Query + "," + matchesResultRow.Iteration + "," +
                    matchesResultRow.Matches);
            }
        }

        private void appendTimesCSVLine(string fileName, TimesResultRow timesResultRow)
        {
            using (StreamWriter timesStreamWriter = new StreamWriter(fileName, append: true))
            {
                timesStreamWriter.WriteLine(timesResultRow.Tool + "," + timesResultRow.Workload + "," + timesResultRow.Description + "," +
                    timesResultRow.Model + "," + timesResultRow.Run + "," + timesResultRow.Phase + "," + timesResultRow.Iteration + "," +
                    timesResultRow.Time);
            }
        }
    }
}
