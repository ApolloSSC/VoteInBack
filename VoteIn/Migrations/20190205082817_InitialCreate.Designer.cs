﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VoteIn.DAL;

namespace VoteIn.Migrations
{
    [DbContext(typeof(VoteInContext))]
    [Migration("20190205082817_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Act", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int?>("IdChoice")
                        .HasColumnName("ID_CHOICE");

                    b.Property<int>("IdSuffrage")
                        .HasColumnName("ID_SUFFRAGE");

                    b.Property<int?>("IdVotingProcessOption")
                        .HasColumnName("ID_VOTING_PROCESS_OPTION");

                    b.Property<int?>("Value")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex("IdChoice");

                    b.HasIndex("IdSuffrage");

                    b.HasIndex("IdVotingProcessOption");

                    b.ToTable("ACT");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("IdVotingProcessMode")
                        .HasColumnName("ID_VOTING_PROCESS_MODE");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.Property<int?>("Value")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex("IdVotingProcessMode");

                    b.ToTable("CHOICE");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Envelope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Content")
                        .HasColumnName("CONTENT");

                    b.Property<int>("IdVotingProcess")
                        .HasColumnName("ID_VOTING_PROCESS");

                    b.Property<string>("Key")
                        .HasColumnName("KEY");

                    b.HasKey("Id");

                    b.HasIndex("IdVotingProcess");

                    b.ToTable("ENVELOPE");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Color")
                        .HasColumnName("COLOR");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("OPTION");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("IdVotingProcess")
                        .HasColumnName("ID_VOTING_PROCESS");

                    b.Property<int?>("IdWinningOption")
                        .HasColumnName("ID_WINNING_OPTION");

                    b.Property<bool>("IsValid")
                        .HasColumnName("VALID");

                    b.Property<int>("NbVoters")
                        .HasColumnName("NUMBER_VOTERS");

                    b.Property<string>("ScoreDetail")
                        .HasColumnName("SCORES_DETAIL");

                    b.HasKey("Id");

                    b.HasIndex("IdVotingProcess");

                    b.HasIndex("IdWinningOption");

                    b.ToTable("RESULT");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Suffrage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("IdVotingProcess")
                        .HasColumnName("ID_VOTING_PROCESS");

                    b.HasKey("Id");

                    b.HasIndex("IdVotingProcess");

                    b.ToTable("SUFFRAGE");
                });

            modelBuilder.Entity("VoteIn.Model.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("USER");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Voter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<bool>("HasVoted")
                        .HasColumnName("HAS_VOTED");

                    b.Property<int>("IdVotingProcess")
                        .HasColumnName("ID_VOTING_PROCESS");

                    b.Property<string>("Mail")
                        .HasColumnName("MAIL");

                    b.Property<string>("Token")
                        .HasColumnName("TOKEN");

                    b.HasKey("Id");

                    b.HasIndex("IdVotingProcess");

                    b.ToTable("VOTER");
                });

            modelBuilder.Entity("VoteIn.Model.Models.VotingProcess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Author")
                        .HasColumnName("AUTHOR");

                    b.Property<string>("AuthorMail")
                        .HasColumnName("AUTHOR_MAIL");

                    b.Property<DateTime?>("ClosingDate")
                        .HasColumnName("CLOSING_DATE");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION");

                    b.Property<Guid>("Guid")
                        .HasColumnName("GUID");

                    b.Property<Guid>("GuidPreviousVotingProcess")
                        .HasColumnName("GUID_PREVIOUS_VOTING_PROCESS");

                    b.Property<int?>("IdPreviousVotingProcess")
                        .HasColumnName("ID_PREVIOUS_VOTING_PROCESS");

                    b.Property<string>("IdUser")
                        .HasColumnName("ID_USER");

                    b.Property<int>("IdVotingProcessMode")
                        .HasColumnName("ID_VOTING_PROCESS_MODE");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnName("OPENING_DATE");

                    b.Property<string>("PrivateKey")
                        .HasColumnName("PRIVATE_KEY");

                    b.Property<bool>("Public")
                        .HasColumnName("PUBLIC");

                    b.Property<string>("PublicKey")
                        .HasColumnName("PUBLIC_KEY");

                    b.HasKey("Id");

                    b.HasIndex("IdPreviousVotingProcess");

                    b.HasIndex("IdUser");

                    b.HasIndex("IdVotingProcessMode");

                    b.ToTable("VOTING_PROCESS");
                });

            modelBuilder.Entity("VoteIn.Model.Models.VotingProcessMode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE");

                    b.Property<bool>("Numeric")
                        .HasColumnName("NUMERIC");

                    b.HasKey("Id");

                    b.ToTable("VOTING_PROCESS_MODE");
                });

            modelBuilder.Entity("VoteIn.Model.Models.VotingProcessOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("IdOption")
                        .HasColumnName("ID_OPTION");

                    b.Property<int>("IdVotingProcess")
                        .HasColumnName("ID_VOTING_PROCESS");

                    b.HasKey("Id");

                    b.HasIndex("IdOption");

                    b.HasIndex("IdVotingProcess");

                    b.ToTable("VOTING_PROCESS_OPTION");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("VoteIn.Model.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("VoteIn.Model.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VoteIn.Model.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("VoteIn.Model.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.Act", b =>
                {
                    b.HasOne("VoteIn.Model.Models.Choice", "Choice")
                        .WithMany("Act")
                        .HasForeignKey("IdChoice");

                    b.HasOne("VoteIn.Model.Models.Suffrage", "Suffrage")
                        .WithMany("Act")
                        .HasForeignKey("IdSuffrage")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VoteIn.Model.Models.VotingProcessOption", "VotingProcessOption")
                        .WithMany("Act")
                        .HasForeignKey("IdVotingProcessOption");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Choice", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcessMode", "VotingProcessMode")
                        .WithMany("Choice")
                        .HasForeignKey("IdVotingProcessMode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.Envelope", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcess", "VotingProcess")
                        .WithMany("Envelope")
                        .HasForeignKey("IdVotingProcess")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.Result", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcess", "VotingProcess")
                        .WithMany()
                        .HasForeignKey("IdVotingProcess")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VoteIn.Model.Models.Option", "WinningOption")
                        .WithMany()
                        .HasForeignKey("IdWinningOption");
                });

            modelBuilder.Entity("VoteIn.Model.Models.Suffrage", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcess", "VotingProcess")
                        .WithMany("Suffrage")
                        .HasForeignKey("IdVotingProcess")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.Voter", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcess", "VotingProcess")
                        .WithMany("Voter")
                        .HasForeignKey("IdVotingProcess")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.VotingProcess", b =>
                {
                    b.HasOne("VoteIn.Model.Models.VotingProcess", "PreviousVotingProcess")
                        .WithMany()
                        .HasForeignKey("IdPreviousVotingProcess");

                    b.HasOne("VoteIn.Model.Models.User", "User")
                        .WithMany("VotingProcess")
                        .HasForeignKey("IdUser");

                    b.HasOne("VoteIn.Model.Models.VotingProcessMode", "VotingProcessMode")
                        .WithMany("VotingProcess")
                        .HasForeignKey("IdVotingProcessMode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteIn.Model.Models.VotingProcessOption", b =>
                {
                    b.HasOne("VoteIn.Model.Models.Option", "Option")
                        .WithMany("VotingProcessOption")
                        .HasForeignKey("IdOption")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VoteIn.Model.Models.VotingProcess", "VotingProcess")
                        .WithMany("VotingProcessOption")
                        .HasForeignKey("IdVotingProcess")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
