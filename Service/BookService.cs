using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.models;
using System;

namespace Service
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookResponse>> GetAllAsync(string? title = null)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                booksQuery = booksQuery.Where(b => b.Title == title);
            }

            var books = await booksQuery
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price
                })
                .ToListAsync();

            return books;
        }

        public async Task<BookResponse?> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price
                })
                .FirstOrDefaultAsync();

            return book;
        }

        public async Task AddAsync(BookRequest bookRequest)
        {
            var book = new Book
            {
                Title = bookRequest.Title,
                Author = bookRequest.Author,
                Price = bookRequest.Price
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, BookRequest bookRequest)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            book.Title = bookRequest.Title;
            book.Author = bookRequest.Author;
            book.Price = bookRequest.Price;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
