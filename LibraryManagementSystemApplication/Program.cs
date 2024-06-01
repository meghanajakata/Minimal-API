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
                    await db.Books.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            app.MapGet("/books/{id}", async (int id, Context db) =>
            {
                try
                {
                    if (await db.Books.FindAsync(id) is Book book)
                        Results.Ok(book);
                    else
                        Results.NotFound();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            });

            app.MapPost("/books", async (Book book, Context db) =>
            {
                try
                {
                    db.Books.Add(book);
                    await db.SaveChangesAsync();
                    return Results.Created($"/books/{book.BookId}", book);

                }
                catch (Exception ex)
                {

                    throw ex;
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
                catch(Exception ex)
                {
                    throw ex;
                }

                return Results.NoContent();
            });

            app.MapDelete("/books/{id}", async (int id, Context db) =>
            {
                if(await db.Books.FindAsync(id) is Book book )
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
                    return authors;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            app.MapGet("/authors/{id}", async (int id, Context db) =>
            {
                try
                {
                    if (await db.Authors.FindAsync(id) is Author author)
                        Results.Ok(author);
                    else
                        Results.NotFound();
                }
                catch (Exception ex)
                {
                    throw ex;
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

                    throw ex;
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
                    throw ex;
                }

                return Results.NoContent();
            });

            app.MapDelete("/authors/{id}", async (int id, Context db) =>
            {
                if (await db.Authors.FindAsync(id) is Author author)
                {
                    db.Authors.Remove(author);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });

            app.Run();
        }
    }
}