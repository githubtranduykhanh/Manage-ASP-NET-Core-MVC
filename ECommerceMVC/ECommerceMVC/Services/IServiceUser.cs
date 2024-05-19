using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Services
{
    public interface IServiceUser<TServiceEntity, TReturnEntity>
    {
        Task<IEnumerable<TReturnEntity>?> GetAllAsync();
        Task<TReturnEntity?> GetByIdAsync(int id);
        Task CreateAsync(TServiceEntity entity);
        Task UpdateAsync(TServiceEntity entity);
        Task DeleteAsync(int id);

        Task<TReturnEntity?> AuthenticateAsync(LoginBaseVM formFata);

        Task<TServiceEntity?> GetRefreshToken(string refreshToken);

        Task<TServiceEntity?> AddRefreshToken(int id, string refreshToken);

        Task<IEnumerable<TServiceEntity>?> GetListByListID(List<int> listID);

        Task<List<TServiceEntity>> ImportRangeUserFormExcel(List<TServiceEntity> users);
    }
}
