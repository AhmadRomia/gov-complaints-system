using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Complaints",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfoRequest",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgencyNotes",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attachments",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GovernmentEntityId",
                table: "Complaints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "OtpCode",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "GovernmentEntityId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GovernmentEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_GovernmentEntityId",
                table: "Complaints",
                column: "GovernmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GovernmentEntityId",
                table: "AspNetUsers",
                column: "GovernmentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GovernmentEntities_GovernmentEntityId",
                table: "AspNetUsers",
                column: "GovernmentEntityId",
                principalTable: "GovernmentEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_GovernmentEntities_GovernmentEntityId",
                table: "Complaints",
                column: "GovernmentEntityId",
                principalTable: "GovernmentEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GovernmentEntities_GovernmentEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_GovernmentEntities_GovernmentEntityId",
                table: "Complaints");

            migrationBuilder.DropTable(
                name: "GovernmentEntities");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_GovernmentEntityId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GovernmentEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdditionalInfoRequest",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AgencyNotes",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Attachments",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "GovernmentEntityId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "GovernmentEntityId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Complaints",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OtpCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
