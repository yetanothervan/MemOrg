using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemOrg.Interfaces
{
    public class PointSize
    {
        public PointSize()
        {
            Offset = new Point();
            Size = new Size();
        }
        public Point Offset;
        public Size Size;
    }
}
