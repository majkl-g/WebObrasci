using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormSubmissionApprovalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalFrom",
                table: "FormSubmissionApproval");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalUserId",
                table: "FormSubmissionApproval",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissionApproval_ApprovalUserId",
                table: "FormSubmissionApproval",
                column: "ApprovalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmissionApproval_Users_ApprovalUserId",
                table: "FormSubmissionApproval",
                column: "ApprovalUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmissionApproval_Users_ApprovalUserId",
                table: "FormSubmissionApproval");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmissionApproval_ApprovalUserId",
                table: "FormSubmissionApproval");

            migrationBuilder.DropColumn(
                name: "ApprovalUserId",
                table: "FormSubmissionApproval");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalFrom",
                table: "FormSubmissionApproval",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
