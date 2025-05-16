namespace Msar_Gded
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Msar_Gded.Models;

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.NationalId);

            modelBuilder.Entity<Student>()
    .HasIndex(s => s.NationalId)
    .HasDatabaseName("IX_Students_NationalId");

            modelBuilder.Entity<Student>()
                .HasOne(s => s.University)
                .WithMany(u => u.Students)
                .HasForeignKey(s => s.UniversityId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            modelBuilder.Entity<Student>()
                .HasOne(s => s.College)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            // Request
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when a Student is deleted

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Status)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            // Restrict delete for specific foreign keys in Request
            modelBuilder.Entity<Request>()
                .HasOne(r => r.CurrentUniversity)
                .WithMany(u => u.RequestsAsCurrent)
                .HasForeignKey(r => r.CurrentUniversityId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            modelBuilder.Entity<Request>()
                .HasOne(r => r.DestinationUniversity)
                .WithMany(u => u.RequestsAsDestination)
                .HasForeignKey(r => r.DestinationUniversityId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            modelBuilder.Entity<Request>()
                .HasOne(r => r.CurrentCollege)
                .WithMany(c => c.RequestsAsCurrent)
                .HasForeignKey(r => r.CurrentCollegeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts

            modelBuilder.Entity<Request>()
                .HasOne(r => r.DestinationCollege)
                .WithMany(c => c.RequestsAsDestination)
                .HasForeignKey(r => r.DestinationCollegeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid cascading conflicts
        }

    }

}
