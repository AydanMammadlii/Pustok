using PustokMVC.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Interfaces;

public interface ISliderServices
{
    Task<Slider> GetByIdAsync(int id);
    Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null);
    Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes);
    Task CreateAsync(Slider slider);
    Task UpdateAsync(Slider slider);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
}
