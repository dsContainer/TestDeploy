using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Digital.Data.Entities
{
    public partial class DigitalSignatureDBContext : DbContext
    {
        public DigitalSignatureDBContext()
        {
        }

        public DigitalSignatureDBContext(DbContextOptions<DigitalSignatureDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<ProcessData> ProcessDatas { get; set; }
        public virtual DbSet<ProcessStep> ProcessSteps { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<Signature> Signatures { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });


            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasIndex(e => e.DocumentTypeId, "IX_Documents_DocumentTypeID");

                entity.HasIndex(e => e.OwnerId, "IX_Documents_OwnerId");

                entity.HasIndex(e => e.ProcessId, "IX_Documents_ProcessId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.DocumentTypeId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.ProcessId);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Process>(entity =>
            {
                entity.HasIndex(e => e.TemplateId, "IX_Processes_TemplateId")
                    .IsUnique()
                    .HasFilter("([TemplateId] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Template)
                    .WithOne(p => p.Process)
                    .HasForeignKey<Process>(d => d.TemplateId);
            });

            modelBuilder.Entity<ProcessData>(entity =>
            {
                entity.HasIndex(e => e.ProcessId, "IX_ProcessDatas_ProcessId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.ProcessData)
                    .HasForeignKey(d => d.ProcessId);
            });

            modelBuilder.Entity<ProcessStep>(entity =>
            {
                entity.HasIndex(e => e.ProcessId, "IX_ProcessSteps_ProcessId");

                entity.HasIndex(e => e.UserId, "IX_ProcessSteps_UserId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Xpoint).HasColumnName("XPoint");

                entity.Property(e => e.XpointPercent).HasColumnName("XPointPercent");

                entity.Property(e => e.Ypoint).HasColumnName("YPoint");

                entity.Property(e => e.YpointPercent).HasColumnName("YPointPercent");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.ProcessSteps)
                    .HasForeignKey(d => d.ProcessId);

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.HasKey(e => new { e.RolesId, e.UsersId });

                entity.ToTable("RoleUser");

                entity.HasIndex(e => e.UsersId, "IX_RoleUser_UsersId");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.RoleUsers)
                    .HasForeignKey(d => d.RolesId);

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.RoleUsers)
                    .HasForeignKey(d => d.UsersId);
            });

            modelBuilder.Entity<Signature>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Signatures_UserId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasIndex(e => e.DocumentTypeId, "IX_Templates_DocumentTypeId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.Templates)
                    .HasForeignKey(d => d.DocumentTypeId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
