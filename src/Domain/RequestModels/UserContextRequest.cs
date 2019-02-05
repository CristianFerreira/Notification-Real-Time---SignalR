using Newtonsoft.Json;

namespace Domain.RequestModels
{
    public class UserContextRequest
    {
        [JsonIgnore]
        public string UserContextId { get; private set; }

        public void SetUserContext(string userId)
        => UserContextId = userId;
    }
}
