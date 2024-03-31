using LocaSubs.DataAccess.Repositories;
using LocaSubs.Helpers;
using LocaSubs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LocaSubs.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeAsync([FromBody][Required] SubscribeRequest request)
        {
            bool userLoginReceived = ClaimHelper.GetUserLogin(HttpContext, out var userLogin);
            if (!userLoginReceived) return BadRequest("Ошибка авторизации");

            var subscription = request.ToSubscription(userLogin);
            await _subscriptionRepository.AddAsync(subscription);
            return Ok(subscription.Id);
        }

        [HttpDelete("unsubscribe")]
        public async Task<IActionResult> UnsubscribeAsync([FromBody][Required] long subscriptionId)
        {
            bool userLoginReceived = ClaimHelper.GetUserLogin(HttpContext, out var userLogin);
            if (!userLoginReceived) return BadRequest("Ошибка авторизации");

            try
            {
                await _subscriptionRepository.RemoveAsync(subscriptionId, userLogin);
                return Ok();
            }
            catch (InvalidOperationException ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetUserSubscruptions()
        {
            bool userLoginReceived = ClaimHelper.GetUserLogin(HttpContext, out var userLogin);
            if (!userLoginReceived) return BadRequest("Ошибка авторизации");

            var subscriptions = await _subscriptionRepository.GetUserSubscriptionsAsync(userLogin);
            return Ok(subscriptions); 
        }
    }
}