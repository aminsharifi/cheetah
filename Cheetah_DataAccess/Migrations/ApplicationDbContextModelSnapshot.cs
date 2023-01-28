﻿// <auto-generated />
using System;
using Cheetah_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheetahDataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cheetah_DataAccess.CopyProduct", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idRole")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("idRole");

                    b.ToTable("CopyProducts");
                });

            modelBuilder.Entity("Cheetah_DataAccess.CopyROLE", b =>
                {
                    b.Property<int>("idRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idRole"));

                    b.Property<Guid>("guidRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("roleName")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("idRole");

                    b.ToTable("CopyROLEs");
                });

            modelBuilder.Entity("Cheetah_DataAccess.ParameterList", b =>
                {
                    b.Property<long>("idParameterList")
                        .HasColumnType("bigint");

                    b.Property<int?>("Code")
                        .HasColumnType("int");

                    b.Property<string>("CodeStr")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Dsc")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("Ordering")
                        .HasColumnType("int");

                    b.Property<long?>("PValue")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParameterType")
                        .HasColumnType("bigint");

                    b.Property<long>("ParameterType1idParameterType")
                        .HasColumnType("bigint");

                    b.Property<byte>("dplyParameterList")
                        .HasColumnType("tinyint");

                    b.Property<bool>("dsblParameterList")
                        .HasColumnType("bit");

                    b.Property<int?>("finalEnt")
                        .HasColumnType("int");

                    b.Property<Guid>("guidParameterList")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("idParameterList");

                    b.HasIndex("ParameterType1idParameterType");

                    b.ToTable("ParameterList");
                });

            modelBuilder.Entity("Cheetah_DataAccess.ParameterType", b =>
                {
                    b.Property<long>("idParameterType")
                        .HasColumnType("bigint");

                    b.Property<int?>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Dsc")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte>("dplyParameterType")
                        .HasColumnType("tinyint");

                    b.Property<bool>("dsblParameterType")
                        .HasColumnType("bit");

                    b.Property<int?>("finalEnt")
                        .HasColumnType("int");

                    b.Property<Guid>("guidParameterType")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("idParameterType");

                    b.ToTable("ParameterType");
                });

            modelBuilder.Entity("Cheetah_DataAccess.CopyProduct", b =>
                {
                    b.HasOne("Cheetah_DataAccess.CopyROLE", "copyROLE")
                        .WithMany()
                        .HasForeignKey("idRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("copyROLE");
                });

            modelBuilder.Entity("Cheetah_DataAccess.ParameterList", b =>
                {
                    b.HasOne("Cheetah_DataAccess.ParameterType", "ParameterType1")
                        .WithMany("ParameterLists")
                        .HasForeignKey("ParameterType1idParameterType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParameterType1");
                });

            modelBuilder.Entity("Cheetah_DataAccess.ParameterType", b =>
                {
                    b.Navigation("ParameterLists");
                });
#pragma warning restore 612, 618
        }
    }
}
