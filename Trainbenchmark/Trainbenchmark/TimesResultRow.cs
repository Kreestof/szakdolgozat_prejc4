﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainbenchmark
{
    class TimesResultRow
    {
        public string Tool { get; set; }
        public string Workload { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Run { get; set; }
        public string Phase { get; set; }
        public int? Iteration { get; set; }
        public long Time { get; set; }
    }
}
