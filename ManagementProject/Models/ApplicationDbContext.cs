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
        public DbSet<Work> Works { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<QuyTrinh> QuyTrinhs { get; set; }
        public DbSet<BuocThucHien> BuocThucHiens { get; set; }
        public DbSet<Nhan> Nhans { get; set; }
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

                entity.HasOne<QuyTrinh>(e => e.QuyTrinh)
                       .WithMany(p => p.Projects)
                       .HasForeignKey(e => e.QuyTrinhId)
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

            builder.Entity<Work>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired(false).HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.CompleteDate).IsRequired(false);
                entity.Property(e => e.Updated).IsRequired(false);
                entity.Property(e => e.ParentWorkId).IsRequired(false);

                entity.HasOne<ApplicationUser>(e => e.CreatedUser)
                        .WithMany(p => p.CreatedWorks)
                        .HasForeignKey(e => e.CreatedUserId)
                        .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<ApplicationUser>(e => e.AssignUser)
                        .WithMany(p => p.AssignWorks)
                        .HasForeignKey(e => e.AssignUserId)
                        .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Project>(e => e.Project)
                        .WithMany(p => p.Works)
                        .HasForeignKey(e => e.ProjectId)
                        .OnDelete(DeleteBehavior.NoAction);

            });

           

            builder.Entity<AttachmentFile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.FileType).IsRequired(true).HasMaxLength(50);
                entity.Property(e => e.FilePath).IsRequired(true);

                entity.HasOne<Work>(e => e.Work)
                        .WithMany(p => p.AttachmentFiles)
                        .HasForeignKey(e => e.WorkId)
                        .OnDelete(DeleteBehavior.NoAction);

            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired(true).HasColumnType("text");
                

                entity.HasOne<Work>(e => e.Work)
                        .WithMany(p => p.Comments)
                        .HasForeignKey(e => e.WorkId)
                        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<ApplicationUser>(e => e.User)
                        .WithMany(p => p.Comments)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<QuyTrinh>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenQuyTrinh).IsRequired(true);
                entity.Property(e => e.NgayCapNhat).IsRequired(false);

                entity.HasOne<ApplicationUser>(e => e.NguoiTao)
                        .WithMany(p => p.QuyTrinhs)
                        .HasForeignKey(e => e.NguoiTaoId)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<BuocThucHien>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique(true);
                entity.Property(e => e.TenBuoc).IsRequired(true);
                entity.Property(e => e.NgayCapNhat).IsRequired(false);

                entity.HasOne<QuyTrinh>(e => e.QuyTrinh)
                        .WithMany(p => p.BuocThucHiens)
                        .HasForeignKey(e => e.QuyTrinhId);
                entity.HasOne<ApplicationUser>(e => e.NguoiThucHien)
                        .WithMany(p => p.BuocThucHiens)
                        .HasForeignKey(e => e.NguoiThucHienId);
            });

            builder.Entity<Nhan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenNhan).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.LoaiNhan).IsRequired(true).HasMaxLength(50);
                entity.Property(e => e.isLock).IsRequired(true);
                entity.Property(e => e.Created).IsRequired(true);
                entity.Property(e => e.Updated).IsRequired(false);
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
