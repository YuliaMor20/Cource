using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class RecordDetail
    {
        public int Id { get; set; }
        [Required]
        public int RecordId  { get; set; }
        [Required]
        public int BserviceId { get; set; }
       
        [Required]
        public int Price { get; set; }
        public Record Record { get; set; }
        public BService BService { get; set; }

    }
}
