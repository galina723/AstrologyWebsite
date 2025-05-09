using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class Planet
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public string? Description { get; set; }
}
