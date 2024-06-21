using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Services.User
{
    public interface IServiceUser<TServiceEntity, TReturnEntity>
    {
        Task<IEnumerable<TReturnEntity>?> GetAllAsync();
        Task<TReturnEntity?> GetByIdAsync(string id);
        Task CreateAsync(TServiceEntity entity);
        Task UpdateAsync(TServiceEntity entity);
        Task DeleteAsync(string id);

        Task<TReturnEntity?> AuthenticateAsync(LoginBaseVM formFata);

        Task<TServiceEntity?> GetRefreshToken(string refreshToken);

        Task<TServiceEntity?> AddRefreshToken(string id, string refreshToken);

        Task<IEnumerable<TServiceEntity>?> GetListByListID(List<string> listID);

        Task<List<TServiceEntity>> ImportRangeUserFormExcel(List<TServiceEntity> users);

        Task<TServiceEntity?> IsRegisterAsync(TServiceEntity user);


    
    }
}
