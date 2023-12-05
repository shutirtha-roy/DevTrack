using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class UserEmailList
    {
        public Guid ProjectId { get; set; }
        public List<string> Emails { get; set; }
    }
}
