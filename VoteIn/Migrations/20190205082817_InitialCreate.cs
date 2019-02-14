using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VoteIn.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OPTION",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COLOR = table.Column<string>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VOTING_PROCESS_MODE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CODE = table.Column<string>(nullable: true),
                    NUMERIC = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTING_PROCESS_MODE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHOICE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ID_VOTING_PROCESS_MODE = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(nullable: true),
                    VALUE = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHOICE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CHOICE_VOTING_PROCESS_MODE_ID_VOTING_PROCESS_MODE",
                        column: x => x.ID_VOTING_PROCESS_MODE,
                        principalTable: "VOTING_PROCESS_MODE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VOTING_PROCESS",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AUTHOR = table.Column<string>(nullable: true),
                    AUTHOR_MAIL = table.Column<string>(nullable: true),
                    CLOSING_DATE = table.Column<DateTime>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    GUID = table.Column<Guid>(nullable: false),
                    GUID_PREVIOUS_VOTING_PROCESS = table.Column<Guid>(nullable: false),
                    ID_PREVIOUS_VOTING_PROCESS = table.Column<int>(nullable: true),
                    ID_USER = table.Column<string>(nullable: true),
                    ID_VOTING_PROCESS_MODE = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(nullable: true),
                    OPENING_DATE = table.Column<DateTime>(nullable: false),
                    PRIVATE_KEY = table.Column<string>(nullable: true),
                    PUBLIC = table.Column<bool>(nullable: false),
                    PUBLIC_KEY = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTING_PROCESS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VOTING_PROCESS_VOTING_PROCESS_ID_PREVIOUS_VOTING_PROCESS",
                        column: x => x.ID_PREVIOUS_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VOTING_PROCESS_USER_ID_USER",
                        column: x => x.ID_USER,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VOTING_PROCESS_VOTING_PROCESS_MODE_ID_VOTING_PROCESS_MODE",
                        column: x => x.ID_VOTING_PROCESS_MODE,
                        principalTable: "VOTING_PROCESS_MODE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ENVELOPE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CONTENT = table.Column<string>(nullable: true),
                    ID_VOTING_PROCESS = table.Column<int>(nullable: false),
                    KEY = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENVELOPE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ENVELOPE_VOTING_PROCESS_ID_VOTING_PROCESS",
                        column: x => x.ID_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RESULT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ID_VOTING_PROCESS = table.Column<int>(nullable: false),
                    ID_WINNING_OPTION = table.Column<int>(nullable: true),
                    VALID = table.Column<bool>(nullable: false),
                    NUMBER_VOTERS = table.Column<int>(nullable: false),
                    SCORES_DETAIL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESULT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RESULT_VOTING_PROCESS_ID_VOTING_PROCESS",
                        column: x => x.ID_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESULT_OPTION_ID_WINNING_OPTION",
                        column: x => x.ID_WINNING_OPTION,
                        principalTable: "OPTION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SUFFRAGE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    ID_VOTING_PROCESS = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUFFRAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SUFFRAGE_VOTING_PROCESS_ID_VOTING_PROCESS",
                        column: x => x.ID_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VOTER",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HAS_VOTED = table.Column<bool>(nullable: false),
                    ID_VOTING_PROCESS = table.Column<int>(nullable: false),
                    MAIL = table.Column<string>(nullable: true),
                    TOKEN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VOTER_VOTING_PROCESS_ID_VOTING_PROCESS",
                        column: x => x.ID_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VOTING_PROCESS_OPTION",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ID_OPTION = table.Column<int>(nullable: false),
                    ID_VOTING_PROCESS = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTING_PROCESS_OPTION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VOTING_PROCESS_OPTION_OPTION_ID_OPTION",
                        column: x => x.ID_OPTION,
                        principalTable: "OPTION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VOTING_PROCESS_OPTION_VOTING_PROCESS_ID_VOTING_PROCESS",
                        column: x => x.ID_VOTING_PROCESS,
                        principalTable: "VOTING_PROCESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ID_CHOICE = table.Column<int>(nullable: true),
                    ID_SUFFRAGE = table.Column<int>(nullable: false),
                    ID_VOTING_PROCESS_OPTION = table.Column<int>(nullable: true),
                    VALUE = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ACT_CHOICE_ID_CHOICE",
                        column: x => x.ID_CHOICE,
                        principalTable: "CHOICE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACT_SUFFRAGE_ID_SUFFRAGE",
                        column: x => x.ID_SUFFRAGE,
                        principalTable: "SUFFRAGE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACT_VOTING_PROCESS_OPTION_ID_VOTING_PROCESS_OPTION",
                        column: x => x.ID_VOTING_PROCESS_OPTION,
                        principalTable: "VOTING_PROCESS_OPTION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACT_ID_CHOICE",
                table: "ACT",
                column: "ID_CHOICE");

            migrationBuilder.CreateIndex(
                name: "IX_ACT_ID_SUFFRAGE",
                table: "ACT",
                column: "ID_SUFFRAGE");

            migrationBuilder.CreateIndex(
                name: "IX_ACT_ID_VOTING_PROCESS_OPTION",
                table: "ACT",
                column: "ID_VOTING_PROCESS_OPTION");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CHOICE_ID_VOTING_PROCESS_MODE",
                table: "CHOICE",
                column: "ID_VOTING_PROCESS_MODE");

            migrationBuilder.CreateIndex(
                name: "IX_ENVELOPE_ID_VOTING_PROCESS",
                table: "ENVELOPE",
                column: "ID_VOTING_PROCESS");

            migrationBuilder.CreateIndex(
                name: "IX_RESULT_ID_VOTING_PROCESS",
                table: "RESULT",
                column: "ID_VOTING_PROCESS");

            migrationBuilder.CreateIndex(
                name: "IX_RESULT_ID_WINNING_OPTION",
                table: "RESULT",
                column: "ID_WINNING_OPTION");

            migrationBuilder.CreateIndex(
                name: "IX_SUFFRAGE_ID_VOTING_PROCESS",
                table: "SUFFRAGE",
                column: "ID_VOTING_PROCESS");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "USER",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "USER",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VOTER_ID_VOTING_PROCESS",
                table: "VOTER",
                column: "ID_VOTING_PROCESS");

            migrationBuilder.CreateIndex(
                name: "IX_VOTING_PROCESS_ID_PREVIOUS_VOTING_PROCESS",
                table: "VOTING_PROCESS",
                column: "ID_PREVIOUS_VOTING_PROCESS");

            migrationBuilder.CreateIndex(
                name: "IX_VOTING_PROCESS_ID_USER",
                table: "VOTING_PROCESS",
                column: "ID_USER");

            migrationBuilder.CreateIndex(
                name: "IX_VOTING_PROCESS_ID_VOTING_PROCESS_MODE",
                table: "VOTING_PROCESS",
                column: "ID_VOTING_PROCESS_MODE");

            migrationBuilder.CreateIndex(
                name: "IX_VOTING_PROCESS_OPTION_ID_OPTION",
                table: "VOTING_PROCESS_OPTION",
                column: "ID_OPTION");

            migrationBuilder.CreateIndex(
                name: "IX_VOTING_PROCESS_OPTION_ID_VOTING_PROCESS",
                table: "VOTING_PROCESS_OPTION",
                column: "ID_VOTING_PROCESS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACT");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ENVELOPE");

            migrationBuilder.DropTable(
                name: "RESULT");

            migrationBuilder.DropTable(
                name: "VOTER");

            migrationBuilder.DropTable(
                name: "CHOICE");

            migrationBuilder.DropTable(
                name: "SUFFRAGE");

            migrationBuilder.DropTable(
                name: "VOTING_PROCESS_OPTION");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OPTION");

            migrationBuilder.DropTable(
                name: "VOTING_PROCESS");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "VOTING_PROCESS_MODE");
        }
    }
}
