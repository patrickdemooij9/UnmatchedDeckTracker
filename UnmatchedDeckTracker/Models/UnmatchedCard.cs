using System.Text.Json.Serialization;

namespace UnmatchedDeckTracker.Models
{
    public class UnmatchedCard
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        public Color GetColor()
        {
            if (Type.Equals("attack")) return Color.Red;
            if (Type.Equals("defense")) return Color.Blue;
            if (Type.Equals("versatile")) return Color.Purple;
            return Color.Yellow;
        }
    }
}
