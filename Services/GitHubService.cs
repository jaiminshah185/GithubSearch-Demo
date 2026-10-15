using GithubSearchDemo.Data;
using GithubSearchDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubSearchDemo.Services
{
    public class GitHubService : IGithubService
    {
        private readonly GithubDbContext _context;
        private readonly HttpClient _httpClient;
        public GitHubService(GithubDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<GitHubSearchResult> SearchAndSaveUserInfoWithRepositoryAsync(string username)
        {
            HttpResponseMessage userInfo;
            try
            {
                userInfo = await _httpClient.GetAsync($"users/{username}");
            }

            catch (Exception ex)
            {
                return new GitHubSearchResult
                {
                    ErrorMessage = $"Error occurred while fetching user info: {ex.Message}"
                };
            }
            if (userInfo.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new GitHubSearchResult
                {
                    ErrorMessage = $"User '{username}' not found."
                };
            }
            if (userInfo.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return new GitHubSearchResult
                {
                    ErrorMessage = "GitHub API rate limit exceeded. Please try again later."
                };
            }
            var userAPIResponse = new GitHubUsersApiResponse();
            if (userInfo.IsSuccessStatusCode)
            {
                userAPIResponse = await userInfo.Content.ReadFromJsonAsync<GitHubUsersApiResponse>() ?? new GitHubUsersApiResponse();
                if (userAPIResponse == null)
                {
                    return new GitHubSearchResult
                    {
                        ErrorMessage = "Failed to get user details from the GitHub"
                    };
                }
            }

            var newRepositories = new List<GitHubRepository>();
            try
            {
                var newRepositoriesResponse = await _httpClient.GetAsync($"users/{Uri.EscapeDataString(username)}/repos?per_page=100");
                if (newRepositoriesResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return new GitHubSearchResult
                    {
                        GitHubUsers = userAPIResponse,
                        ErrorMessage = "GitHub API rate limit exceeded. Please try again later."
                    };
                }
                if (!newRepositoriesResponse.IsSuccessStatusCode)
                {
                    return new GitHubSearchResult
                    {
                        GitHubUsers = userAPIResponse,
                        ErrorMessage = $"Error occurred while fetching repositories: {newRepositoriesResponse.ReasonPhrase}"
                    };
                }

                var reposData = await newRepositoriesResponse.Content.ReadFromJsonAsync<List<GitHubUserRepoResponse>>();
                if (reposData != null)
                {
                    var topRepos = reposData.OrderByDescending(x => x.StargazersCount).Take(5).ToList();
                    foreach (var item in topRepos)
                    {
                        var repo = new GitHubRepository
                        {
                            Id = item.Id,
                            Username = username,
                            Name = item.RepoName ?? string.Empty,
                            StargazersCount = item.StargazersCount,
                            NodeId = item.NodeId ?? string.Empty,
                            RepositoryUrl = item.HtmlUrl ?? string.Empty,
                            Description = item.Description ?? string.Empty,
                            Language = item.Language ?? string.Empty
                        };
                        newRepositories.Add(repo);

                        var existingRepo = await _context.GitHubRepositories.FirstOrDefaultAsync(r => r.Id == repo.Id);
                        if (existingRepo == null)
                        {
                            await _context.GitHubRepositories.AddAsync(repo);
                        }
                        else
                        {
                            existingRepo.Name = repo.Name;
                            existingRepo.NodeId = repo.NodeId;
                            existingRepo.StargazersCount = repo.StargazersCount;
                            existingRepo.Username = repo.Username;
                            existingRepo.RepositoryUrl = repo.RepositoryUrl;
                            existingRepo.Description = repo.Description;
                            existingRepo.Language = repo.Language;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new GitHubSearchResult
                {
                    GitHubUsers = userAPIResponse,
                    ErrorMessage = $"Error occurred while fetching repositories: {message}"
                };
            }

            
            return new GitHubSearchResult
            {
                GitHubUsers = userAPIResponse,
                GitHubRepositories = newRepositories
            };
        }
    }
}
