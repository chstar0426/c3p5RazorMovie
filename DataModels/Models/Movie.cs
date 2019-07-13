using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
    public class Movie
    {

        public int ID { get; set; }
        [Display(Name ="제목")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:MM-dd}")]
        [Display(Name ="개봉일")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "장르")]
        [StringLength(50)]
        public string Gener { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName ="money")]
        //[Column(Typename ="decimal(18, 2)"]
        public decimal Price { get; set; }

        public ICollection<AttachFile> AttachFiles { get; set; }
    }
}
