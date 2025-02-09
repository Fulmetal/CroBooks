using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserDto?> GetUser(int id)
        {
            var user = await unitOfWork.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.ToDto();
        }
    }
}
