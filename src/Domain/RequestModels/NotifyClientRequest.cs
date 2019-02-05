using System.Collections.Generic;

namespace Domain.RequestModels
{
    public class NotifyClientRequest : UserContextRequest
    {
        public NotifyClientRequest()
        {
            UsersId = new List<string>();
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Owner { get; set; }
        public int ApplicationId { get; set; }
        public IList<string> UsersId { get; set; }
    }
}
