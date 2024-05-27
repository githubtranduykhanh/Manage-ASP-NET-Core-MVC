using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Repositorys.User
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly ECommerceContext _dbContext;

        public RepositoryUser(ECommerceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DbUser>> GetAllAsync()
        {
            return await _dbContext.DbUsers.Include(u => u.IdRoleNavigation).ToListAsync();
        }

        public async Task<DbUser?> GetByIdAsync(int id)
        {
            return await _dbContext.DbUsers.Include(u => u.IdRoleNavigation).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(DbUser entity)
        {
            _dbContext.DbUsers.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DbUser entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.DbUsers.FindAsync(id);
            if (entity != null)
            {
                _dbContext.DbUsers.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<DbUser?> AuthenticateAsync(LoginBaseVM formData)
        {
            return await _dbContext.DbUsers.Include(u => u.IdRoleNavigation).FirstOrDefaultAsync(item => (item.Email == formData.emailorphone || item.Phone == formData.emailorphone) && item.Password == formData.passwordlogin);          
        }

        public async Task<DbUser?> GetRefreshToken(string refreshToken)
        {
            return await _dbContext.DbUsers.Include(u => u.IdRoleNavigation).FirstOrDefaultAsync(item => item.RefreshToken == refreshToken);
        }

        public async Task<DbUser?> AddRefreshToken(int id, string refreshToken)
        {
            var entity = await _dbContext.DbUsers.FindAsync(id);
            if (entity == null) return null;
            entity.RefreshToken = refreshToken;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<DbUser>> GetListByListID(List<int> listID)
        {
            return await _dbContext.DbUsers.Where(p => listID.Contains(p.Id)).ToListAsync();
        }

        public async Task<DbUser?> GetUserWithRoleAsync(int id)
        {
            return await _dbContext.DbUsers
                .Include(d => d.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<DbUser?> IsRegisterAsync(DbUser user)
        {
            return await _dbContext.DbUsers.FirstOrDefaultAsync(item => item.Email.Equals(user.Email) || item.Phone.Equals(user.Phone));
        }

        public async Task<IEnumerable<DbRole>?> GetRolesAsync()
        {
            return await _dbContext.DbRoles.ToListAsync();
        }
    }
}
