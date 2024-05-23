using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Repositorys.User
{
    public interface IRepositoryUser
    {
        Task<DbUser?> GetByIdAsync(int id);
        Task<IEnumerable<DbUser>> GetAllAsync();
        Task CreateAsync(DbUser entity);
        Task UpdateAsync(DbUser entity);
        Task DeleteAsync(int id);

        Task<DbUser?> AuthenticateAsync(LoginBaseVM formData);

        Task<DbUser?> GetRefreshToken(string refreshToken);


        Task<DbUser?> AddRefreshToken(int id, string refreshToken);

        Task<IEnumerable<DbUser>> GetListByListID(List<int> listID);

        Task<DbUser?> GetUserWithRoleAsync(int id);


        Task<DbUser?> IsRegisterAsync(DbUser user);
    }
}
