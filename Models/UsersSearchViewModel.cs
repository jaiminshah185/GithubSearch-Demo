using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GithubSearchDemo.Models
{
    public class UsersSearchViewModel
    {
        [Display(Name = "GitHub Username")]
        public string? UserName { get; set; }
        public GitHubUsersApiResponse? GitHubUsers { get; set; }
        public List<GitHubRepository>? GitHubRepositories { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

