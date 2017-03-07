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
            reader.read();
            Validator validator = new Validator();
            validator.check();
        }
    }
}