﻿// <auto-generated />
using System;
using Digital.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalSignature.Migrations
{
    [DbContext(typeof(DigitalSignatureDBContext))]
    [Migration("20221112151225_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitalSignature.Entities.Batch", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("DigitalSignature.Entities.BatchProcess", b =>
                {
                    b.Property<Guid>("BatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProcessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BatchId", "ProcessId");

                    b.HasIndex(new[] { "ProcessId" }, "IX_BatchProcess_ProcessId");

                    b.ToTable("BatchProcess", (string)null);
                });

            modelBuilder.Entity("DigitalSignature.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DocumentTypeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DocumentTypeID");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProcessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DocumentTypeId" }, "IX_Documents_DocumentTypeID");

                    b.HasIndex(new[] { "OwnerId" }, "IX_Documents_OwnerId");

                    b.HasIndex(new[] { "ProcessId" }, "IX_Documents_ProcessId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("DigitalSignature.Entities.DocumentType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Process", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TemplateId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "TemplateId" }, "IX_Processes_TemplateId")
                        .IsUnique()
                        .HasFilter("([TemplateId] IS NOT NULL)");

                    b.ToTable("Processes");
                });

            modelBuilder.Entity("DigitalSignature.Entities.ProcessData", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DateUpload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProcessId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ProcessId" }, "IX_ProcessDatas_ProcessId");

                    b.ToTable("ProcessDatas");
                });

            modelBuilder.Entity("DigitalSignature.Entities.ProcessStep", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSign")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("OrderIndex")
                        .HasColumnType("real");

                    b.Property<int>("PageSign")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProcessId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.Property<float>("Xpoint")
                        .HasColumnType("real")
                        .HasColumnName("XPoint");

                    b.Property<float>("XpointPercent")
                        .HasColumnType("real")
                        .HasColumnName("XPointPercent");

                    b.Property<float>("Ypoint")
                        .HasColumnType("real")
                        .HasColumnName("YPoint");

                    b.Property<float>("YpointPercent")
                        .HasColumnType("real")
                        .HasColumnName("YPointPercent");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ProcessId" }, "IX_ProcessSteps_ProcessId");

                    b.HasIndex(new[] { "UserId" }, "IX_ProcessSteps_UserId")
                        .IsUnique();

                    b.ToTable("ProcessSteps");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DigitalSignature.Entities.RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex(new[] { "UsersId" }, "IX_RoleUser_UsersId");

                    b.ToTable("RoleUser", (string)null);
                });

            modelBuilder.Entity("DigitalSignature.Entities.Signature", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_Signatures_UserId")
                        .IsUnique();

                    b.ToTable("Signatures");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DocumentTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DocumentTypeId" }, "IX_Templates_DocumentTypeId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("DigitalSignature.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SigId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DigitalSignature.Entities.BatchProcess", b =>
                {
                    b.HasOne("DigitalSignature.Entities.Batch", "Batch")
                        .WithMany("BatchProcesses")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalSignature.Entities.Process", "Process")
                        .WithMany("BatchProcesses")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("Process");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Document", b =>
                {
                    b.HasOne("DigitalSignature.Entities.DocumentType", "DocumentType")
                        .WithMany("Documents")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalSignature.Entities.User", "Owner")
                        .WithMany("Documents")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalSignature.Entities.Process", "Process")
                        .WithMany("Documents")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentType");

                    b.Navigation("Owner");

                    b.Navigation("Process");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Process", b =>
                {
                    b.HasOne("DigitalSignature.Entities.Template", "Template")
                        .WithOne("Process")
                        .HasForeignKey("DigitalSignature.Entities.Process", "TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("DigitalSignature.Entities.ProcessData", b =>
                {
                    b.HasOne("DigitalSignature.Entities.Process", "Process")
                        .WithMany("ProcessData")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Process");
                });

            modelBuilder.Entity("DigitalSignature.Entities.ProcessStep", b =>
                {
                    b.HasOne("DigitalSignature.Entities.Process", "Process")
                        .WithMany("ProcessSteps")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalSignature.Entities.User", "User")
                        .WithOne("ProcessStep")
                        .HasForeignKey("DigitalSignature.Entities.ProcessStep", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Process");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DigitalSignature.Entities.RoleUser", b =>
                {
                    b.HasOne("DigitalSignature.Entities.Role", "Roles")
                        .WithMany("RoleUsers")
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalSignature.Entities.User", "Users")
                        .WithMany("RoleUsers")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Signature", b =>
                {
                    b.HasOne("DigitalSignature.Entities.User", "User")
                        .WithOne("Signature")
                        .HasForeignKey("DigitalSignature.Entities.Signature", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Template", b =>
                {
                    b.HasOne("DigitalSignature.Entities.DocumentType", "DocumentType")
                        .WithMany("Templates")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentType");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Batch", b =>
                {
                    b.Navigation("BatchProcesses");
                });

            modelBuilder.Entity("DigitalSignature.Entities.DocumentType", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Templates");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Process", b =>
                {
                    b.Navigation("BatchProcesses");

                    b.Navigation("Documents");

                    b.Navigation("ProcessData");

                    b.Navigation("ProcessSteps");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Role", b =>
                {
                    b.Navigation("RoleUsers");
                });

            modelBuilder.Entity("DigitalSignature.Entities.Template", b =>
                {
                    b.Navigation("Process")
                        .IsRequired();
                });

            modelBuilder.Entity("DigitalSignature.Entities.User", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("ProcessStep")
                        .IsRequired();

                    b.Navigation("RoleUsers");

                    b.Navigation("Signature")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
