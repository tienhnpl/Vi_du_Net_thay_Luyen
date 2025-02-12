using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspnetCoreMvcStarter.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspnetCoreMvcStarter.Data
{
    public class AspnetCoreMvcStarterContext : IdentityDbContext
    {
        public AspnetCoreMvcStarterContext (DbContextOptions<AspnetCoreMvcStarterContext> options)
            : base(options)
        {
        }

        public DbSet<AspnetCoreMvcStarter.Areas.Admin.Models.Product> Product { get; set; } = default!;
    }
}
