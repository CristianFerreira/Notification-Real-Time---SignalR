using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AbstractionModels
{
    public class NotifyClientAbstraction
    {
        public NotifyClientAbstraction()
        {
            UsersId = new List<string>();
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public IList<string> UsersId { get; set; }
    }
}
