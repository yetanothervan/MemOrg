using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphDrawService
{
    public class LogicalBlock : ContainerVisual
    {
        public LogicalBlock()
        {
            Data = null;
        }
        public object Data;
    }
}
