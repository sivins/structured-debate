using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class Source
    {
        public int SourceID { get; set; }
        public int? ArgumentID { get; set; }
        public int? CrossExaminationID { get; set; }
        public string URL { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}