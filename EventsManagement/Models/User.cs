namespace EventsManagement.Models
{
    public class User(Guid userId, string firstName,string lastName,string email,string password,string profileImage,string? institutionId,string? schoolId)
    {
        public Guid UserId { get; set; } = userId;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string  Email { get; set; } = email;

        public string Password { get; set; } = password;
        public string ProfileImage { get; set; } = profileImage;
        public string? InstitutionId { get; set; }=institutionId;
        public string? SchoolId {  get; set; }=schoolId; 
    }
}
