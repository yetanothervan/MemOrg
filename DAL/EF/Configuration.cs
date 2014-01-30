using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class Configuration : DropCreateDatabaseAlways<MemOrgContext> //CreateDatabaseIfNotExists<MemOrgContext>
    {
        protected override void Seed(MemOrgContext context)
        {
            //SeedSample.Seed(context);
            SeedConceptualWar.Seed(context);
        }
    }
}
