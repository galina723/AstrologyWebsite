using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public byte? Type { get; set; }

    public int? Duration { get; set; }

    public int? Price { get; set; }
    public string Avatar { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
