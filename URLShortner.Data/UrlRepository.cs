using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shortid;
using System.Data.SqlClient;

namespace URLShortner.Data
{
    public class UrlRepository
    {
        private string _connectionString;

        public UrlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<UrlLink> GetUrlsPerUser(int id)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                return uc.UrlLinks.Include(u => u.UserId).Where(a => a.UserId == id).ToList();
            }
        }
        public string AddUrl(String url, int? userId)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                var hash = ShortId.Generate(7);
                while (CheckHashExists(hash))
                {
                    hash = ShortId.Generate(7);
                }
                var u = new UrlLink { Url = url, HashedUrl = hash, UserId = userId };
                uc.UrlLinks.Add(u);
                uc.SaveChanges();
                return hash;
            }
        }
        public void AddView(int urlId)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                uc.Database.ExecuteSqlCommand("UPDATE urlLinks SET Views = Views + 1 WHERE Id = @id",
                new SqlParameter("@id", urlId));
            }
        }
        public string GetHashedUrlForUser(string url, int userId)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                if (uc.UrlLinks.FirstOrDefault(u => (u.Url == url) && (u.UserId == userId)) != null)
                {
                    return uc.UrlLinks.FirstOrDefault(u => (u.Url == url) && (u.UserId == userId)).HashedUrl;
                }
                return null;

            }
        }

        public string GetHashedUrl(string url)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                if (uc.UrlLinks.Any(u => (u.Url == url)))
                {
                    return uc.UrlLinks.FirstOrDefault(u => (u.Url == url)).HashedUrl;
                }
                return null;
            }
        }

        public bool CheckHashExists(string hash)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                return uc.UrlLinks.Any(u => u.HashedUrl == hash);
            }
        }
        public UrlLink GetULByHash(string url)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                return uc.UrlLinks.FirstOrDefault(u => u.HashedUrl == url);
            }
        }

        public IEnumerable<UrlLink> GetUrlsForUser(int userId)
        {
            using (var uc = new UrlContext(_connectionString))
            {
                return uc.UrlLinks.Where(u => u.UserId == userId).ToList();
            }
        }
    }
}
