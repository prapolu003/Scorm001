using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCORM_Razor.Data.Models
{
    public partial class cmi_interactions_correct_responses
    {
        [Key]
        public int id { get; set; }
        public int? interactions_id { get; set; }
        public int? n { get; set; }
        [StringLength(8000)]
        public string pattern { get; set; }
    }
}
