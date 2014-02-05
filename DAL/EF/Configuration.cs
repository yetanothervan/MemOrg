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
    public class Configuration : CreateDatabaseIfNotExists<MemOrgContext>
        //DropCreateDatabaseAlways<MemOrgContext> 
    {
        protected override void Seed(MemOrgContext context)
        {
            //SeedSample.Seed(context);
            var seed = new SeedConceptualWar(context);
            seed.Seed();
        }
    }
}
