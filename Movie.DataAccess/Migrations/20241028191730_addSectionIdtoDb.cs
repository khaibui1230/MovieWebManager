using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addSectionIdtoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SectionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "OrderHeaders");
        }
    }
}
