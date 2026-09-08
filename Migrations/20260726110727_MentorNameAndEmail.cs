using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class MentorNameAndEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MentorMail",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MentorName",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MentorMail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MentorName",
                table: "Users");
        }
    }
}
