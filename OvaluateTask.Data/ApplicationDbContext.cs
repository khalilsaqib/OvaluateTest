using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OvaluateTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OvaluateTask.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>(e =>
            {
                e.ToTable(nameof(Customer));
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            });
            builder.Entity<Order>(e =>
            {
                e.ToTable(nameof(Order));
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).IsRequired().ValueGeneratedNever();
                e.Property(x => x.CustomerId).IsRequired();
                e.HasOne(x=>x.Customer).WithMany(y=>y.Orders).HasForeignKey(z=>z.CustomerId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }

}
