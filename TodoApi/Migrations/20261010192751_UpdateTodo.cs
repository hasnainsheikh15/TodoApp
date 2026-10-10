using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Todos",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Todos",
                newName: "id");
        }
    }
}
