using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
