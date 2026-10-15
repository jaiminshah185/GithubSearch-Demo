using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GithubSearchDemo.Models
{
    public class UserApiResponse
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }
        public List<GitHubRepository>? GitHubRepositories { get; set; }
    }
}
