using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AwesomeCatApi.Models
{
    public class CatEntity
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("external_id")]
        public string CatId { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("tags")]
        public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();
    }
}