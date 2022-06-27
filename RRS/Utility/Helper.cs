using Microsoft.AspNetCore.Mvc.Rendering;

namespace RRS.Utility
{
    public class Helper
    {
        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 15;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 0; i < 8; i++)
            {
                duration.Add(new SelectListItem() { Text = minute.ToString() + " minutes", Value = minute.ToString() });
                minute += 15;
            }
            
            return duration;
        }
        public static List<SelectListItem> GetTimeDropDown(DateTime start, DateTime end, int interval)
        {
            List<SelectListItem> duration = new List<SelectListItem>();
            DateTime time = start;
            while (time <= end)
            {
                duration.Add(new SelectListItem() { Text = time.ToString("hh:mm tt"), Value = time.ToString() });
                time = time.AddMinutes(interval);
            }
            return duration;
        }


    }
}
