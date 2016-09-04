using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoodSurround.Logic;
using System.Data.Entity.Migrations;

namespace GoodSurround
{
    public class DbConfiguration : DbMigrationsConfiguration<GoodSurroundDbContext>
    {
        public DbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GoodSurroundDbContext context)
        {
            base.Seed(context);
        }
    }
}