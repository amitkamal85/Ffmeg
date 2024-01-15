using System;
using System.Collections.Generic;

namespace NotFinal.Models
{
    public partial class VideoConfig
    {
        public int Id { get; set; }
        public string? OriginalVideo { get; set; }
        public string? Title { get; set; }
        public string? Sypnosis { get; set; }
        public string? LowQuality { get; set; }
        public string? MediumQuality { get; set; }
        public string? HighQuality { get; set; }
        public string? VeryHighQuality { get; set; }
        public string? CreatedBy { get; set; }
        public string? Duration { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ProcessingTme { get; set; }
    }
}
