﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace StructuredDebate.Models
{
    public class Argument
    {
        public int ArgumentID { get; set; }
        public int PostID { get; set; }
        public string Body { get; set; }
        public bool Affirmative { get; set; }
        public int Score { get; set; }

        public virtual ICollection<Source> Sources { get; set; }
        public virtual ICollection<CrossExamination> CrossExaminations { get; set; }
        public virtual Post Post { get; set; }
    }
}