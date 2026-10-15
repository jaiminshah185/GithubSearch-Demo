using GithubSearchDemo.Data;
using GithubSearchDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubSearchDemo.Services
{
    public class StoredRecordsService : IStoredRecordsService
    {
        private readonly GithubDbContext _context;
        public StoredRecordsService(GithubDbContext context)
        {
            _context = context;
        }
        public async Task<StoredRecordsResponse> GetSavedRecords(int page = 1, int pageSize = 10, string? search = null, string? sortBy = null, string? sortOrder = null)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            var query = BuildSearchQuery(search);
            var totalCount = await query.CountAsync();

            query = ApplySorting(query, sortBy, sortOrder);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new StoredRecordsResponse
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Search = search,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
        }
        public async Task<List<GitHubRepository>> GetAllRepositoriesAsync(string? search = null)
        {
            var query = BuildSearchQuery(search);
            return await query.OrderByDescending(x => x.StargazersCount).ThenByDescending(r => r.CreatedDate).ToListAsync();
        }

        #region Private methods
        private IQueryable<GitHubRepository> BuildSearchQuery(string? search)
        {
            var query = _context.GitHubRepositories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                query = query.Where(r => (r.Name != null && r.Name.Contains(trimmed))
                                      || (r.NodeId != null && r.NodeId.Contains(trimmed))
                                      || (r.Username != null && r.Username.Contains(trimmed)));
            }
            return query;
        }

        private IQueryable<GitHubRepository> ApplySorting(IQueryable<GitHubRepository> query, string? sortBy, string? sortOrder)
        {
            bool isDesc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            return sortBy?.ToLower() switch
            {
                "name" => isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "nodeid" => isDesc ? query.OrderByDescending(x => x.NodeId) : query.OrderBy(x => x.NodeId),
                "username" => isDesc ? query.OrderByDescending(x => x.Username) : query.OrderBy(x => x.Username),
                "stars" or "stargazerscount" => isDesc ? query.OrderByDescending(x => x.StargazersCount) : query.OrderBy(x => x.StargazersCount),
                _ => isDesc ? query.OrderBy(x => x.StargazersCount).ThenBy(r => r.CreatedDate)
                            : query.OrderByDescending(x => x.StargazersCount).ThenByDescending(r => r.CreatedDate)
            };
        }
        #endregion
    }
}
