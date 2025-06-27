using System.Text.Json.Serialization;

namespace Rest_API_CV.DTO
{
    public class GithubRepoDto
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
    }
}

