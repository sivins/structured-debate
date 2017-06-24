using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class Argument
    {
        public int ArgumentID { get; set; }
        public string Body { get; set; }
        public bool Affirmative { get; set; }
        public int CXArgumentID { get; set; }
        public int Score { get; set; }

        public virtual ICollection<Source> Sources { get; set; }
        public virtual Post Post { get; set; }
    }
}