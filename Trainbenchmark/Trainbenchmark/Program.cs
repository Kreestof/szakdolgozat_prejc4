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
    internal class Program
    {
        static void Main(string[] args)
        {
            RDFReader reader = new RDFReader();
            Modifier modifier = new Modifier();
            Validator validator = new Validator();
            reader.batch();
            validator.check();
            modifier.routeSensorInject(10);
            validator.check();
            modifier.switchSetInject(10);
            validator.check();
            modifier.routeSensorRepair();
            validator.check();
            modifier.switchSetRepair();
            validator.check();
        }
    }
}