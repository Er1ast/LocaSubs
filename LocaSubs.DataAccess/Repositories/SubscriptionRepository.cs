using LocaSubs.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaSubs.DataAccess.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly LocaSubsDbContext _dbContext;

    public SubscriptionRepository(LocaSubsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> AddAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
        return subscription.Id;
    }

    public async Task<IList<Subscription>> GetUserSubscriptionsAsync(string userLogin)
    {
        var userSubscriptions = _dbContext.Subscriptions.Where(subscription => subscription.UserLogin == userLogin);
        return await userSubscriptions.ToListAsync();
    }

    public async Task RemoveAsync(long subscriptionId, string userLogin)
    {
        var subscriptionToRemove = _dbContext.Subscriptions
            .FirstOrDefault(subscription => subscription.Id == subscriptionId && subscription.UserLogin == userLogin);
        
        if (subscriptionToRemove is null)
        {
            throw new InvalidOperationException($"Подписка с id {subscriptionId} у пользователя {userLogin} не найдена");
        }
        _dbContext.Subscriptions.Remove(subscriptionToRemove);
        await _dbContext.SaveChangesAsync();
    }
}
