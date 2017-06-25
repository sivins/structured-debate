using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class TagRelation
    {
        public int TagRelationID { get; set; }
        public int TagID { get; set; }
        public int PostID { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Post Post { get; set; }
    }
}