using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class TypeService 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<BService> BServices { get; set; }
        public ICollection<Master> Masters { get; set; }

    }
}
