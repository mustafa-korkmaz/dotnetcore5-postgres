﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dotnetpostgres.Dal.Databases.Postgres;

namespace dotnetpostgres.Dal.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20210211201130_V0")]
    partial class V0
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AuthorizedPersonName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorizedPersonName = "Esra Korkmaz",
                            CreatedAt = new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Akcam Ltd. "
                        });
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7f9fcc26-c38c-46bd-86a7-b7b3d5959b78"),
                            ConcurrencyStamp = "f0e32c74-6510-484d-a6f4-db6a79eb82e4",
                            Name = "owner",
                            NormalizedName = "OWNER"
                        },
                        new
                        {
                            Id = new Guid("e964fe31-ba9a-4ee6-98c1-7fa84767868d"),
                            ConcurrencyStamp = "a620a23a-3760-4d4f-a095-79ffe5fd9a39",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("0967d456-60a8-43de-9ac8-5f15dfaa1909"),
                            ConcurrencyStamp = "7473ba90-3e20-491d-a4c2-ecbc7f22ec5f",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = new Guid("8a158f67-b9aa-4dec-9e8f-53d29aeb1905"),
                            ConcurrencyStamp = "8a51070d-4ad9-4b7a-84af-c7b4bfb7aa41",
                            Name = "demo_user",
                            NormalizedName = "DEMO_USER"
                        },
                        new
                        {
                            Id = new Guid("de522b4b-4a2a-4992-a1b3-3b27b93a0d5a"),
                            ConcurrencyStamp = "7c591b7b-0cfc-4de0-a216-a920779b7e7d",
                            Name = "premium_user",
                            NormalizedName = "PREMIUM_USER"
                        });
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NameSurname")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(8000)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(12)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Settings")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("87622649-96c8-40b5-bcef-8351b0883b49"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "024e1046-752c-4943-9373-5ac78ab5601a",
                            CreatedAt = new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mustafa@koruq.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NameSurname = "Mustafa Korkmaz",
                            NormalizedEmail = "MUSTAFA@KORUQ.COM",
                            NormalizedUserName = "MUSTAFA@KORUQ.COM",
                            PasswordHash = "AD5bszN5VbOZSQW+1qcXQb08ElGNt9uNoTrsNenNHSsD1g2Gp6ya4+uFJWmoUsmfng==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "951a4c00-20d0-4d65-9d4a-7db4001c834c",
                            Title = "System",
                            TwoFactorEnabled = false,
                            UserName = "mustafa@koruq.com"
                        });
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserClaim<System.Guid>", b =>
                {
                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserLogin<System.Guid>", b =>
                {
                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserRole<System.Guid>", b =>
                {
                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetpostgres.Dal.Entities.Identity.ApplicationUserToken<System.Guid>", b =>
                {
                    b.HasOne("dotnetpostgres.Dal.Entities.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
