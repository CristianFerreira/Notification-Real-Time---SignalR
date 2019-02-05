namespace Domain.RequestModels
{
    public class NotifyAllClientRequest
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string OwnerId { get; set; }
        public int ApplicationId { get; set; }
    }
}
