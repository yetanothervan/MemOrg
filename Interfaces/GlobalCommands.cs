using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace MemOrg.Interfaces
{
    public static class GlobalCommands
    {
        public static CompositeCommand ToggleHeadersCompositeCommand = new CompositeCommand();
        public static CompositeCommand RefreshGraphViewCompositeCommand = new CompositeCommand();
    }
}
