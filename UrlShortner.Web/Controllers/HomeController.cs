using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UrlShortner.Web.Models;
using URLShortner.Data;

namespace UrlShortner.Web.Controllers
{
    public class HashedObj
    {
        public string HashedUrl { get; set; }
    }
    public class HomeController : Controller
    {
        private string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ShortenUrl(string url)
        {
            var authDb = new UserRepository(_connectionString);
            var repo = new UrlRepository(_connectionString);
            UrlLink ul = new UrlLink();
            if (User.Identity.IsAuthenticated)
            {
                ul.HashedUrl = repo.GetHashedUrlForUser(url, authDb.GetIdForEmail(User.Identity.Name));
            }
            else
            {
                ul.HashedUrl = repo.GetHashedUrl(url);
            }

            if (ul.HashedUrl == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    repo.AddUrl(url, authDb.GetIdForEmail(User.Identity.Name));
                }
                else
                {
                    repo.AddUrl(url, null);
                }
                ul.HashedUrl= repo.GetHashedUrl(url);
            }
            return Json(new HashedObj { HashedUrl = GetFullUrl(ul.HashedUrl) });

        }

        [Route("{hash}")]
        public IActionResult ViewShortUrl(string hash)
        {
            var repo = new UrlRepository(_connectionString);
            var url = repo.GetULByHash(hash);
            if (url == null)
            {
                return Redirect("/");
            }
            //var ul = repo.GetULByHash(hash);
            repo.AddView(url.Id);
            return Redirect(url.Url);
        }

        private string GetFullUrl(string hash)
        {
            var fullUrl = Request.GetDisplayUrl();
            var baseUrl = fullUrl.Replace(Request.GetEncodedPathAndQuery(), "");
            return $"{baseUrl}/{hash}";
        }
    }
}
