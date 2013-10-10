using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksMapViewerModule
{
    public class ContentViewModel 
    {
        public ContentViewModel()
        {
            MyText = "Some of my texts";
        }

        public string MyText { get; set; }
    }
}
