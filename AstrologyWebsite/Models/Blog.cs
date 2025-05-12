using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? AuthorId { get; set; }

   // public int? AstroUserId { get; set; }




    //public virtual User? Author { get; set; }

    public virtual ICollection<BlogDetail> BlogDetails { get; set; } = new List<BlogDetail>();
}
