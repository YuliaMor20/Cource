using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Models;

namespace Beauty.Models
{
    public class Item
    {
       
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public byte[] Image {  get; set; }
        [DisplayName("Цена")]
        public int Price { get; set; }
        public int Availability {  get; set; }
        [DisplayName("Категория")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
       
    }
}
