using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApiWalkthrough2.Models;

namespace WebApiWalkthrough2
{
    public class WalkthroughDbContext : DbContext
    {
        static WalkthroughDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new ApplicationDbInit());
        }
        public WalkthroughDbContext()
            :base("WalkthroughDbContext")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public IDbSet<Company> Companies { get; set; } 


        public static WalkthroughDbContext Create()
        {
            return new WalkthroughDbContext();
        }
    }

    public class ApplicationDbInit : DropCreateDatabaseIfModelChanges<WalkthroughDbContext>
    {
        public override void InitializeDatabase(WalkthroughDbContext context)
        {
            try
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                        context.Database.Connection.Database));
            }
            catch (Exception)
            {
                // ignored
            }

            base.InitializeDatabase(context);

            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE {0} SET MULTI_USER", context.Database.Connection.Database));
        }

        protected override void Seed(WalkthroughDbContext context)
        {
            base.Seed(context);

            context.Companies.Add(new Company {Name = "Microsoft"});
            context.Companies.Add(new Company {Name = "Apple"});
            context.Companies.Add(new Company { Name = "Google" });
        }
    }
}