using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apirest.Migrations;
/// <inheritdoc />
public partial class ForeignTaskToComments : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TaskId",
            table: "Comments",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Comments_TaskId",
            table: "Comments",
            column: "TaskId");

        migrationBuilder.AddForeignKey(
            name: "FK_Comments_Tasks_TaskId",
            table: "Comments",
            column: "TaskId",
            principalTable: "Tasks",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Comments_Tasks_TaskId",
            table: "Comments");

        migrationBuilder.DropIndex(
            name: "IX_Comments_TaskId",
            table: "Comments");

        migrationBuilder.DropColumn(
            name: "TaskId",
            table: "Comments");
    }
}
