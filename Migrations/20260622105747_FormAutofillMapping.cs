using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebObrasci1.Migrations
{
    /// <inheritdoc />
    public partial class FormAutofillMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormAutofillMappingId",
                table: "FormFields",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormAutofillMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mapping = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAutofillMappings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FormAutofillMappingId",
                table: "FormFields",
                column: "FormAutofillMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_FormAutofillMappings_FormAutofillMappingId",
                table: "FormFields",
                column: "FormAutofillMappingId",
                principalTable: "FormAutofillMappings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_FormAutofillMappings_FormAutofillMappingId",
                table: "FormFields");

            migrationBuilder.DropTable(
                name: "FormAutofillMappings");

            migrationBuilder.DropIndex(
                name: "IX_FormFields_FormAutofillMappingId",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "FormAutofillMappingId",
                table: "FormFields");
        }
    }
}
