using Microsoft.AspNetCore.Mvc.Rendering;
using RRS.Data;

namespace RRS.Areas.Employee.Models
{
    public class SittingsVM
    {
        public SelectList SittingTypes { get; set; }       
        public string Description { get; set; }
        public List<Sitting> Sittings { get; set; }
    }
}
