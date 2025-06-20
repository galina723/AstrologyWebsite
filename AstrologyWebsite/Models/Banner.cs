﻿namespace AstrologyWebsite.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BannerType bannerType { get; set; }
        public string Image {  get; set; }
    }
}
