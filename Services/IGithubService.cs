using GithubSearchDemo.Models;

namespace GithubSearchDemo.Services
{
    public interface IGithubService
    {
        Task<GitHubSearchResult> SearchAndSaveUserInfoWithRepositoryAsync(string username);
    }
}
