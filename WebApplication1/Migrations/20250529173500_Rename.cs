using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignments_Assignments_AssignmentId",
                table: "StudentAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignments_Users_StudentId",
                table: "StudentAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Users_StudentId",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAssignments",
                table: "StudentAssignments");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "StudentsCourse");

            migrationBuilder.RenameTable(
                name: "StudentAssignments",
                newName: "StudentsAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentsCourse",
                newName: "IX_StudentsCourse_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentsCourse",
                newName: "IX_StudentsCourse_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAssignments_StudentId",
                table: "StudentsAssignment",
                newName: "IX_StudentsAssignment_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAssignments_AssignmentId",
                table: "StudentsAssignment",
                newName: "IX_StudentsAssignment_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsCourse",
                table: "StudentsCourse",
                column: "StudentCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsAssignment",
                table: "StudentsAssignment",
                column: "StudentAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsAssignment_Assignments_AssignmentId",
                table: "StudentsAssignment",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsAssignment_Users_StudentId",
                table: "StudentsAssignment",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsCourse_Courses_CourseId",
                table: "StudentsCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsCourse_Users_StudentId",
                table: "StudentsCourse",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsAssignment_Assignments_AssignmentId",
                table: "StudentsAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsAssignment_Users_StudentId",
                table: "StudentsAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsCourse_Courses_CourseId",
                table: "StudentsCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsCourse_Users_StudentId",
                table: "StudentsCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsCourse",
                table: "StudentsCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsAssignment",
                table: "StudentsAssignment");

            migrationBuilder.RenameTable(
                name: "StudentsCourse",
                newName: "StudentCourses");

            migrationBuilder.RenameTable(
                name: "StudentsAssignment",
                newName: "StudentAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsCourse_StudentId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsCourse_CourseId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsAssignment_StudentId",
                table: "StudentAssignments",
                newName: "IX_StudentAssignments_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsAssignment_AssignmentId",
                table: "StudentAssignments",
                newName: "IX_StudentAssignments_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                column: "StudentCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAssignments",
                table: "StudentAssignments",
                column: "StudentAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignments_Assignments_AssignmentId",
                table: "StudentAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignments_Users_StudentId",
                table: "StudentAssignments",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Users_StudentId",
                table: "StudentCourses",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
