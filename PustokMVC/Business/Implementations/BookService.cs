using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.CustomExceptions.Exceptions;
using PustokMVC.CustomExceptions.GenreExceptions;
using PustokMVC.Data;
using PustokMVC.Models;
using System.Linq;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations;

public class BookService : IBookService
{
    private readonly PustokDbContext _context;

    public BookService(PustokDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Book book)
    {
        if (_context.Books.Any(x => x.BookCode.ToLower() == book.BookCode.ToLower()))
            throw new BookCodeAlreadyExistException("BookCode", "BookCode is already exist!");

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Books.FindAsync(id);
        if (data is null) throw new BookNotFoundException("Book is not found!");

        _context.Remove(data);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Books.AsQueryable(); 

        query = _getIncludes(query, includes);

        return expression is not null
                ? await query.Where(expression).ToListAsync()  
                : await query.ToListAsync(); 
    }

    private IQueryable<Book> _getIncludes(IQueryable<Book> query, string[] includes)
    {
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var data = await _context.Books.FindAsync(id);
        if (data is null) throw new BookNotFoundException();

        return data;
    }

    public async Task<Book> GetSingleAsync(Expression<Func<Book, bool>>? expression = null)
    {
        var query = _context.Books.AsQueryable();

        return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var data = await _context.Books.FindAsync(id);
        if (data is null) throw new BookNotFoundException();
        data.IsDeleted = !data.IsDeleted;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var existData = await _context.Books.FindAsync(book.Id);
        if (existData is null) throw new BookNotFoundException("Book is not found!");
        if (_context.Books.Any(x => x.BookCode.ToLower() == book.BookCode.ToLower())
            && existData.BookCode != book.BookCode)
            throw new BookCodeAlreadyExistException("Book", "BookCode is already exist!");

        existData.BookCode = book.BookCode;
        await _context.SaveChangesAsync();
    }
}
