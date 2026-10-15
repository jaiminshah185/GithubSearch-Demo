namespace GithubSearchDemo.Models
{
    public class StoredRecordsResponse
    {
        public List<GitHubRepository> Items { get; set; } = new List<GitHubRepository>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
