using EventsManagement.Models;

namespace EventsManagement.DTOs
{
    public class SchoolCreateDto:SchoolBase
    {
    }
    public class SchoolUpdateDto:SchoolBase
    {
        public required string schoolId { get; set; }
    }

    public class SchoolOutDto:SchoolBase
    {
        public required string SchoolId { get; set; }
        public string? ProfileImage { get; set; }

        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
