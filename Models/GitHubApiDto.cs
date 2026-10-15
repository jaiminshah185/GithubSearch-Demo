using System.Text.Json.Serialization;

namespace GithubSearchDemo.Models
{
    public class GitHubUsersApiResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("location")]
        public string? Location { get; set; }
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
    public class GitHubUserRepoResponse
    {
        [JsonPropertyName("name")]
        public string? RepoName { get; set; }
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("node_id")]
        public string? NodeId { get; set; }

        [JsonPropertyName("stargazers_count")]
        public int StargazersCount { get; set; }

        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }

}
