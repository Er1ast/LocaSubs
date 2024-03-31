using System.ComponentModel.DataAnnotations;

namespace LocaSubs.Models;

public class User
{
    [Key]
    public string Login { get; set; }
    public string Password { get; set; }

    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }
}