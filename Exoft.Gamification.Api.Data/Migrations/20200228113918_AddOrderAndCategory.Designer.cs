﻿// <auto-generated />
using System;
using Exoft.Gamification.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Exoft.Gamification.Api.Data.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20200228113918_AddOrderAndCategory")]
    partial class AddOrderAndCategory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Achievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid?>("IconId");

                    b.Property<string>("Name");

                    b.Property<int>("XP");

                    b.HasKey("Id");

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ChapterId");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<double?>("UnitNumber");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("IconId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description");

                    b.Property<int>("Type");

                    b.Property<Guid?>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("IconId");

                    b.Property<int>("Popularity");

                    b.Property<int>("Price");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.OrderCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<Guid>("OrderId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderCategory");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.RequestAchievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AchievementId");

                    b.Property<string>("Message");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestAchievements");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Thank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedTime");

                    b.Property<Guid?>("FromUserId")
                        .IsRequired();

                    b.Property<string>("Text");

                    b.Property<Guid>("ToUserId");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Thanks");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AvatarId");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("Status");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.UserAchievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AchievementId");

                    b.Property<DateTime>("AddedTime");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAchievement");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.UserRoles", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Article", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Chapter")
                        .WithMany("Articles")
                        .HasForeignKey("ChapterId");
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Event", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.OrderCategory", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Order", "Order")
                        .WithMany("Categories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.RequestAchievement", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Achievement")
                        .WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.Thank", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User")
                        .WithMany()
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.UserAchievement", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Achievement", "Achievement")
                        .WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User", "User")
                        .WithMany("Achievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Exoft.Gamification.Api.Data.Core.Entities.UserRoles", b =>
                {
                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Exoft.Gamification.Api.Data.Core.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
