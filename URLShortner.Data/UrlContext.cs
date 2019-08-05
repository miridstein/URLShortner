using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace URLShortner.Data
{
    public class UrlContext : DbContext
    {
        private string _connectionString;

        public UrlContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<UrlLink> UrlLinks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Taken from here:
            //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration


        }
    }
}
