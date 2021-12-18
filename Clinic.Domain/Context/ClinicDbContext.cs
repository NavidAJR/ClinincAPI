using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Domain.Context
{
    public  class ClinicDbContext : IdentityDbContext<User,Role,Guid>
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
            
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(p => new { p.ProviderKey, p.LoginProvider });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(p => new { p.UserId, p.LoginProvider });



            modelBuilder.Entity<User>(b =>
                b.HasQueryFilter(u => u.IsDelete == false));

            modelBuilder.Entity<User>(b =>
            {
                b.Property(e => e.Id).HasColumnName("UserId");
            });



            modelBuilder.Entity<Role>().HasData(
            
                new Role() {Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN"},
                new Role() {Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER"}
            );
        }

    }
}
