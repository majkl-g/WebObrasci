using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormFieldChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FormSubmissionApproval",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TypeTemp",
                table: "FormFields",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""FormFields"" set " +
                @"""TypeTemp"" = case when ""Type"" = 'text' then 0 " +
                @"when ""Type"" = 'number' then 2 " +
                @"when ""Type"" = 'date' then 4 else 0 end"
                );

            migrationBuilder.DropColumn(
                name: "Type",
                table: "FormFields");

            migrationBuilder.RenameColumn(
                name: "TypeTemp",
                table: "FormFields",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "FormFields",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxValue",
                table: "FormFields",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinValue",
                table: "FormFields",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StringType",
                table: "FormFields",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FormSubmissionApproval");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "StringType",
                table: "FormFields");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FormFields",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
