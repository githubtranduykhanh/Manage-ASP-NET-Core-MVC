namespace ECommerceMVC.Models.User
{
    public class ModelUserClaim
    {
        public string UserId { get; set; }

        public int ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
