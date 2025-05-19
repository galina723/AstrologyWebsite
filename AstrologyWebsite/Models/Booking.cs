using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? TarotId { get; set; }

    public int? ServiceId { get; set; }

    public byte? Status { get; set; }

    public string? Note { get; set; }

    public DateTime? ScheduleAt { get; set; }

    public int? Price { get; set; }

    //public virtual User? Customer { get; set; }

    public virtual Service? Service { get; set; }


    //public virtual User? Tarot { get; set; }
}
