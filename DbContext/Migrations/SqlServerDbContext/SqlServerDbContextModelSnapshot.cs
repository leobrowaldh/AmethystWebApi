﻿// <auto-generated />
using System;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    [DbContext(typeof(MainDbContext.SqlServerDbContext))]
    partial class SqlServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DbModels.AddressDbM", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<int>("Country")
                        .HasColumnType("int");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.Property<string>("strCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("strCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses", "supusr");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.Property<Guid>("AttractionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressDbMAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("strCategory")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("AttractionId");

                    b.HasIndex("AddressDbMAddressId");

                    b.ToTable("Attractions", "supusr");
                });

            modelBuilder.Entity("DbModels.BankDbM", b =>
                {
                    b.Property<Guid>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BankComment")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("BankNumber")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Banks")
                        .HasColumnType("int");

                    b.Property<string>("EnryptedToken")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RiskLevel")
                        .HasColumnType("int");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("strIssuer")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("BankId");

                    b.HasIndex("AttractionId")
                        .IsUnique();

                    b.ToTable("Banks", "supusr");
                });

            modelBuilder.Entity("DbModels.CommentDbM", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttractionDbMAttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommentAge")
                        .HasColumnType("int");

                    b.Property<string>("CommentName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("strRating")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("strType")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("CommentId");

                    b.HasIndex("AttractionDbMAttractionId");

                    b.ToTable("Comments", "supusr");
                });

            modelBuilder.Entity("DbModels.UserDbM", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("Models.DTO.GstUsrInfoDbDto", b =>
                {
                    b.Property<int>("NrSeededAddresses")
                        .HasColumnType("int");

                    b.Property<int>("NrSeededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("NrSeededComments")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededAddresses")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededComments")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoDb", "gstusr");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.HasOne("DbModels.AddressDbM", "AddressDbM")
                        .WithMany("AttractionDbM")
                        .HasForeignKey("AddressDbMAddressId");

                    b.Navigation("AddressDbM");
                });

            modelBuilder.Entity("DbModels.BankDbM", b =>
                {
                    b.HasOne("DbModels.AttractionDbM", "AttractionDbM")
                        .WithOne("BankDbM")
                        .HasForeignKey("DbModels.BankDbM", "AttractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttractionDbM");
                });

            modelBuilder.Entity("DbModels.CommentDbM", b =>
                {
                    b.HasOne("DbModels.AttractionDbM", "AttractionDbM")
                        .WithMany("CommentsDbM")
                        .HasForeignKey("AttractionDbMAttractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttractionDbM");
                });

            modelBuilder.Entity("DbModels.AddressDbM", b =>
                {
                    b.Navigation("AttractionDbM");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.Navigation("BankDbM");

                    b.Navigation("CommentsDbM");
                });
#pragma warning restore 612, 618
        }
    }
}
