using LdapApplication.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace LdapApplicaiton.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserLoginInfo> UserLoginInfo { get; set; }
    }
}
