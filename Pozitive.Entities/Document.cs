using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitive.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string File { get; set; }
    }
}
