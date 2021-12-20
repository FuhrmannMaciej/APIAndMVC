using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksApi.Models
{
    public static class SeedData
    {
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new BookContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<BookContext>>()))
                {
                    if (context.Books.Any())
                    {
                        return;
                    }

                    context.Books.AddRange(
                        new Book
                        {
                            Author = "J.K. Rowling",
                            Title = "Harry Potter and the Prisoner of Azkaban",
                            NumberOfPages = 317,
                            CopiesSold = 124_521_412L,
                            IsAwarded = true
                        },

                        new Book
                        {
                            Author = "John Grisham",
                            Title = "The Judge's List",
                            NumberOfPages = 412,
                            CopiesSold = 52_111_957L,
                            IsAwarded = true
                        },

                        new Book
                        {
                            Author = "James Patterson",
                            Title = "Fear No Evil",
                            NumberOfPages = 521,
                            CopiesSold = 4_893_922L,
                            IsAwarded = false
                        },

                        new Book
                        {
                            Author = "Danielle Steel",
                            Title = "Flying Angels",
                            NumberOfPages = 429,
                            CopiesSold = 721_990L,
                            IsAwarded = false
                        },

                        new Book
                        {
                            Author = "David Baldacci",
                            Title = "Mercy",
                            NumberOfPages = 896,
                            CopiesSold = 95_524_871L,
                            IsAwarded = true
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
