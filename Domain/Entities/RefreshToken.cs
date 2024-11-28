namespace Tickest.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public int ExpirationInMinutes { get; set; } // Define a expiração do token
        public User User { get; set; }

        public RefreshToken() { }

        public RefreshToken(Guid userId, string token, DateTime createdDate, DateTime? deactivatedDate = null, int expirationInMinutes = 60)
        {
            UserId = userId;
            Token = token;
            CreatedDate = createdDate;
            DeactivatedDate = deactivatedDate;
            ExpirationInMinutes = expirationInMinutes;
        }
    }
}
