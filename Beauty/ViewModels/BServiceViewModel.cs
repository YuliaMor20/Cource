using Beauty.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty.ViewModels
{
    public class BServiceViewModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public IFormFile ImageFile { get; set; }
        public int TypeServiceId { get; set; }
        public TypeService TypeService { get; internal set; }
    }
}
