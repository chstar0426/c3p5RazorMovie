using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
    public class AttachFile
    {

        public int ID { get; set; }

       
        public int MovieID { get; set; }

        [Display(Name = "파일명")]
        [StringLength(50)]
        [Required]
        public string FileName { get; set; }

        public Movie Movie { get; set; }

    }
}
