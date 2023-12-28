using Beauty.Models;

namespace Beauty.ViewModels
{
    public class BServiceDisplayModel 
    {
        public IEnumerable<BService> BServices  { get; set; }
        public IEnumerable<TypeService> TypeServices  { get; set; }
        public string STerm { get; set; } = "";
        public int TypeServiceId  { get; set; } = 0;
    }
}
