using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AutoService
{
    public partial class AutoServiceEntities : DbContext
    {
        private static AutoServiceEntities context;

        public AutoServiceEntities() : base("name=Autoservice")
        {

        }
        public static AutoServiceEntities GetContext()
        {
            if (context == null)
                context = new AutoServiceEntities();
            return context;
        }
    }
}
