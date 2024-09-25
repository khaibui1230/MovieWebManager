﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movie.DataAccess.Data;

#nullable disable

namespace Movie.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240916195940_AddImageUrl")]
    partial class AddImageUrl
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Movie.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Scifi"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("Movie.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Price100")
                        .HasColumnType("float");

                    b.Property<double>("Price50")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("cateGoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("cateGoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "F. Scott Fitzgerald",
                            Description = "A novel set in the 1920s about the mysterious millionaire Jay Gatsby.",
                            ISBN = "9780743273565",
                            ImageUrl = "",
                            ListPrice = 20.0,
                            Price = 18.0,
                            Price100 = 14.0,
                            Price50 = 16.0,
                            Title = "The Great Gatsby",
                            cateGoryId = 1
                        },
                        new
                        {
                            Id = 2,
                            Author = "George Orwell",
                            Description = "A dystopian novel that delves into the dangers of totalitarianism.",
                            ISBN = "9780451524935",
                            ImageUrl = "",
                            ListPrice = 15.0,
                            Price = 13.5,
                            Price100 = 10.5,
                            Price50 = 12.0,
                            Title = "1984",
                            cateGoryId = 1
                        },
                        new
                        {
                            Id = 3,
                            Author = "Harper Lee",
                            Description = "A classic of modern American literature, exploring racial injustice in the Deep South.",
                            ISBN = "9780060935467",
                            ImageUrl = "",
                            ListPrice = 25.0,
                            Price = 22.5,
                            Price100 = 18.0,
                            Price50 = 20.0,
                            Title = "To Kill a Mockingbird",
                            cateGoryId = 1
                        },
                        new
                        {
                            Id = 4,
                            Author = "Jane Austen",
                            Description = "A romantic novel that critiques the British landed gentry at the end of the 18th century.",
                            ISBN = "9780141439518",
                            ImageUrl = "",
                            ListPrice = 12.0,
                            Price = 10.5,
                            Price100 = 8.0,
                            Price50 = 9.0,
                            Title = "Pride and Prejudice",
                            cateGoryId = 2
                        },
                        new
                        {
                            Id = 5,
                            Author = "J.D. Salinger",
                            Description = "A novel about teenage alienation and rebellion.",
                            ISBN = "9780316769488",
                            ImageUrl = "",
                            ListPrice = 18.0,
                            Price = 16.5,
                            Price100 = 13.5,
                            Price50 = 15.0,
                            Title = "The Catcher in the Rye",
                            cateGoryId = 3
                        },
                        new
                        {
                            Id = 6,
                            Author = "J.R.R. Tolkien",
                            Description = "A fantasy novel that precedes The Lord of the Rings.",
                            ISBN = "9780345339683",
                            ImageUrl = "",
                            ListPrice = 22.0,
                            Price = 20.0,
                            Price100 = 16.0,
                            Price50 = 18.0,
                            Title = "The Hobbit",
                            cateGoryId = 1
                        },
                        new
                        {
                            Id = 7,
                            Author = "Herman Melville",
                            Description = "A novel about Captain Ahab’s obsession with a white whale.",
                            ISBN = "9781503280786",
                            ImageUrl = "",
                            ListPrice = 17.0,
                            Price = 15.5,
                            Price100 = 12.5,
                            Price50 = 14.0,
                            Title = "Moby-Dick",
                            cateGoryId = 2
                        },
                        new
                        {
                            Id = 8,
                            Author = "Leo Tolstoy",
                            Description = "A historical novel set during the Napoleonic Wars in Russia.",
                            ISBN = "9781853260629",
                            ImageUrl = "",
                            ListPrice = 30.0,
                            Price = 27.0,
                            Price100 = 22.0,
                            Price50 = 24.0,
                            Title = "War and Peace",
                            cateGoryId = 2
                        },
                        new
                        {
                            Id = 9,
                            Author = "Homer",
                            Description = "An epic poem following Odysseus’ journey home after the Trojan War.",
                            ISBN = "9780140268867",
                            ImageUrl = "",
                            ListPrice = 14.0,
                            Price = 12.5,
                            Price100 = 10.0,
                            Price50 = 11.0,
                            Title = "The Odyssey",
                            cateGoryId = 2
                        },
                        new
                        {
                            Id = 10,
                            Author = "Fyodor Dostoevsky",
                            Description = "A psychological novel exploring morality and guilt.",
                            ISBN = "9780486415871",
                            ImageUrl = "",
                            ListPrice = 20.0,
                            Price = 18.0,
                            Price100 = 14.5,
                            Price50 = 16.0,
                            Title = "Crime and Punishment",
                            cateGoryId = 2
                        });
                });

            modelBuilder.Entity("Movie.Models.Product", b =>
                {
                    b.HasOne("Movie.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("cateGoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
