using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Forms",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Forms");
        }
    }
}
