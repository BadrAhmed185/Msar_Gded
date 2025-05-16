using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Msar_Gded.Migrations
{
    /// <inheritdoc />
    public partial class besmallah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    NationalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HighSchoolDivision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationalZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PureDegree = table.Column<int>(type: "int", nullable: false),
                    DegreeWithLanguage = table.Column<int>(type: "int", nullable: false),
                    TotalDegree = table.Column<int>(type: "int", nullable: false),
                    UniversityGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolCertificate = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LeterOfStatus = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    CollegeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.NationalId);
                    table.ForeignKey(
                        name: "FK_Students_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentCode = table.Column<int>(type: "int", nullable: true),
                    NeedRivision = table.Column<bool>(type: "bit", nullable: true),
                    CurrentUniversityId = table.Column<int>(type: "int", nullable: false),
                    CurrentCollegeId = table.Column<int>(type: "int", nullable: false),
                    DestinationUniversityId = table.Column<int>(type: "int", nullable: false),
                    DestinationCollegeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Colleges_CurrentCollegeId",
                        column: x => x.CurrentCollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Colleges_DestinationCollegeId",
                        column: x => x.DestinationCollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "NationalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Universities_CurrentUniversityId",
                        column: x => x.CurrentUniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Universities_DestinationUniversityId",
                        column: x => x.DestinationUniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrentCollegeId",
                table: "Requests",
                column: "CurrentCollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrentUniversityId",
                table: "Requests",
                column: "CurrentUniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DestinationCollegeId",
                table: "Requests",
                column: "DestinationCollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DestinationUniversityId",
                table: "Requests",
                column: "DestinationUniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StatusId",
                table: "Requests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StudentId",
                table: "Requests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CollegeId",
                table: "Students",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UniversityId",
                table: "Students",
                column: "UniversityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Colleges");

            migrationBuilder.DropTable(
                name: "Universities");
        }
    }
}
