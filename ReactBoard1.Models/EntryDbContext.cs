using Microsoft.EntityFrameworkCore;

namespace ReactBoard1.Models
{
    public class EntryDbContext :DbContext
    {
        public EntryDbContext()
        {
            // empty
        }

        public EntryDbContext(DbContextOptions<EntryDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>().Property(m => m.Created).HasDefaultValueSql("GetDate()");
        }

        public DbSet<Entry> Entries { get; set; }
        
        // For dotnet framework or Windows Forms/WPF
        
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {   
        //     base.OnConfiguring(optionsBuilder);
        // }

        // Microsoft.EntityFrameworkCore
        // Microsoft.EntityFramework.SqlServer
        // Microsoft.EntityFrameworkCore.Tools
        // Microsoft.EntityFrameworkCore.InMemory
        // Microsoft.System.Configuration.ConfigurationManager
        // Microsoft.Data.SqlClient
    }
}