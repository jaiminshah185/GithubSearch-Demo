namespace GithubSearchDemo.Models
{
    public class GitHubSearchResult
    {
        public GitHubUsersApiResponse? GitHubUsers { get; set; }
        public List<GitHubRepository>? GitHubRepositories { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
