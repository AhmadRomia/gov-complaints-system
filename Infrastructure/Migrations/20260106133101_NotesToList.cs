using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotesToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfoRequest",
                table: "Complaints");

            migrationBuilder.AlterColumn<string>(
                name: "AgencyNotes",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfoRequests",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfoRequests",
                table: "Complaints");

            migrationBuilder.AlterColumn<string>(
                name: "AgencyNotes",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfoRequest",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
