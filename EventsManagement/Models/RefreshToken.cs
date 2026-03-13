namespace EventsManagement.Models
{
    public class RefreshToken
    {
        public required string UserId;
        public required string Token;
        public required DateTime ExpiresAt;
    }
}
