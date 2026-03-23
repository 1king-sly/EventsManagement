using System.Text.Json.Serialization;

namespace EventsManagement.DTOs
{
    public enum LeaderType { Institution, Club, School }

    public class LeaderOutDto:UserOutDto
    {
        public required DateTime StartDate { get; set; } 
    }

    public class LeaderInDto
    {
        public required string UserId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public  LeaderType LeaderType { get; set; } = LeaderType.Club;

        public  DateTime StartDate { get; set; } = DateTime.UtcNow;
    }

}
