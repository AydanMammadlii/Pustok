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

public class SliderService : ISliderServices
{
    private readonly PustokDbContext _context;

    public SliderService(PustokDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Slider slider)
    {
        if (_context.Sliders.Any(x => x.ImageUrl.ToLower() == slider.ImageUrl.ToLower()))
            throw new ImageUrlAlreadyExistException("ImageUrl", "Slider is already exist!");

        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Sliders.FindAsync(id);
        if (data is null) throw new SliderNotFoundException("Slider is not found!");

        _context.Remove(data);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Sliders.AsQueryable(); 

        query = _getIncludes(query, includes);

        return expression is not null
                ? await query.Where(expression).ToListAsync()  
                : await query.ToListAsync(); 
    }

    private IQueryable<Slider> _getIncludes(IQueryable<Slider> query, string[] includes)
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

    public async Task<Slider> GetByIdAsync(int id)
    {
        var data = await _context.Sliders.FindAsync(id);
        if (data is null) throw new SliderNotFoundException();

        return data;
    }

    public async Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null)
    {
        var query = _context.Sliders.AsQueryable();

        return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var data = await _context.Sliders.FindAsync(id);
        if (data is null) throw new SliderNotFoundException();
        data.IsDeleted = !data.IsDeleted;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Slider slider)
    {
        var existData = await _context.Sliders.FindAsync(slider.Id);
        if (existData is null) throw new GenreNotFoundException("Slider is not found!");
        if (_context.Sliders.Any(x => x.ImageUrl.ToLower() == slider.ImageUrl.ToLower())
            && existData.ImageUrl != slider.ImageUrl)
            throw new ImageUrlAlreadyExistException("ImageUrl", "Slider is already exist!");

        existData.ImageUrl = slider.ImageUrl;
        await _context.SaveChangesAsync();
    }
}
