using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string Claim { get; set; }
        public string OpeningStatement { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Argument> Arguments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}