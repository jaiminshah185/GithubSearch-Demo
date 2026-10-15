using GithubSearchDemo.Models;

namespace GithubSearchDemo.Services
{
    public interface IStoredRecordsService
    {
        Task<StoredRecordsResponse> GetSavedRecords(int page = 1, int pageSize = 10, string? search = null, string? sortBy = null, string? sortOrder = null);
    }
}
