﻿namespace DevTrack.Web.Areas.App.Models
{
    public class MouseActivitiesModel
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public int TotalHits { get; set; }
    }
}