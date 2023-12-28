using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    [Table("Record")]
    public class Record
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }
        [ForeignKey("BService")]
        public int BServiceId { get; set; }
        public BService BService { get; set; }
        [ForeignKey("Master")]
        public int MasterId { get; set; }  
        public Master Master { get; set; }

    }
}
