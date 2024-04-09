using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagerBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenCreated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cgpa",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "dept",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SemesterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterNumber = table.Column<int>(type: "int", nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    CGPA = table.Column<double>(type: "float", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterDetails_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SemesterDetails_StudentId",
                table: "SemesterDetails",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SemesterDetails");

            migrationBuilder.DropColumn(
                name: "dept",
                table: "Student");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenCreated",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "Cgpa",
                table: "Student",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
