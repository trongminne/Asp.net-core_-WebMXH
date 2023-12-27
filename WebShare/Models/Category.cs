using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
