﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.BooksServer.Models
{
    public class Book
    {
        public Book()
        {
        }

        public Book(int id, string isbn, string title, decimal price, string[] authors = null, DateTime? releaseDate = null)
        {
            Id = id;
            Isbn = isbn;
            Title = title;
            Price = price;
            Authors = authors;
            ReleaseDate = releaseDate;
        }

        public int Id { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string[] Authors { get; set; }
        public DateTime? ReleaseDate { get; set; }

    }
}
