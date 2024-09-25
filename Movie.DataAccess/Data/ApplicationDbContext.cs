using Microsoft.EntityFrameworkCore;
using Movie.Models;


namespace Movie.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Scifi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Description = "A novel set in the 1920s about the mysterious millionaire Jay Gatsby.",
                    ISBN = "9780743273565",
                    Author = "F. Scott Fitzgerald",
                    ListPrice = 20.00,
                    Price = 18.00,
                    Price50 = 16.00,
                    Price100 = 14.00,
                    cateGoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "1984",
                    Description = "A dystopian novel that delves into the dangers of totalitarianism.",
                    ISBN = "9780451524935",
                    Author = "George Orwell",
                    ListPrice = 15.00,
                    Price = 13.50,
                    Price50 = 12.00,
                    Price100 = 10.50,
                    cateGoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "To Kill a Mockingbird",
                    Description = "A classic of modern American literature, exploring racial injustice in the Deep South.",
                    ISBN = "9780060935467",
                    Author = "Harper Lee",
                    ListPrice = 25.00,
                    Price = 22.50,
                    Price50 = 20.00,
                    Price100 = 18.00,
                    cateGoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Pride and Prejudice",
                    Description = "A romantic novel that critiques the British landed gentry at the end of the 18th century.",
                    ISBN = "9780141439518",
                    Author = "Jane Austen",
                    ListPrice = 12.00,
                    Price = 10.50,
                    Price50 = 9.00,
                    Price100 = 8.00,
                    cateGoryId = 2,
                    ImageUrl = ""

                },
                new Product
                {
                    Id = 5,
                    Title = "The Catcher in the Rye",
                    Description = "A novel about teenage alienation and rebellion.",
                    ISBN = "9780316769488",
                    Author = "J.D. Salinger",
                    ListPrice = 18.00,
                    Price = 16.50,
                    Price50 = 15.00,
                    Price100 = 13.50,
                    cateGoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "The Hobbit",
                    Description = "A fantasy novel that precedes The Lord of the Rings.",
                    ISBN = "9780345339683",
                    Author = "J.R.R. Tolkien",
                    ListPrice = 22.00,
                    Price = 20.00,
                    Price50 = 18.00,
                    Price100 = 16.00
                    ,cateGoryId=1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 7,
                    Title = "Moby-Dick",
                    Description = "A novel about Captain Ahab’s obsession with a white whale.",
                    ISBN = "9781503280786",
                    Author = "Herman Melville",
                    ListPrice = 17.00,
                    Price = 15.50,
                    Price50 = 14.00,
                    Price100 = 12.50
                    ,cateGoryId=2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 8,
                    Title = "War and Peace",
                    Description = "A historical novel set during the Napoleonic Wars in Russia.",
                    ISBN = "9781853260629",
                    Author = "Leo Tolstoy",
                    ListPrice = 30.00,
                    Price = 27.00,
                    Price50 = 24.00,
                    Price100 = 22.00,
                    cateGoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 9,
                    Title = "The Odyssey",
                    Description = "An epic poem following Odysseus’ journey home after the Trojan War.",
                    ISBN = "9780140268867",
                    Author = "Homer",
                    ListPrice = 14.00,
                    Price = 12.50,
                    Price50 = 11.00,
                    Price100 = 10.00,
                    cateGoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 10,
                    Title = "Crime and Punishment",
                    Description = "A psychological novel exploring morality and guilt.",
                    ISBN = "9780486415871",
                    Author = "Fyodor Dostoevsky",
                    ListPrice = 20.00,
                    Price = 18.00,
                    Price50 = 16.00,
                    Price100 = 14.50,
                    cateGoryId = 2,
                    ImageUrl = ""
                }
                );
        }
    }
}
