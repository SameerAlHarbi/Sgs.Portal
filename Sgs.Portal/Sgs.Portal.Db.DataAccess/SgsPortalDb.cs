using Microsoft.EntityFrameworkCore;
using Sgs.Portal.Shared.Models;

namespace Sgs.Portal.Db.DataAccess
{
    public class SgsPortalDb : DbContext
    {
        public SgsPortalDb(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
    }
}
