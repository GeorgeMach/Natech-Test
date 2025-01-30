using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AwesomeCatApi.Models
{
    public class TagEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("cats")]
        public ICollection<CatEntity> Cats { get; set; } = new List<CatEntity>();
    }
}