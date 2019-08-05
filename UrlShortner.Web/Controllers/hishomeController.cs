//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using shortid;
//using UrlShortener.Data;
//using UrlShortener.Web.Models;

//namespace UrlShortener.Web.Controllers
//{
//    public class HomeController : Controller
//    {
//        private string _connectionString;

//        public HomeController(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("ConStr");
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult ShortenUrl(string originalUrl)
//        {
//            ShortenedUrl url = null;
//            var urlRepo = new ShortenedUrlsRepository(_connectionString);
//            if (User.Identity.IsAuthenticated)
//            {
//                url = urlRepo.GetByOriginalUrlForUser(User.Identity.Name, originalUrl);
//            }
//            else
//            {
//                url = urlRepo.GetByOriginalUrl(originalUrl);
//            }

//            if (url == null)
//            {
//                var hash = ShortId.Generate(7);
//                while (urlRepo.HashExists(hash))
//                {
//                    hash = ShortId.Generate(7);
//                }
//                url = new ShortenedUrl
//                {
//                    OriginalUrl = originalUrl,
//                    UrlHash = hash
//                };
//                if (User.Identity.IsAuthenticated)
//                {
//                    var userRepo = new UserRepository(_connectionString);
//                    var user = userRepo.GetByEmail(User.Identity.Name);
//                    url.UserId = user.Id;
//                }

//                urlRepo.Add(url);
//            }

//            return Json(new { shortUrl = GetFullUrl(url.UrlHash) });
//        }

//        [Route("{hash}")]
//        public IActionResult ViewShortUrl(string hash)
//        {
//            var urlRepo = new ShortenedUrlsRepository(_connectionString);
//            var url = urlRepo.GetByHash(hash);
//            if (url == null)
//            {
//                return Redirect("/");
//            }
//            urlRepo.AddView(url.Id);
//            return Redirect(url.OriginalUrl);
//        }

//        private string GetFullUrl(string hash)
//        {
//            var fullUrl = Request.GetDisplayUrl();
//            var baseUrl = fullUrl.Replace(Request.GetEncodedPathAndQuery(), "");
//            return $"{baseUrl}/{hash}";
//        }
//    }
//}