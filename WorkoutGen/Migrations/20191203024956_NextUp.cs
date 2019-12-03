using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutGen.Migrations
{
    public partial class NextUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "user_exercise",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "user_exercise",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
