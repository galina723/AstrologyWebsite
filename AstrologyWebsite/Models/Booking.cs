using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstrologyWebsite.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public string? TarotId { get; set; }

    public int? ServiceId { get; set; }

    public byte? Status { get; set; }

    public string? Note { get; set; }

    public DateTime? ScheduleAt { get; set; }

    public int? Price { get; set; }
    [ForeignKey("CustomerId")]
    public virtual AstroUser? Customer { get; set; }
   
    public virtual Service? Service { get; set; }

    [ForeignKey("TarotId")]
    public virtual AstroUser? Tarot { get; set; }
}
