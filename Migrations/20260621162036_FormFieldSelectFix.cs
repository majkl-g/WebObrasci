using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormFieldSelectFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFieldSelectValue_FormFields_FormFieldId",
                table: "FormFieldSelectValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormFieldSelectValue",
                table: "FormFieldSelectValue");

            migrationBuilder.RenameTable(
                name: "FormFieldSelectValue",
                newName: "FormFieldSelectValues");

            migrationBuilder.RenameIndex(
                name: "IX_FormFieldSelectValue_FormFieldId",
                table: "FormFieldSelectValues",
                newName: "IX_FormFieldSelectValues_FormFieldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormFieldSelectValues",
                table: "FormFieldSelectValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFieldSelectValues_FormFields_FormFieldId",
                table: "FormFieldSelectValues",
                column: "FormFieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFieldSelectValues_FormFields_FormFieldId",
                table: "FormFieldSelectValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormFieldSelectValues",
                table: "FormFieldSelectValues");

            migrationBuilder.RenameTable(
                name: "FormFieldSelectValues",
                newName: "FormFieldSelectValue");

            migrationBuilder.RenameIndex(
                name: "IX_FormFieldSelectValues_FormFieldId",
                table: "FormFieldSelectValue",
                newName: "IX_FormFieldSelectValue_FormFieldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormFieldSelectValue",
                table: "FormFieldSelectValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFieldSelectValue_FormFields_FormFieldId",
                table: "FormFieldSelectValue",
                column: "FormFieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
