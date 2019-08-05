using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using URLShortner.Data;

namespace UrlShortner.Web.Controllers
{
    public class AccountController : Controller
    {
        private IHostingEnvironment _environment;
        private string _connectionString;

        public AccountController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            UserRepository repository = new UserRepository(_connectionString);
            repository.AddUser(user);
            return Redirect("/account/logIn");
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string email, string password)
        {
            UserRepository repository = new UserRepository(_connectionString);
            string passwordFromDB = repository.GetPasswordForEmail(email);
            if (passwordFromDB == null)
            {
                return Redirect("/account/logIn");
            }

            var claims = new List<Claim>
            {
                new Claim ("user",email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var urlRepo = new UrlRepository(_connectionString);
            var authDb = new UserRepository(_connectionString);
            var urls = urlRepo.GetUrlsForUser(authDb.GetIdForEmail(User.Identity.Name));
            return View(urls);
        }
    }
}