﻿// <auto-generated />
using System;
using F.Fireworks.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace F.Fireworks.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250629121710_Update_UserLoginLog_UserId_Nullable")]
    partial class Update_UserLoginLog_UserId_Nullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("F.Fireworks.Domain.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Identity.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RevokedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Logging.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("ExecutionDurationMs")
                        .HasColumnType("bigint");

                    b.Property<string>("HttpMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequestData")
                        .HasColumnType("text");

                    b.Property<string>("RequestName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResponseData")
                        .HasColumnType("text");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Logging.UserLoginLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Device")
                        .HasColumnType("text");

                    b.Property<string>("FailureReason")
                        .HasColumnType("text");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("WasSuccessful")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLoginLogs");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Permissions.ApplicationRolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Permissions.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<int>("SortOrder")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Permissions", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Tenants.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Attributes")
                        .HasColumnType("jsonb");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("F.Fireworks.Domain.Identity.RefreshToken", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Logging.UserLoginLog", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Permissions.ApplicationRolePermission", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Permissions.Permission", "Permission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationRole", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Permissions.Permission", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Permissions.Permission", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("F.Fireworks.Domain.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("F.Fireworks.Domain.Identity.ApplicationRole", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Identity.ApplicationUser", b =>
                {
                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("F.Fireworks.Domain.Permissions.Permission", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
