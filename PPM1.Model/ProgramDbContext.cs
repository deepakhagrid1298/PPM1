using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PPM1.Model;

namespace PPM.Model
{
    public class ProgramDbContext : DbContext
    {
        private const string connectString = "Server=DESKTOP-NNAFOET\\SQLEXPRESS; Database=DB1;Integrated security=true;TrustServerCertificate=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectString);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
