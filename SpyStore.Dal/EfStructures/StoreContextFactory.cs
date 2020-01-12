using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.EfStructures
{
    class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            var connectionStirng =
                @"Server=DESKTOP-2BNPQS1\SQLEXPRESS2017;Database=SpyStore21;Trusted_Connection=True;MultipleActiveResultSets=true;";
            optionsBuilder
                .UseSqlServer(connectionStirng, options =>
                {
                    options.EnableRetryOnFailure();
                })
                .ConfigureWarnings(warnings =>
                {
                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
                });
            return new StoreContext(optionsBuilder.Options);

        }
    }
}
