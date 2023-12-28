using Beauty.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty.ViewModels
{
    public class MasterViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TypeServiceId { get; set; }  
        public TypeService TypeService { get; internal set; }
    }
}
