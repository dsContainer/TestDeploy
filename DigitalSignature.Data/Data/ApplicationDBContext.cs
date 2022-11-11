using System;
using System.Reflection;
using System.Reflection.Emit;
using DigitalSignature.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Digital.Data.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessData> ProcessDatas { get; set; }
        public DbSet<ProcessStep> ProcessSteps { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Batch> Batches { get; set; }

        #endregion
    }
}

