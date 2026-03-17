using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Models
{
    public class SchoolBase
        {
            [Required]
            public required string InstitutionId { get; set; }
            [Required]
            public required string Name { get; set; }
            public string? Abbreviation { get; set; }
        }
    
}
