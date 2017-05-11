﻿using AspNetCore.BooksServer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.BooksServer.Controllers
{
    [Route("api/books")]
    public class BooksController : Controller
    {
        private static int lastBookId = 4;

        // ACHTUNG: nur zu DEMO-Zwecken. Dictionary ist nicht threadsafe!
        private static readonly Dictionary<int, Book> books = new Dictionary<int, Book>()
        {
            {1,new Book(1,"1430242337", "C# Pro", 30, new []{"Troelson"}, DateTime.Now.AddYears(-2))},
            {2,new Book(2,"161729134X", "C# in Depth", 40, new []{"Skeet"}, DateTime.Now.AddMonths(-2))},
            {3,new Book(3,"1449320104", "C# in a Nutshell", 40, new []{"Albahari"}, DateTime.Now.AddMonths(-2))},
            {4,new Book(4,"0596807260", "Entity Framework 6", 20, new []{"Lerman"}, DateTime.Now.AddMonths(-2))},
        };

        // GET api/books
        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            return books.Values;
        }

        // GET api/books/1
        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            Book book;
            return books.TryGetValue(id, out book) ? (IActionResult)Ok(book) : NotFound();
        }

        // ~ überschreibt das RoutePrefix
        // GET api/authors/skeet/books
        [HttpGet("~/api/authors/{author:alpha}/books")]
        public IEnumerable<Book> GetBookByAuthorName(string author)
        {
            var result = books.Values
                .Where(b => b.Authors != null && b.Authors.Contains(author, StringComparer.OrdinalIgnoreCase))
                .ToArray();
            return result;
        }

        [HttpGet("~/api/authors/{author:alpha}/books/{year:int:min(1950):max(2050)}")]
        // GET api/authors/skeet/books/2015
        public IEnumerable<Book> GetBookByAuthorNameInYear(string author, int year)
        {
            var result = books.Values
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year == year
                    && b.Authors != null && b.Authors.Contains(author, StringComparer.OrdinalIgnoreCase))
                    .ToArray();
            return result;
        }

        [HttpPost]
        public IActionResult CreateBook(Book book)
        {
            // Während des Model Bindings werden die Validatoren
            // von Book geprüft
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            book.Id = GetNextId();
            books.Add(book.Id, book);

            return Ok(book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, Book book)
        {
            // Während des Model Bindings werden die Validatoren
            // von Book geprüft
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!books.ContainsKey(id))
            {
                return NotFound();
            }
            book.Id = id;
            books[id] = book;
            return Ok(book);
        }

        private int GetNextId()
        {
            // TODO: only for Demo-Purposes! Not Threadsafe
            return ++lastBookId;
        }

    }
}
