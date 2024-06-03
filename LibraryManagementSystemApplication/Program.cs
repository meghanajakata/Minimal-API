using LibraryManagementSystemApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;


namespace LibraryManagementSystemApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/", () => "Hello World!");
            app.MapGet("/books", async (Context db) =>
            {
                try
                {
                    var books = await db.Books.ToListAsync();
                    return Results.Ok(books);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.MapGet("/books/{id}", async (int id, Context db) =>
            {
                try
                {
                    var book = await db.Books.FindAsync(id);
                    if (book is not null)
                        return Results.Ok(book);
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.MapPost("/books", async (Book book, Context db) =>
            {
                try
                {
                    var _book = await db.Books.AnyAsync(b => b.ISBN == book.ISBN);
                    if (!_book)
                    {
                        db.Books.Add(book);
                        await db.SaveChangesAsync();
                        return Results.Created($"/books/{book.BookId}", book);
                    }
                    return Results.Conflict("The ISBN already exists");

                }
                catch (Exception ex)
                {

                    return Results.Problem(ex.Message);
                }

            });

            app.MapPut("/books/{id}", async (int id, Book book, Context db) =>
            {
                try
                {
                    var _book = await db.Books.FindAsync(id);

                    if (_book is null) return Results.NotFound();

                    _book.Title = book.Title;
                    _book.Author = book.Author;

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

                return Results.NoContent();
            });

            app.MapDelete("/books/{id}", async (int id, Context db) =>
            {
                if (await db.Books.FindAsync(id) is Book book)
                {
                    db.Books.Remove(book);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });

            app.MapGet("/authors", async (Context db) =>
            {
                try
                {
                    var authors = await db.Authors.ToListAsync();
                    return Results.Ok(authors);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.MapGet("/authors/{id}", async (int id, Context db) =>
            {
                try
                {
                    if (await db.Authors.FindAsync(id) is Author author)
                        return Results.Ok(author);
                    else
                        return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            });

            app.MapPost("/authors", async (Author author, Context db) =>
            {
                try
                {
                    db.Authors.Add(author);
                    await db.SaveChangesAsync();
                    return Results.Created($"/authors/{author.AuthorID}", author);

                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            });

            app.MapPut("/authors/{id}", async (int id, Author author, Context db) =>
            {
                try
                {
                    var _author = await db.Authors.FindAsync(id);

                    if (_author is null) return Results.NotFound();

                    _author.Name = author.Name;

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

                return Results.NoContent();
            });

            app.MapDelete("/authors/{id}", async (int id, Context db) =>
            {
                try
                {
                    if (await db.Authors.FindAsync(id) is Author author)
                    {
                        db.Authors.Remove(author);
                        await db.SaveChangesAsync();
                        return Results.NoContent();
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.Run();
        }
    }
}