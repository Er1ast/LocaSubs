using LocaSubs.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaSubs.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LocaSubsDbContext _dbContext;

    public UserRepository(LocaSubsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(string login)
    {
        return await _dbContext.Users.AnyAsync(user => user.Login == login);
    }

    public async Task<User> GetUserAsync(string login, string password)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
        
        return existingUser!;
    }

    public async Task<IReadOnlyCollection<User>> GetUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }
}
