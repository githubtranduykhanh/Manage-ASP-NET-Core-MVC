using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Repositorys.User
{
    public interface IRepositoryUser
    {
        Task<DbUser?> GetByIdAsync(string id);
        Task<IEnumerable<DbUser>> GetAllAsync();
        Task CreateAsync(DbUser entity);
        Task UpdateAsync(DbUser entity);
        Task DeleteAsync(string id);

        Task<DbUser?> AuthenticateAsync(LoginBaseVM formData);

        Task<DbUser?> GetRefreshToken(string refreshToken);


        Task<DbUser?> AddRefreshToken(string id, string refreshToken);

        Task<IEnumerable<DbUser>> GetListByListID(List<string> listID);

        Task<DbUser?> GetUserWithRoleAsync(string id);


        Task<DbUser?> IsRegisterAsync(DbUser user);

       
    }
}
