namespace EventsManagement.DTOs
{
    public class ResetPasswordDto
    {
        public required string Password { get; set; }

        public required string NewPassword { get; set; }

        public required string UserId { get; set; }
    }
}
