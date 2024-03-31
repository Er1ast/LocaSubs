using LocaSubs.DataAccess.Repositories;
using LocaSubs.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocaSubs.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string login, string password)
    {
        var userExist = await _userRepository.ExistAsync(login);
        if (userExist)
        {
            return BadRequest($"Пользователь {login} уже существует");
        }

        User user = new(login, password);
        await _userRepository.CreateUserAsync(user);
        return Ok();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(string login, string password)
    {
        var user = await _userRepository.GetUserAsync(login, password);

        if (user is null)
        {
            return NotFound("Неверный логин или пароль");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
        };

        ClaimsIdentity claimsIdentity = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Ok();
    }

    [Authorize]
    [HttpPost("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        return Ok(users);
    }
}
