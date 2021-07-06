using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class ChartJSModel
    {
        public ChartJSModel()
        {
            Labels = new List<string>();
            Data = new List<int>();
            BackgroundColor = new List<string>();
        }
        public List<string> Labels { get; set; }

        public List<int> Data { get; set; }

        public List<string> BackgroundColor { get; set; }
    }
}
