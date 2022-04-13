using System.Text.Json.Serialization;

namespace UnmatchedDeckTracker.Models
{
    public class UnmatchedDeck
    {
        [JsonPropertyName("cards")]
        public UnmatchedCard[] Cards { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public UnmatchedDeck()
        {
            Cards = Array.Empty<UnmatchedCard>();
        }
    }
}
