using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GithubSearchDemo.Models
{
    public class GitHubRepository
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? NodeId { get; set; }
        public int StargazersCount { get; set; }
        public string? RepositoryUrl { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }

        [Column("CreatedAtUtc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
