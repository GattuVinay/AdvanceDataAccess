using AdvanceDataAccess.Data;
using AdvanceDataAccess.IRepository;
using AdvanceDataAccess.Models;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using EFCore.BulkExtensions;

namespace AdvanceDataAccess.Repository
{
        public class UserRepository : IUserRepository
        {
            private readonly AppDbContext _context;
            private readonly string _connectionString;
            private readonly IDatabase _cache;

            public UserRepository(AppDbContext context, IConfiguration configuration, IConnectionMultiplexer redis)
            {
                _context = context;
                _connectionString = configuration.GetConnectionString("DefaultConnection");
                _cache = redis.GetDatabase();
            }

            // Fetch users with caching
            public async Task<IEnumerable<Users>> GetUsersAsync()
            {
                string cacheKey = "users";
                var cachedUsers = await _cache.StringGetAsync(cacheKey);

                if (!cachedUsers.IsNullOrEmpty)
                    return System.Text.Json.JsonSerializer.Deserialize<List<Users>>(cachedUsers);

                var users = await _context.Users.ToListAsync();
                await _cache.StringSetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(users), TimeSpan.FromMinutes(10));
                return users;
            }

            // Fetch single user with Dapper
            public async Task<Users> GetUserByIdAsync(int id)
            {
                using var connection = new SqlConnection(_connectionString);
                return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            }

            // Add a user with EF Core
            public async Task AddUserAsync(Users users)
            {
                await _context.Users.AddAsync(users);
                await _context.SaveChangesAsync();
            }

            // Bulk Insert Users using EFCore.BulkExtensions
            public async Task BulkInsertUsersAsync(List<Users> users)
            {
                await _context.BulkInsertAsync(users);
            }
        }
    
}
