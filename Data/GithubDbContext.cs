using GithubSearchDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubSearchDemo.Data
{
    public class GithubDbContext : DbContext
    {
        public GithubDbContext(DbContextOptions<GithubDbContext> options) : base(options)
        {

        }
        public DbSet<GitHubRepository> GitHubRepositories { get; set; }
    }
}
