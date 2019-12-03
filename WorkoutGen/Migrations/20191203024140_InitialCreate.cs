using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutGen.Migrations
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
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exercise",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(unicode: false, nullable: true),
                    hyperlink = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "muscle_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_muscle_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_equipment_set",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_equipment_set", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_equipment_set_AspNetUsers",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_exercise",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    hyperlink = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_exercise_AspNetUsers",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_workout",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_workout", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_workout_AspNetUsers",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_equipment_set_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipment_id = table.Column<int>(nullable: false),
                    user_equipment_set_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_equipment_set_equipment", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_equipment_set_equipment_equipment1",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exercise_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exercise_id = table.Column<int>(nullable: false),
                    equipment_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_equipment", x => x.id);
                    table.ForeignKey(
                        name: "FK__exercise___equip__208CD6FA",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__exercise___exerc__1F98B2C1",
                        column: x => x.exercise_id,
                        principalTable: "exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exercise_muscle_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exercise_id = table.Column<int>(nullable: false),
                    muscle_group_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_muscle_group", x => x.id);
                    table.ForeignKey(
                        name: "FK__exercise___exerc__29221CFB",
                        column: x => x.exercise_id,
                        principalTable: "exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__exercise___muscl__2A164134",
                        column: x => x.muscle_group_id,
                        principalTable: "muscle_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_exercise_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    user_exercise_id = table.Column<int>(nullable: false),
                    equipment_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise_equipment", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_exercise_equipment_equipment",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_exercise_equipment_user_exercise",
                        column: x => x.user_exercise_id,
                        principalTable: "user_exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_exercise_equipment_AspNetUsers",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_exercise_muscle_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    user_exercise_id = table.Column<int>(nullable: false),
                    muscle_group_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise_muscle_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_exercise_muscle_group_muscle_group",
                        column: x => x.muscle_group_id,
                        principalTable: "muscle_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_exercise_muscle_group_user_exercise",
                        column: x => x.user_exercise_id,
                        principalTable: "user_exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_exercise_muscle_group_AspNetUsers",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_set",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exercise_id = table.Column<int>(nullable: true),
                    user_exercise_id = table.Column<int>(nullable: true),
                    user_workout_id = table.Column<int>(nullable: false),
                    repetitions = table.Column<string>(maxLength: 50, nullable: true),
                    weight = table.Column<string>(maxLength: 50, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_set", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_set_exercise",
                        column: x => x.exercise_id,
                        principalTable: "exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_set_user_exercise1",
                        column: x => x.user_exercise_id,
                        principalTable: "user_exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_set_user_workout",
                        column: x => x.user_workout_id,
                        principalTable: "user_workout",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exercise_alternate_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exercise_equipment_id = table.Column<int>(nullable: false),
                    alternate_equipment_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_alternate_equipment", x => x.id);
                    table.ForeignKey(
                        name: "FK__exercise___alter__25518C17",
                        column: x => x.alternate_equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__exercise___exerc__245D67DE",
                        column: x => x.exercise_equipment_id,
                        principalTable: "exercise_equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_alternate_equipment_alternate_equipment_id",
                table: "exercise_alternate_equipment",
                column: "alternate_equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_alternate_equipment_exercise_equipment_id",
                table: "exercise_alternate_equipment",
                column: "exercise_equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_equipment_equipment_id",
                table: "exercise_equipment",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_equipment_exercise_id",
                table: "exercise_equipment",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_muscle_group_exercise_id",
                table: "exercise_muscle_group",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_muscle_group_muscle_group_id",
                table: "exercise_muscle_group",
                column: "muscle_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_equipment_set_user_id",
                table: "user_equipment_set",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_equipment_set_equipment_equipment_id",
                table: "user_equipment_set_equipment",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_user_id",
                table: "user_exercise",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_equipment_equipment_id",
                table: "user_exercise_equipment",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_equipment_user_exercise_id",
                table: "user_exercise_equipment",
                column: "user_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_equipment_user_id",
                table: "user_exercise_equipment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_muscle_group_muscle_group_id",
                table: "user_exercise_muscle_group",
                column: "muscle_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_muscle_group_user_exercise_id",
                table: "user_exercise_muscle_group",
                column: "user_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_muscle_group_user_id",
                table: "user_exercise_muscle_group",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_set_exercise_id",
                table: "user_set",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_set_user_exercise_id",
                table: "user_set",
                column: "user_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_set_user_workout_id",
                table: "user_set",
                column: "user_workout_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_workout_user_id",
                table: "user_workout",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "exercise_alternate_equipment");

            migrationBuilder.DropTable(
                name: "exercise_muscle_group");

            migrationBuilder.DropTable(
                name: "user_equipment_set");

            migrationBuilder.DropTable(
                name: "user_equipment_set_equipment");

            migrationBuilder.DropTable(
                name: "user_exercise_equipment");

            migrationBuilder.DropTable(
                name: "user_exercise_muscle_group");

            migrationBuilder.DropTable(
                name: "user_set");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "exercise_equipment");

            migrationBuilder.DropTable(
                name: "muscle_group");

            migrationBuilder.DropTable(
                name: "user_exercise");

            migrationBuilder.DropTable(
                name: "user_workout");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "exercise");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}