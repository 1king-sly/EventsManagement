using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Models
{

    public class InstitutionBase
    {
        [Required,EmailAddress(ErrorMessage ="Invalid email format")]
        public required string Email { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
    public class Institution:InstitutionBase
    {

        public required string InstitutionId { get; set; }
        public string? ProfileImage { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpDatedAt { get; set; }
    }
}
