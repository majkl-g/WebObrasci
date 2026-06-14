using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormSubmissionDenial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Denied",
                table: "FormSubmissionApproval",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Denied",
                table: "FormSubmissionApproval");
        }
    }
}
