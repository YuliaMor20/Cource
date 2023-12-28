using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class Master
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<BService> BServices { get; set; }

        [ForeignKey("TypeService")]
        public int TypeServiceId { get; set; }  // Сделаем его Nullable
        public TypeService TypeService { get; set; }
    }
}
