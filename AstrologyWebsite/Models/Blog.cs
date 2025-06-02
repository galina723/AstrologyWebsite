using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Avatar { get; set; }

    public string? AuthorId { get; set; }
    public virtual AstroUser? Author { get; set; }

}
