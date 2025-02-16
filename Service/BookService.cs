using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Book>> GetAllAsync(string? title = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                return await _context.Books.Where(b => b.Title == title).ToListAsync();
            }
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id) => await _context.Books.FindAsync(id);


        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            if (!_context.Books.Any(b => b.Id == book.Id)) return false;
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
