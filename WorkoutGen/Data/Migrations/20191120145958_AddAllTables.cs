using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutGen.Data.Migrations
{
    public partial class AddAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
                    description = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    image = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
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
                name: "user_equipment_set",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_equipment_set", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_equipment_set_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    equipment_id = table.Column<int>(nullable: false),
                    user_equipment_set_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_equipment_set_equipment", x => x.id);
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
                    image = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_exercise_equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    exercise_id = table.Column<int>(nullable: false),
                    equipment_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise_equipment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_exercise_muscle_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(maxLength: 450, nullable: false),
                    exercise_id = table.Column<int>(nullable: false),
                    muscle_group_id = table.Column<int>(nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_exercise_muscle_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_set",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    exercise_id = table.Column<int>(nullable: false),
                    workout_id = table.Column<int>(nullable: false),
                    repetitions = table.Column<string>(maxLength: 50, nullable: true),
                    weight = table.Column<string>(maxLength: 50, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_set", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_workout",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    user_id = table.Column<string>(maxLength: 450, nullable: true),
                    date_added = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    date_deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_workout", x => x.id);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exercise_alternate_equipment");

            migrationBuilder.DropTable(
                name: "exercise_muscle_group");

            migrationBuilder.DropTable(
                name: "user_equipment_set");

            migrationBuilder.DropTable(
                name: "user_equipment_set_equipment");

            migrationBuilder.DropTable(
                name: "user_exercise");

            migrationBuilder.DropTable(
                name: "user_exercise_equipment");

            migrationBuilder.DropTable(
                name: "user_exercise_muscle_group");

            migrationBuilder.DropTable(
                name: "user_set");

            migrationBuilder.DropTable(
                name: "user_workout");

            migrationBuilder.DropTable(
                name: "exercise_equipment");

            migrationBuilder.DropTable(
                name: "muscle_group");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "exercise");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
