using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FileModels
{
    public class FileSupport
    {
        public byte[] Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
