using ManagementProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ManagementProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ResetPasswordToken).IsRequired(false);
                entity.Property(e => e.Position).IsRequired(false);
                //entity.Property(e => e.DepartmentId).IsRequired(false);
                entity.HasOne<Department>(e => e.Department)
                        .WithMany(d => d.Users)
                        .HasForeignKey(e => e.DepartmentId)
                        .IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()").IsRequired(true);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired(false);
            });

            builder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()").IsRequired(true);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.StartDate).IsRequired(false);
                entity.Property(e => e.EndDate).IsRequired(false);
                entity.Property(e => e.CompleteDate).IsRequired(false);
                entity.HasOne<ApplicationUser>(e => e.CreatedUser)
                        .WithMany(p => p.Projects)
                        .HasForeignKey(e => e.CreatedId)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
                
                entity.HasOne<Project>(e => e.Project)
                        .WithMany(p => p.Members)
                        .HasForeignKey(e => e.ProjectId)
                        .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<ApplicationUser>(e => e.Member)
                        .WithMany(p => p.ProjectMembers)
                        .HasForeignKey(e => e.MemberId)
                        .OnDelete(DeleteBehavior.NoAction);
            });


            //builder.Entity<EquipmentType>(entity =>
            //{
            //    entity.HasKey(e => e.EquipmentTypeId);
            //    entity.Property(e => e.EquipmentTypeId).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Name).IsRequired();
            //    entity.HasIndex(e => e.Name).IsUnique();
            //});

            //builder.Entity<Equipment>(entity =>
            //{
            //    entity.HasKey(e => e.EquipmentId);
            //    entity.Property(e => e.EquipmentId).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Name).IsRequired();
            //    entity.Property(e => e.Status).IsRequired();
            //    entity.HasOne<EquipmentType>(e => e.EquipmentType)
            //            .WithMany(et => et.Equipments)
            //            .HasForeignKey(e => e.EquipmentTypeId);

            //});

            //builder.Entity<AssignmentEquipment>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.HasOne<Equipment>(ass => ass.Equipment)
            //            .WithMany(e => e.AssignmentEquipments)
            //            .HasForeignKey(ass => ass.EquipmentId);
            //    entity.HasOne<ApplicationUser>(ass => ass.UserHandle)
            //            .WithMany(u => u.HistoryHandles)
            //            .HasForeignKey(ass => ass.UserIdOfHandle)
            //            .OnDelete(DeleteBehavior.NoAction);
            //    entity.HasOne<ApplicationUser>(ass => ass.Employee)
            //            .WithMany(u => u.HistoryEquipments)
            //            .HasForeignKey(ass => ass.EmployeeId)
            //            .OnDelete(DeleteBehavior.NoAction);
            //});
        }
    }
}
