
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RRS.Areas.Manager.Models
{
    public class SittingVm
    {
        public int SittingId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Capacity { get; set; } = 60;

        public bool IsClosed { get; set; }
        public bool IsPrivate { get; set; }

        public SelectList? SittingTypes { get; set; }
        public int SittingTypeId { get; set; } = 1;
        public bool Repeat { get; set; }

        //Days zero index based for Sunday i.e. Sunday = 0, Monday = 1 etc...
        public bool[] Days { get; set; }
        public int NumberOfWeeks { get; set; } = 1;

    }


}
