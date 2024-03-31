using LocaSubs.Contracts;
using LocaSubs.DataAccess.Repositories;
using LocaSubs.Helpers;
using LocaSubs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocaSubs.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IServiceReceiver _serviceReceiver;

    public NotificationController(
        ISubscriptionRepository subscriptionRepository,
        IServiceReceiver serviceReceiver)
    {
        _subscriptionRepository = subscriptionRepository;
        _serviceReceiver = serviceReceiver;
    }

    [HttpGet("receive-notification")]
    public async Task<IActionResult> ReceiveNotification(double coordinateLat, double coordinateLon, ServiceType serviceType)
    {
        bool userLoginReceived = ClaimHelper.GetUserLogin(HttpContext, out var userLogin);
        if (!userLoginReceived) return BadRequest("Ошибка авторизации");

        var subscriptions = await _subscriptionRepository.GetUserSubscriptionsAsync(userLogin);
        var targetSubscription = subscriptions.FirstOrDefault(subscription => subscription.ServiceType == serviceType);

        if (targetSubscription is null) return NotFound();

        var nextSession = await _serviceReceiver
            .GetNextSessionAsync(coordinateLat, coordinateLon, targetSubscription.Range, serviceType);

        return Ok(nextSession);
    }
}
