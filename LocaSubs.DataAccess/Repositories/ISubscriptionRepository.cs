using LocaSubs.Models;

namespace LocaSubs.DataAccess.Repositories;

public interface ISubscriptionRepository
{
    Task<long> AddAsync(Subscription subscription);
    Task RemoveAsync(long subscriptionId, string userLogin);
    Task<IList<Subscription>> GetUserSubscriptionsAsync(string userLogin);
}
