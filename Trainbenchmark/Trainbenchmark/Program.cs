using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity;
using VDS.RDF;
using VDS.RDF.Parsing;
using TrainBenchmarkTSLProject;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Trainbenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2); // Uses the second Core or Processor for the Test
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;      // Prevents "Normal" processes 
                                                                                        // from interrupting Threads
            Thread.CurrentThread.Priority = ThreadPriority.Highest;     // Prevents "Normal" Threads 
                                                                        // from interrupting this thread
            Test test = new Test(1);
            test.performBenchmark();
        }
    }
}