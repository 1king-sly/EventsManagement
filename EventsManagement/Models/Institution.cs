namespace EventsManagement.Models
{

    public class InstitutionBase
    {
        public required string Email;
        public required string Name;
        public string? Abbreviation;
    }
    public class Institution:InstitutionBase
    {
        public required string InstitutionId;
        public string? ProfileImage;
        public required DateTime CreatedAt;
        public required DateTime UpDatedAt;
    }
}
