using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class BService
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public int Time { get; set; }    
        public byte[] Image { get; set; }
        [DisplayName("Цена")]
        public int Price { get; set; }
        [ForeignKey("TypeService")]
        public int TypeServiceId { get; set; }
        public TypeService TypeService { get; set; }
        [NotMapped]
        public List<TimeSlot> AvailableTimeSlots { get; set; }

    }
}
