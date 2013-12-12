using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace Interfaces
{
    public interface ITmpXmlExportImportService
    {
        void SaveGraph();
    }
}
