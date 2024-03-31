using LocaSubs.Models;

namespace LocaSubs.Requests;

public class SubscribeRequest
{
    public long Range { get; init; }
    public ServiceType ServiceType { get; init; }

    public Subscription ToSubscription(string userLogin)
    {
        return new Subscription
        {
            Id = 0,
            UserLogin = userLogin,
            Range = this.Range,
            ServiceType = ServiceType
        };
    }
}