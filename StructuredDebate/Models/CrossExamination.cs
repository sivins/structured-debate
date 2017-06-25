using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class CrossExamination
    {
        public int CrossExaminationID { get; set; }
        public int ArgumentID { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }

        public virtual ICollection<Source> Sources { get; set; }
        public virtual Argument Argument { get; set; }
    }
}