using Microsoft.EntityFrameworkCore;
using HospitalAPI.Models;

namespace HospitalAPI.Data
{
    public class DataApplicationDBContext : DbContext
    {
        public DataApplicationDBContext(DbContextOptions<DataApplicationDBContext> options)
            : base(options) { }

        public DbSet<LabRequest> LabRequests { get; set; } = null!;
        public DbSet<LabTest> LabTests { get; set; } = null!;

        // ✅ ADD Result DbSet
         public DbSet<Result> Results { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Map Result entity to renamed table
            modelBuilder.Entity<Result>()
                .ToTable("result_table");

            modelBuilder.Entity<LabRequest>()
                .HasMany(r => r.LabTests)
                .WithOne(t => t.LabRequest)
                .HasForeignKey(t => t.LabRequestId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
