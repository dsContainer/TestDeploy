using Microsoft.EntityFrameworkCore;

namespace DigitalSignature.Entities
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

        public virtual DbSet<Batch> Batches { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public virtual DbSet<Process> Processes { get; set; } = null!;
        public virtual DbSet<ProcessData> ProcessDatas { get; set; } = null!;
        public virtual DbSet<ProcessStep> ProcessSteps { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Signature> Signatures { get; set; } = null!;
        public virtual DbSet<Template> Templates { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=digtalsig.database.windows.net;Initial Catalog=DigitalSignatureDB;User ID=digitalsignature;Password=password123@");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasMany(d => d.Processes)
                    .WithMany(p => p.Batches)
                    .UsingEntity<Dictionary<string, object>>(
                        "BatchProcess",
                        l => l.HasOne<Process>().WithMany().HasForeignKey("ProcessId"),
                        r => r.HasOne<Batch>().WithMany().HasForeignKey("BatchId"),
                        j =>
                        {
                            j.HasKey("BatchId", "ProcessId");

                            j.ToTable("BatchProcess");

                            j.HasIndex(new[] { "ProcessId" }, "IX_BatchProcess_ProcessId");
                        });
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

                entity.HasOne(d => d.User)
                    .WithOne(p => p.ProcessStep)
                    .HasForeignKey<ProcessStep>(d => d.UserId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "RoleUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UsersId"),
                        r => r.HasOne<Role>().WithMany().HasForeignKey("RolesId"),
                        j =>
                        {
                            j.HasKey("RolesId", "UsersId");

                            j.ToTable("RoleUser");

                            j.HasIndex(new[] { "UsersId" }, "IX_RoleUser_UsersId");
                        });
            });

            modelBuilder.Entity<Signature>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Signatures_UserId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Signature)
                    .HasForeignKey<Signature>(d => d.UserId);
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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
