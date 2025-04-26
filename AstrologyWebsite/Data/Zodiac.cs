using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Data;

public partial class Zodiac
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public string? Modality { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Element { get; set; }
}
