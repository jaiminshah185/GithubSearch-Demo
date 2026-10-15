using System.Diagnostics;
using GithubSearchDemo.Models;
using GithubSearchDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace GithubSearchDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGithubService _githubService;
        private readonly IStoredRecordsService _storedRecordService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IGithubService githubService, IStoredRecordsService storedRecordsService)
        {
            _logger = logger;
            _githubService = githubService;
            _storedRecordService = storedRecordsService;
        }

        public IActionResult Index()
        {
            var usersData = new UsersSearchViewModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string userName)
        {
            var usersData = new UsersSearchViewModel();
            if (string.IsNullOrEmpty(userName))
            {
                usersData.ErrorMessage = "Please enter a username to search.";
                return View(usersData);
            }
            try
            {
                var result = await _githubService.SearchAndSaveUserInfoWithRepositoryAsync(userName);
                usersData.GitHubUsers = result.GitHubUsers ?? new GitHubUsersApiResponse();
                usersData.GitHubRepositories = result.GitHubRepositories ?? new List<GitHubRepository>();
                usersData.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for user information.");
                usersData.ErrorMessage = "An error occurred while searching for user information. Please try again later.";
            }
            return View(usersData);
        }


        public async Task<IActionResult> GetStoredResult(int page = 1, int pageSize = 10, string? search = null, string? sortBy = null, string? sortOrder = null)
        {
            var model = await _storedRecordService.GetSavedRecords(page, pageSize, search, sortBy, sortOrder);
            return View(model);
        }
    }
}
