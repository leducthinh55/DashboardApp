using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MailModels
{
    public class Message
    {
        public string To { get; set; }
        public IEnumerable<string> CC { get; set; } = new List<string>();
        public string Subject { get; set; } = "";
        public string Content { get; set; } = "";
        public IFormFileCollection Attachments { get; set; }
    }
}
