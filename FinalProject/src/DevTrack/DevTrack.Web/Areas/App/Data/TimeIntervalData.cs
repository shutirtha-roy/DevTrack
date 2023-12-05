using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevTrack.Web.Areas.App.Data
{
    public class TimeIntervalData
    {
        public static IEnumerable<SelectListItem> GetTimeIntervals()
        {
            IEnumerable<SelectListItem> listItems = new List<SelectListItem> 
            {
                new SelectListItem{ Value = "5", Text = "5 minutes", Selected = true },
                new SelectListItem{ Value = "10", Text = "10 minutes" },
                new SelectListItem{ Value = "15", Text = "15 minutes" },
                new SelectListItem{ Value = "30", Text = "30 minutes" },
                new SelectListItem{ Value = "60", Text = "60 minutes" },
            };

            return listItems;
        }
    }
}