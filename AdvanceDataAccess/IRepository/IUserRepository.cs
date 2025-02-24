using AdvanceDataAccess.Models;

namespace AdvanceDataAccess.IRepository
{
    public abstract class  IUserRepository
    {
        public abstract Task<IEnumerable<Users>> GetUsersAsync();
         public abstract Task<Users> GetUserByIdAsync(int id);
         public abstract Task AddUserAsync(Users user);
        public abstract Task BulkInsertUsersAsync(List<Users> users);
    }
}
