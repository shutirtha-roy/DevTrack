﻿namespace DevTrack.API.Models
{
    public class ProjectResponseModel
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public IList<ProjectModel> Data { get; set; }
        public string[]? Errors { get; set; }
    }
}
