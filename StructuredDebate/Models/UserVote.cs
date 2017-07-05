using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StructuredDebate.Models
{
    public class UserVote
    {
        public int UserVoteID { get; set; }
        public string UserID { get; set; }
        public int? PostID { get; set; }
        public int? ArgumentID { get; set; }
        public int? CrossExaminationID { get; set; }
        public string Vote { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }
        [ForeignKey("ArgumentID")]
        public virtual Argument Argument { get; set; }
        [ForeignKey("CrossExaminationID")]
        public virtual CrossExamination CrossExamination { get; set; }

    }
}