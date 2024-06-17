using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.DAL.Models;

public partial class RamyaEmployeeDirectoryDbContext : DbContext
{
    public RamyaEmployeeDirectoryDbContext(DbContextOptions<RamyaEmployeeDirectoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasOne<Department>(d => d.DepartmentNavigation).WithMany(r=>r.Roles).HasForeignKey(e => e.Department).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Role>().HasOne<Location>(l => l.LocationNavigation).WithMany(r=>r.Roles).HasForeignKey(e => e.Location).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Employee>().HasOne<Department>(d=>d.DepartmentNavigation).WithMany(e=>e.Employees).HasForeignKey(e=>e.Department).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Employee>().HasOne<Location>(l=>l.LocationNavigation).WithMany(e=>e.Employees).HasForeignKey(e=>e.Location).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
        
        modelBuilder.Entity<Employee>().HasOne<Manager>(d=>d.ManagerNavigation).WithMany(e=>e.Employees).HasForeignKey(e=>e.Manager).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
        
        modelBuilder.Entity<Employee>().HasOne<Project>(d=>d.ProjectNavigation).WithMany(e=>e.Employees).HasForeignKey(e=>e.Project).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);

       modelBuilder.Entity<Employee>().HasOne<Role>(d=>d.JobTitleNavigation).WithMany(e=>e.Employees).HasForeignKey(e=>e.JobTitle).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
    }
}