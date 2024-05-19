﻿using ECommerceMVC.Data;
using ECommerceMVC.Repositorys;
using ECommerceMVC.Repositorys.User;
using ECommerceMVC.ViewModels;
using NuGet.Protocol.Core.Types;

namespace ECommerceMVC.Services
{
    public class ServiceUser : IServiceUser<DbUser, UserVM>
    {
        private readonly IRepositoryUser _userRepository;
        public ServiceUser(IRepositoryUser userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(DbUser user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserVM>?> GetAllAsync()
        {
            IEnumerable<DbUser> list = await _userRepository.GetAllAsync();
            if (list == null) return null;
            return list.Select(item => new UserVM()
            {
                id = item.Id,
                idRole = item.IdRole,
                roleName = item.IdRoleNavigation.Name,               
                loginType = item.LoginType,
                name = item.Name,
                phone = item.Phone,
                refreshToken = item.RefreshToken ?? "",
                address = item.Address ?? "",
                avatar = item.Avatar ?? "",
                createdAt = item.CreatedAt,
                email = item.Email,
                sex = item.Sex,
                status = item.Status,
                securityQuestion = item.SecurityQuestion ?? ""
            });
        }

        public async Task<UserVM?> GetByIdAsync(int id)
        {
            var item = await _userRepository.GetByIdAsync(id);
            if (item == null) return null;
            return new UserVM()
            {
                id = item.Id,
                idRole = item.IdRole,
                roleName = item.IdRoleNavigation.Name,
                loginType = item.LoginType,
                name = item.Name,
                phone = item.Phone,
                refreshToken = item.RefreshToken ?? "",
                address = item.Address ?? "",
                avatar = item.Avatar ?? "",
                createdAt = item.CreatedAt,
                email = item.Email ?? "",
                sex = item.Sex,
                status = item.Status,
                securityQuestion = item.SecurityQuestion ?? ""
            };
        }




        public async Task UpdateAsync(DbUser entity)
        {

            await _userRepository.UpdateAsync(entity);
        }


        public async Task<UserVM?> AuthenticateAsync(LoginBaseVM formFata)
        {
            var item = await _userRepository.AuthenticateAsync(formFata);
            if (item == null) return null;
            return new UserVM()
            {
                id = item.Id,
                idRole = item.IdRole,
                roleName = "User",
                loginType = item.LoginType,
                name = item.Name,
                phone = item.Phone,
                refreshToken = item.RefreshToken ?? "",
                address = item.Address ?? "",
                avatar = item.Avatar ?? "",
                createdAt = item.CreatedAt,
                email = item.Email ?? "",
                sex = item.Sex,
                status = item.Status,
                securityQuestion = item.SecurityQuestion ?? ""
            };
        }


        public async Task<DbUser?> GetRefreshToken(string refreshToken)
        {
            return await _userRepository.GetRefreshToken(refreshToken);
        }

        public async Task<DbUser?> AddRefreshToken(int id, string refreshToken)
        {
            return await _userRepository.AddRefreshToken(id, refreshToken);
        }


        public async Task<IEnumerable<DbUser>?> GetListByListID(List<int> listID)
        {
            return await _userRepository.GetListByListID(listID);
        }



        public async Task<List<DbUser>> ImportRangeUserFormExcel(List<DbUser> users)
        {
            var updateTasks = new List<Task>();

            // Bắt đầu cập nhật hoặc tạo mới từng người dùng trong danh sách
            foreach (var user in users)
            {
                var userFind = await _userRepository.GetByIdAsync(user.Id);
                if (userFind != null)
                {
                    // Cập nhật thông tin người dùng nếu đã tồn tại
                    userFind.Name = user.Name;
                    userFind.Email = user.Email;
                    userFind.Phone = user.Phone;
                    userFind.Address = user.Address;
                    userFind.Sex = user.Sex;
                    userFind.IdRole = user.IdRole;
                    userFind.Status = user.Status;
                    userFind.LoginType = user.LoginType;
                    userFind.CreatedAt = user.CreatedAt;
                    updateTasks.Add(_userRepository.UpdateAsync(userFind));
                }
                else
                {
                    // Tạo mới người dùng nếu chưa tồn tại
                    updateTasks.Add(_userRepository.CreateAsync(user));
                }
            }

            // Chờ tất cả các tác vụ cập nhật hoặc tạo mới hoàn thành
            await Task.WhenAll(updateTasks);

            // Trả về danh sách người dùng sau khi đã hoàn thành tất cả các tác vụ
            return users;
        }
    }
}
