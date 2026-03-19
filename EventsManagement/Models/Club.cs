namespace EventsManagement.Models
{
    public class ClubBase
    {
        public string? SchoolId { get; set; }
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }

        public required string UserId { get; set; }
    }

    public class Club:ClubBase
    {
        public required string ClubId { get; set; }
        public required DateTime CreatedAt  { get; set; }
        public required DateTime UpdatedAt  { get; set; }

        public string? ProfileImage { get; set; }

    }
}
