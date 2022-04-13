using System.Text.Json.Serialization;

namespace UnmatchedDeckTracker.Models
{
    public class UnmatchedDeckResponse
    {
        [JsonPropertyName("decks")]
        public UnmatchedDeck[] Decks { get; set; }
    }
}
