using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movie.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", "A novel set in the 1920s about the mysterious millionaire Jay Gatsby.", "9780743273565", 20.0, 18.0, 14.0, 16.0, "The Great Gatsby" },
                    { 2, "George Orwell", "A dystopian novel that delves into the dangers of totalitarianism.", "9780451524935", 15.0, 13.5, 10.5, 12.0, "1984" },
                    { 3, "Harper Lee", "A classic of modern American literature, exploring racial injustice in the Deep South.", "9780060935467", 25.0, 22.5, 18.0, 20.0, "To Kill a Mockingbird" },
                    { 4, "Jane Austen", "A romantic novel that critiques the British landed gentry at the end of the 18th century.", "9780141439518", 12.0, 10.5, 8.0, 9.0, "Pride and Prejudice" },
                    { 5, "J.D. Salinger", "A novel about teenage alienation and rebellion.", "9780316769488", 18.0, 16.5, 13.5, 15.0, "The Catcher in the Rye" },
                    { 6, "J.R.R. Tolkien", "A fantasy novel that precedes The Lord of the Rings.", "9780345339683", 22.0, 20.0, 16.0, 18.0, "The Hobbit" },
                    { 7, "Herman Melville", "A novel about Captain Ahab’s obsession with a white whale.", "9781503280786", 17.0, 15.5, 12.5, 14.0, "Moby-Dick" },
                    { 8, "Leo Tolstoy", "A historical novel set during the Napoleonic Wars in Russia.", "9781853260629", 30.0, 27.0, 22.0, 24.0, "War and Peace" },
                    { 9, "Homer", "An epic poem following Odysseus’ journey home after the Trojan War.", "9780140268867", 14.0, 12.5, 10.0, 11.0, "The Odyssey" },
                    { 10, "Fyodor Dostoevsky", "A psychological novel exploring morality and guilt.", "9780486415871", 20.0, 18.0, 14.5, 16.0, "Crime and Punishment" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
