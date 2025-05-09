using System;
using System.Collections.Generic;

namespace AstrologyWebsite.Models;

public partial class BlogDetail
{
    public int Id { get; set; }

    public int? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }
}
