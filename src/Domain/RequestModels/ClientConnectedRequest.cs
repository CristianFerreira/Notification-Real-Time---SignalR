namespace Domain.RequestModels
{
    public class ClientConnectedRequest
    {
        public ClientConnectedRequest(string connectionId, string userId, int applicationId)
        {
            ConnectionId = connectionId;
            UserId = userId;
            ApplicationId = applicationId;
        }

        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
       
    }
}
