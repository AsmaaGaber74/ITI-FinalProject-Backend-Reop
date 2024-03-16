using Jumia.Application.Contract;
using Jumia.Dtos.ViewModel.User;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserViewModel> CreateUserAsync(UserViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.Phone,
                // Map other properties as needed
            };

            var result = await _userRepository.CreateAsync(user);
            await _userRepository.SaveChangesAsync();

            // Map result back to UserViewModel if necessary
            return model; // This should be replaced with a proper mapping
        }

        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            // Map to UserViewModel
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                // Map other fields
            };
            return model;
        }

        public async Task<UserViewModel> UpdateUserAsync(UserViewModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            if (user == null) return null;

            // Update the user properties
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.Phone;
            // Update other properties as necessary

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            // Map updated user back to UserViewModel
            return model; // This should be replaced with proper mapping
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null) return new List<UserViewModel>();

            var userViewModels = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                // Map other necessary fields from ApplicationUser to UserViewModel
            }).ToList();

            return userViewModels;
        }


        // Implement other methods as necessary
    }
}
