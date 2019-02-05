using AuditorHelper.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Client")]
    public class Client : Auditable
    {
        protected Client() { }

        public Client(string userId,
                      string userIdContext) : base(userIdContext)
        {
            UserId = userId;
        }

        public int Id { get; private set; }
        public string UserId { get; private set; }
    }
}
