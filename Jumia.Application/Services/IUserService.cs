using Jumia.Dtos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public interface IUserService
    {
        Task<UserViewModel> CreateUserAsync(UserViewModel model);
        Task<UserViewModel> GetUserByIdAsync(string id);
        Task<UserViewModel> UpdateUserAsync(UserViewModel model);
        Task<bool> DeleteUserAsync(string id);
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        // Add other necessary methods
    }
}
