namespace Domain.AbstractionModels
{
    public class CacheValueAbstraction
    {
        public CacheValueAbstraction(string userId, int applicationId)
        {
            UserId = userId;
            ApplicationId = applicationId;
        }

        public string UserId { get; set; }
        public int ApplicationId { get; set; }
    }
}
