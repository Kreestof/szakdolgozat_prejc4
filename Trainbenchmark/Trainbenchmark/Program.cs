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

namespace Trainbenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i < 32; i *= 2)
            {
                using (StreamWriter file = new StreamWriter(@"c:\Kristof\BME\szakdolgozat\results\railway-inject-"+ i +".txt"))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    RDFReader reader = new RDFReader(@"c:\Kristof\BME\szakdolgozat\models\inject\railway-inject-"+ i +"-inferred.ttl");
                    reader.batch();
                    sw.Stop();
                    file.WriteLine("Time elapsed: " + sw.Elapsed);
                    sw.Restart();
                    Validator validator = new Validator(file);
                    validator.check();
                    sw.Stop();
                    file.WriteLine("Time elapsed: " + sw.Elapsed);
                    sw.Restart();
                    Modifier modifier = new Modifier(file);
                    modifier.routeSensorInject(10);
                    sw.Stop();
                    file.WriteLine("Time elapsed: " + sw.Elapsed);
                    sw.Restart();
                    modifier.switchSetInject(10);
                    sw.Stop();
                    file.WriteLine("Time elapsed: " + sw.Elapsed);
                    sw.Restart();
                    validator.check();
                    sw.Stop();
                    file.WriteLine("Time elapsed: " + sw.Elapsed);
                    //modifier.routeSensorRepair();
                    //validator.check();
                    //modifier.switchSetRepair();
                    //validator.check();
                }
                
            }
        }
    }
}