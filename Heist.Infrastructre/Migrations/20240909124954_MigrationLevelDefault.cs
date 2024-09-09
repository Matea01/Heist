using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Heist.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationLevelDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "MemberSkills",
                type: "varchar(255)",
                nullable: false, // or true if you want to allow NULLs
                defaultValue: "DefaultLevel", // Set a default value or use a meaningful value
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "MemberSkills",
                type: "varchar(255)",
                nullable: true, // or false depending on your requirement
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldDefaultValue: "DefaultLevel");
        }
    }
}
